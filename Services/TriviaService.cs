using Grpc.Core;
using GrpcTest.Errors;

namespace GrpcTest.Services
{
    public class TriviaService : GrpcTest.TriviaService.TriviaServiceBase
    {
        // Helper method to base64-decode the JWT and extract id_user_profile and document_number
        private (string idUserProfile, string documentNumber, string errorMsg) ParseJwtFromMetadata(Metadata metadata)
        {
            var authHeader = metadata.FirstOrDefault(m => string.Equals(m.Key, "authorization", StringComparison.OrdinalIgnoreCase));
            if (authHeader == null || string.IsNullOrEmpty(authHeader.Value))
            {
                return (string.Empty, string.Empty, "No autenticado. Token de autenticación no proporcionado.");
            }

            var val = authHeader.Value;
            if (!val.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return (string.Empty, string.Empty, "No autenticado. Formato de token inválido (debe ser 'Bearer <jwt>').");
            }

            var token = val.Substring(7).Trim();
            try
            {
                var parts = token.Split('.');
                if (parts.Length < 2)
                {
                    return (string.Empty, string.Empty, "No autenticado. Token JWT malformado.");
                }

                var payload = parts[1];
                payload = payload.Replace('-', '+').Replace('_', '/');
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }

                var bytes = Convert.FromBase64String(payload);
                var jsonStr = System.Text.Encoding.UTF8.GetString(bytes);

                using var doc = System.Text.Json.JsonDocument.Parse(jsonStr);
                var root = doc.RootElement;

                // Validate expiration claim "exp" if present
                if (root.TryGetProperty("exp", out var expProp))
                {
                    long expSeconds = expProp.GetInt64();
                    var expTime = DateTimeOffset.FromUnixTimeSeconds(expSeconds);
                    if (expTime < BoliviaClock.Now)
                    {
                        return (string.Empty, string.Empty, "No autenticado. El token de autenticación ha expirado.");
                    }
                }

                string idUserProfile = string.Empty;
                if (root.TryGetProperty("id_user_profile", out var idProp))
                {
                    idUserProfile = idProp.GetString() ?? string.Empty;
                }
                else if (root.TryGetProperty("idUserProfile", out var idProp2))
                {
                    idUserProfile = idProp2.GetString() ?? string.Empty;
                }

                string documentNumber = string.Empty;
                if (root.TryGetProperty("document_number", out var docProp))
                {
                    documentNumber = docProp.GetString() ?? string.Empty;
                }
                else if (root.TryGetProperty("documentNumber", out var docProp2))
                {
                    documentNumber = docProp2.GetString() ?? string.Empty;
                }

                if (string.IsNullOrEmpty(idUserProfile))
                {
                    return (string.Empty, string.Empty, "No autenticado. El claim 'id_user_profile' no se encuentra en el token.");
                }

                return (idUserProfile, documentNumber, string.Empty);
            }
            catch (Exception ex)
            {
                return (string.Empty, string.Empty, $"No autenticado. Error al descifrar el token: {ex.Message}");
            }
        }

