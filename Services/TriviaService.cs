using Grpc.Core;
using GrpcTest.Errors;

namespace GrpcTest.Services
{
    public class TriviaService : GrpcTest.TriviaService.TriviaServiceBase
    {
        // Only validates presence of the token and of the id_user_profile claim.
        // Format/signature/expiration are the gateway's responsibility, not ours.
        private (string idUserProfile, string documentNumber, string errorCode, string errorMsg) ParseJwtFromMetadata(Metadata metadata)
        {
            var authHeader = metadata.FirstOrDefault(m => string.Equals(m.Key, "authorization", StringComparison.OrdinalIgnoreCase));
            if (authHeader == null || string.IsNullOrEmpty(authHeader.Value))
            {
                return (string.Empty, string.Empty, ErrorCode.ERR009, ErrorMessage.ERR009);
            }

            var val = authHeader.Value;
            var token = val.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? val.Substring(7).Trim() : val.Trim();

            try
            {
                var parts = token.Split('.');
                if (parts.Length < 2)
                {
                    return (string.Empty, string.Empty, ErrorCode.ERR011, ErrorMessage.ERR011);
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
                    return (string.Empty, string.Empty, ErrorCode.ERR011, ErrorMessage.ERR011);
                }

                return (idUserProfile, documentNumber, string.Empty, string.Empty);
            }
            catch (Exception)
            {
                return (string.Empty, string.Empty, ErrorCode.ERR011, ErrorMessage.ERR011);
            }
        }

        public override Task<GetProfileBaseResponsePb> GetProfile(GetProfileRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new GetProfileBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetProfileBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = string.Format(ErrorMessage.ERR010, idUserProfile)
                });
            }

            TriviaMockDatabase.EnsureWeeklyAttemptsFresh(user);
            string nextResetDate = user.NextAttemptsResetAt.ToString("dd/MM/yyyy");

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
                StatusCode = ErrorCode.SUC000
            };

            return Task.FromResult(baseResponse);
        }

        public override Task<GetCurrentQuestionBaseResponsePb> GetCurrentQuestion(GetCurrentQuestionRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = string.Format(ErrorMessage.ERR010, idUserProfile)
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
                        Message = ErrorMessage.ERR007
                    });
                }
            }
            else
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR013,
                    Message = string.Format(ErrorMessage.ERR013, deptCode)
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
                        Message = ErrorMessage.ERR002
                    });
                }
                if (session.IdUserProfile != idUserProfile)
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR015,
                        Message = ErrorMessage.ERR015
                    });
                }
                if (!string.Equals(session.DepartmentCode, deptCode, StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR014,
                        Message = string.Format(ErrorMessage.ERR014, deptCode, session.DepartmentCode)
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
                        Message = ErrorMessage.ERR006
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
                    Message = ErrorMessage.ERR001
                });
            }

            var mockQuestion = TriviaMockDatabase.GetQuestionForSession(session, session.CurrentQuestionIndex);
            if (mockQuestion == null)
            {
                return Task.FromResult(new GetCurrentQuestionBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR012,
                    Message = ErrorMessage.ERR012
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
                StatusCode = ErrorCode.SUC000
            };

            return Task.FromResult(baseResponse);
        }

        public override Task<SubmitAnswerBaseResponsePb> SubmitAnswer(SubmitAnswerRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
            if (session == null)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR002,
                    Message = ErrorMessage.ERR002
                });
            }

            if (session.IdUserProfile != idUserProfile)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR015,
                    Message = ErrorMessage.ERR015
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = string.Format(ErrorMessage.ERR010, idUserProfile)
                });
            }

            // Check session timeout
            if ((BoliviaClock.Now - session.LastAccessedAt) > TriviaMockDatabase.SessionTimeout)
            {
                TriviaMockDatabase.TerminateSession(session.SessionId);
                return Task.FromResult(new SubmitAnswerBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR005,
                    Message = ErrorMessage.ERR005
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
                        Message = ErrorMessage.ERR001
                    });
                }

                var mockQuestion = TriviaMockDatabase.GetQuestionForSession(session, session.CurrentQuestionIndex);
                if (mockQuestion == null)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR012,
                        Message = ErrorMessage.ERR012
                    });
                }

                // Validate question_id matches current question index
                if (mockQuestion.Id != request.QuestionId)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR003,
                        Message = ErrorMessage.ERR003
                    });
                }

                // Validate selected_option_id belongs to the question
                bool optionExists = mockQuestion.Options.Any(opt => string.Equals(opt.Id, request.SelectedOptionId, StringComparison.OrdinalIgnoreCase));
                if (!optionExists)
                {
                    return Task.FromResult(new SubmitAnswerBaseResponsePb
                    {
                        StatusCode = ErrorCode.ERR008,
                        Message = ErrorMessage.ERR008
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
                    StatusCode = ErrorCode.SUC000
                };

                return Task.FromResult(baseResponse);
            }
        }

        public override Task<GetRewardsBaseResponsePb> GetRewards(GetRewardsRequestPb request, ServerCallContext context)
        {
            var (idUserProfile, _, errorCode, errorMsg) = ParseJwtFromMetadata(context.RequestHeaders);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = errorCode,
                    Message = errorMsg
                });
            }

            var session = TriviaMockDatabase.GetSession(request.TriviaSessionId);
            if (session == null)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR002,
                    Message = ErrorMessage.ERR002
                });
            }

            if (session.IdUserProfile != idUserProfile)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR015,
                    Message = ErrorMessage.ERR015
                });
            }

            // Check session timeout
            if ((BoliviaClock.Now - session.LastAccessedAt) > TriviaMockDatabase.SessionTimeout)
            {
                TriviaMockDatabase.TerminateSession(session.SessionId);
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR005,
                    Message = ErrorMessage.ERR005
                });
            }

            int totalQuestions = TriviaMockDatabase.GetTotalQuestionsCount();
            if (session.CurrentQuestionIndex < totalQuestions)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR004,
                    Message = string.Format(ErrorMessage.ERR004, session.CurrentQuestionIndex, totalQuestions)
                });
            }

            var user = TriviaMockDatabase.GetOrCreateUser(idUserProfile);
            if (user == null)
            {
                return Task.FromResult(new GetRewardsBaseResponsePb
                {
                    StatusCode = ErrorCode.ERR010,
                    Message = string.Format(ErrorMessage.ERR010, idUserProfile)
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
                StatusCode = ErrorCode.SUC000
            };

            return Task.FromResult(baseResponse);
        }
    }
}