        public override Task<GetProfileBaseResponsePb> GetProfile(GetProfileRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Task.FromResult(new GetProfileBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR009,
                    Message = errorMsg
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetProfileBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = $"No se encontró un usuario registrado con id_user_profile '{idUserProfile}'."
                });
            }

            TriviaMockDatabase.EnsureWeeklyAttemptsFresh(user);
            string nextResetDate = user.NextAttemptsResetAt.ToString("yyyy-MM-ddTHH:mm:sszzz");

            var response = new GetProfileResponsePb
            {
                User = new UserProfilePb
                {
                    Id = user.IdUserProfile,
                    Name = user.Name,
                    Gender = user.Gender,
                    Age = user.Age,
                    AccountOpeningBranch = user.AccountOpeningBranch
                },
                Quota = new UserQuotaPb
                {
                    WeeklyAttemptsMax = TriviaMockDatabase.WeeklyAttemptsMax,
                    WeeklyAttemptsLeft = user.WeeklyAttemptsLeft,
                    NextResetDate = nextResetDate
                },
                GlobalProgress = new GlobalProgressPb
                {
                    TotalXp = user.TotalXp,
                    TotalYastaCoins = user.TotalYastaCoins
                }
            };

            response.GlobalProgress.Departments.AddRange(user.Departments.Values.OrderBy(d => d.Name));

            var baseResponse = new GetProfileBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000,
                Message = "Perfil recuperado con éxito."
            };

            return Task.FromResult(baseResponse);
        }

        public override Task<GetCurrentQuestionBaseResponsePb> GetCurrentQuestion(GetCurrentQuestionRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR009,
                    Message = errorMsg
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = $"No se encontró un usuario registrado con id_user_profile '{idUserProfile}'."
                });
            }

            TriviaMockDatabase.EnsureWeeklyAttemptsFresh(user);

            string deptCode = string.IsNullOrWhiteSpace(request.DepartmentCode) ? user.AccountOpeningBranch : request.DepartmentCode.ToUpper();

            // Verify department status
            if (user.Departments.TryGetValue(deptCode, out var deptProgress))
            {
                if (deptProgress.Status == "locked")
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR007,
                        Message = "El departamento seleccionado está bloqueado. Debes completar primero la trivia de tu departamento de apertura."
                    });
                }
            }
            else
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR007,
                    Message = $"El código de departamento '{deptCode}' no es válido."
                });
            }

            // Retrieve session
            TriviaSessionState? session = null;
            if (!string.IsNullOrEmpty(request.TriviaSessionId))
            {
                session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
                if (session == null)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR002,
                        Message = "La sesión de trivia no fue encontrada o ha expirado."
                    });
                }
                if (session.IdUserProfile != idUserProfile)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR009,
                        Message = "El token de autenticación no coincide con el creador de la sesión."
                    });
                }
                if (!string.Equals(session.DepartmentCode, deptCode, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR007,
                        Message = $"El department_code '{deptCode}' no corresponde al departamento de la sesión de trivia especificada ('{session.DepartmentCode}')."
                    });
                }
            }
            else
            {
                session = TriviaMockDatabase.GetActiveSessionForDepartment(idUserProfile, deptCode);
            }

            // If starting a new session, verify attempts and deduct
            if (session == null)
            {
                if (user.WeeklyAttemptsLeft <= 0)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR006,
                        Message = "Has agotado tus intentos semanales permitidos para jugar trivias."
                    });
                }

                user.WeeklyAttemptsLeft--;

                session = TriviaMockDatabase.StartNewSession(idUserProfile, deptCode);

                if (deptProgress.Status == "unlocked")
                {
                    deptProgress.Status = "in_progress";
                }
            }

            session.Touch();

            int totalQuestions = TriviaMockDatabase.GetTotalQuestionsCount();
            if (session.CurrentQuestionIndex >= totalQuestions)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR001,
                    Message = "La sesión ya ha finalizado. Por favor reclame sus recompensas."
                });
            }

            var mockQuestion = TriviaMockDatabase.GetQuestionByIndex(session.CurrentQuestionIndex);
            if (mockQuestion == null)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR002,
                    Message = "La pregunta solicitada no se pudo cargar."
                });
            }

            session.LastQuestionId = mockQuestion.Id;

            var response = new GetCurrentQuestionResponsePb
            {
                TriviaSessionId = session.SessionId,
                Department = session.DepartmentCode,
                CurrentQuestionNumber = session.CurrentQuestionIndex + 1,
                TotalQuestions = totalQuestions,
                Question = new TriviaQuestionPb
                {
                    Id = mockQuestion.Id,
                    Category = mockQuestion.Category,
                    CategoryLabel = mockQuestion.CategoryLabel,
                    CategoryIconKey = mockQuestion.CategoryIconKey,
                    Title = mockQuestion.Title
                }
            };

            foreach (var opt in mockQuestion.Options)
            {
                response.Question.Options.Add(new QuestionOptionPb
                {
                    Id = opt.Id,
                    Text = opt.Text
                });
            }

            var baseResponse = new GetCurrentQuestionBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000,
                Message = "Pregunta recuperada con éxito."
            };

            return Task.FromResult(baseResponse);
        }

        public override Task<SubmitAnswerBaseResponsePb> SubmitAnswer(SubmitAnswerRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR009,
                    Message = errorMsg
                });
            }

            var session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
            if (session == null)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR002,
                    Message = "La sesión de trivia no fue encontrada."
                });
            }

            if (session.IdUserProfile != idUserProfile)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR009,
                    Message = "La sesión de trivia especificada no pertenece al usuario autenticado."
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = $"No se encontró un usuario registrado con id_user_profile '{idUserProfile}'."
                });
            }

            // Check session timeout
            if ((BoliviaClock.Now - session.LastAccessedAt) > TriviaMockDatabase.SessionTimeout)
            {
                TriviaMockDatabase.TerminateSession(session.SessionId);
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR005,
                    Message = "La sesión de trivia ha expirado por inactividad."
                });
            }

            // Everything below reads and mutates this session's counters, so it runs under
            // the session's lock — otherwise two concurrent submits for the same question
            // (double tap, client retry) could both read the same CurrentQuestionIndex and
            // each advance/score it independently.
            lock (session.Lock)
            {
                int totalQuestions = TriviaMockDatabase.GetTotalQuestionsCount();
                if (session.CurrentQuestionIndex >= totalQuestions)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR001,
                        Message = "La sesión ya ha finalizado. Por favor reclame sus recompensas."
                    });
                }

                var mockQuestion = TriviaMockDatabase.GetQuestionByIndex(session.CurrentQuestionIndex);
                if (mockQuestion == null)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR002,
                        Message = "No se pudo recuperar la pregunta en curso."
                    });
                }

                // Validate question_id matches current question index
                if (mockQuestion.Id != request.QuestionId)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR003,
                        Message = "La pregunta enviada no corresponde al orden actual de tu sesión de trivia (mismatch de question_id)."
                    });
                }

                // Validate selected_option_id belongs to the question
                bool optionExists = mockQuestion.Options.Any(opt => string.Equals(opt.Id, request.SelectedOptionId, StringComparison.OrdinalIgnoreCase));
                if (!optionExists)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR008,
                        Message = "La opción seleccionada es inválida o no corresponde a las opciones de la pregunta."
                    });
                }

                bool isCorrect = string.Equals(mockQuestion.CorrectOptionId, request.SelectedOptionId, StringComparison.OrdinalIgnoreCase);
                int xpEarned = isCorrect ? 50 : 0;

                if (isCorrect)
                {
                    session.CorrectAnswersCount++;
                }

                // Advance session question pointer
                session.CurrentQuestionIndex++;
                session.Touch();

                bool isFinished = session.CurrentQuestionIndex >= totalQuestions;

                // Update user progress (completed_questions field)
                if (user.Departments.TryGetValue(session.DepartmentCode, out var deptProgress))
                {
                    deptProgress.CompletedQuestions = session.CurrentQuestionIndex;
                }

                var response = new SubmitAnswerResponsePb
                {
                    IsCorrect = isCorrect,
                    CorrectOptionId = mockQuestion.CorrectOptionId,
                    Explanation = mockQuestion.Explanation,
                    IsSessionFinished = isFinished,
                    XpEarned = xpEarned
                };

                var baseResponse = new SubmitAnswerBaseResponsePb
                {
                    Data = response,
                    StatusCode = ErrorCode.SUC000,
                    Message = isCorrect ? "Respuesta correcta registrada." : "Respuesta incorrecta registrada."
                };

                return Task.FromResult(baseResponse);
            }
        }

        public override Task<GetRewardsBaseResponsePb> GetRewards(GetRewardsRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR009,
                    Message = errorMsg
                });
            }

            var session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
            if (session == null)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR002,
                    Message = "La sesión de trivia no fue encontrada."
                });
            }

            if (session.IdUserProfile != idUserProfile)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR009,
                    Message = "La sesión de trivia especificada no pertenece al usuario autenticado."
                });
            }

            // Check session timeout
            if ((BoliviaClock.Now - session.LastAccessedAt) > TriviaMockDatabase.SessionTimeout)
            {
                TriviaMockDatabase.TerminateSession(session.SessionId);
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR005,
                    Message = "La sesión de trivia ha expirado por inactividad."
                });
            }

            int totalQuestions = TriviaMockDatabase.GetTotalQuestionsCount();
            if (session.CurrentQuestionIndex < totalQuestions)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR004,
                    Message = $"La sesión de trivia aún no ha sido completada. Se completaron {session.CurrentQuestionIndex} de {totalQuestions} preguntas."
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = $"No se encontró un usuario registrado con id_user_profile '{idUserProfile}'."
                });
            }

            int score = session.CorrectAnswersCount;
            int xpFromQuestions = score * 50;
            int xpBonus = 100; // completion bonus
            int totalXpEarned = xpFromQuestions + xpBonus;
            int yastaCoinsEarned = score * 10;

            // Increment user total scores
            user.TotalXp += totalXpEarned;
            user.TotalYastaCoins += yastaCoinsEarned;

            // Mark department as completed and unlock others if applicable
            var unlockedDepts = new List<string>();
            if (user.Departments.TryGetValue(session.DepartmentCode, out var deptProgress))
            {
                deptProgress.Status = "completed";
                deptProgress.CompletedQuestions = totalQuestions;

                // If completed department is the opening branch, unlock all other departments
                if (string.Equals(session.DepartmentCode, user.AccountOpeningBranch, StringComparison.OrdinalIgnoreCase))
                {
                    TriviaMockDatabase.UnlockOtherDepartments(user);
                    foreach (var dept in user.Departments.Values)
                    {
                        if (!string.Equals(dept.Code, user.AccountOpeningBranch, StringComparison.OrdinalIgnoreCase))
                        {
                            unlockedDepts.Add(dept.Code);
                        }
                    }
                }
            }

            var response = new GetRewardsResponsePb
            {
                SessionId = session.SessionId,
                Department = session.DepartmentCode,
                Score = new TriviaScorePb
                {
                    CorrectAnswers = score,
                    TotalQuestions = totalQuestions
                },
                RewardsEarned = new RewardsEarnedPb
                {
                    XpBonus = totalXpEarned,
                    YastaCoins = yastaCoinsEarned
                },
                Message = score >= 7
                    ? $"¡Excelente trabajo! Lograste un puntaje de {score}/{totalQuestions} y desbloqueaste altas recompensas."
                    : $"¡Buen intento! Lograste un puntaje de {score}/{totalQuestions}. ¡Sigue aprendiendo y mejora en la siguiente ronda!",
                WeeklyAttemptsLeft = user.WeeklyAttemptsLeft,
                DepartmentStatus = "completed"
            };

            response.UnlockedDepartmentCodes.AddRange(unlockedDepts);

            // Clean up session from database
            TriviaMockDatabase.TerminateSession(session.SessionId);

            var baseResponse = new GetRewardsBaseResponsePb
            {
                Data = response,
                StatusCode = ErrorCode.SUC000,
                Message = "Recompensas liquidadas y reclamadas con éxito."
            };

            return Task.FromResult(baseResponse);
        }
    }
}
