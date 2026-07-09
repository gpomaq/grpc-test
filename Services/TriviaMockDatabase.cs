using System.Collections.Concurrent;

namespace GrpcTest.Services
{
    // Bolivia does not observe daylight saving time, so a fixed UTC-4 offset always holds.
    // Centralizing "now" here keeps every timestamp in the mock expressed in local Bolivia
    // time instead of UTC, matching how the real backend reports dates to clients.
    public static class BoliviaClock
    {
        public static readonly TimeSpan Offset = TimeSpan.FromHours(-4);

        public static DateTimeOffset Now => DateTimeOffset.UtcNow.ToOffset(Offset);
    }

    public class UserState
    {
        public string IdUserProfile { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Name { get; set; } = "Usuario Demo";
        public string Gender { get; set; } = "female";
        public int Age { get; set; } = 28;
        public string AccountOpeningBranch { get; set; } = "LP";
        public int WeeklyAttemptsLeft { get; set; } = 3;
        public DateTimeOffset NextAttemptsResetAt { get; set; }
        public int TotalXp { get; set; } = 150;
        public int TotalYastaCoins { get; set; } = 30;

        // Tracks progress for each department
        public ConcurrentDictionary<string, DepartmentProgressPb> Departments { get; set; } = new();
    }

    public class TriviaSessionState
    {
        public string SessionId { get; set; } = string.Empty;
        public string IdUserProfile { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public int CurrentQuestionIndex { get; set; } = 0;
        public int CorrectAnswersCount { get; set; } = 0;
        public string LastQuestionId { get; set; } = string.Empty;
        public DateTimeOffset LastAccessedAt { get; set; } = BoliviaClock.Now;

        // Per-session shuffle of question ids, generated once when the session starts so each
        // playthrough sees the full question bank in its own random order instead of every
        // department replaying the same fixed sequence.
        public List<string> QuestionOrder { get; set; } = new();

        // Guards read-modify-write of this session's mutable counters (e.g. concurrent
        // SubmitAnswer retries) so a double request can't advance/score a question twice.
        public readonly object Lock = new();

        public void Touch() => LastAccessedAt = BoliviaClock.Now;
    }

    public class MockQuestion
    {
        public string Id { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryLabel { get; set; } = string.Empty;
        public string CategoryIconKey { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<(string Id, string Text)> Options { get; set; } = new();
        public string CorrectOptionId { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
    }

    public static class TriviaMockDatabase
    {
        private static readonly ConcurrentDictionary<string, UserState> Users = new();
        private static readonly ConcurrentDictionary<string, TriviaSessionState> Sessions = new();
        private static readonly ConcurrentDictionary<string, string> DepartmentSessions = new();

        public static readonly TimeSpan SessionTimeout = TimeSpan.FromMinutes(30);

        // Predefined list of 10 mixed questions (Finance, Environment, Gender Equity, Women's Rights)
        private static readonly List<MockQuestion> MockQuestions = new()
        {
            new MockQuestion
            {
                Id = "q1",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "¿Cuál es el principal beneficio del interés compuesto al ahorrar en un banco boliviano?",
                Options = new()
                {
                    ("opt1a", "Calcula intereses únicamente sobre el monto del depósito inicial."),
                    ("opt1b", "Genera intereses sobre el capital inicial y también sobre los intereses acumulados previamente."),
                    ("opt1c", "Garantiza que la inflación del país nunca afectará el valor real de tus ahorros."),
                    ("opt1d", "Se aplica únicamente en préstamos de consumo a muy corto plazo.")
                },
                CorrectOptionId = "opt1b",
                Explanation = "El interés compuesto suma los intereses ganados al capital inicial de forma periódica, haciendo que el interés futuro se calcule sobre un monto mayor."
            },
            new MockQuestion
            {
                Id = "q2",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "En Bolivia, ¿cuál de las siguientes opciones representa una fuente de energía renovable clave en pleno desarrollo en el altiplano?",
                Options = new()
                {
                    ("opt2a", "El carbón mineral."),
                    ("opt2b", "La energía solar fotovoltaica (como en la planta solar de Oruro)."),
                    ("opt2c", "El gas natural licuado."),
                    ("opt2d", "La energía de fisión nuclear.")
                },
                CorrectOptionId = "opt2b",
                Explanation = "Bolivia cuenta con plantas de energía solar (como la de Oruro) que aprovechan la alta radiación solar del altiplano para generar energía limpia y renovable."
            },
            new MockQuestion
            {
                Id = "q3",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Cuál es el propósito central de promover la equidad de género en las empresas e instituciones en Bolivia?",
                Options = new()
                {
                    ("opt3a", "Garantizar igualdad de oportunidades, trato y remuneración justa sin importar el género."),
                    ("opt3b", "Establecer que los hombres trabajen turnos más largos en áreas operativas."),
                    ("opt3c", "Restringir la participación de mujeres en puestos jerárquicos o de toma de decisiones."),
                    ("opt3d", "Dividir las tareas de oficina estrictamente bajo roles tradicionales de género.")
                },
                CorrectOptionId = "opt3a",
                Explanation = "La equidad de género busca eliminar barreras históricas para asegurar oportunidades iguales y salarios justos por el mismo trabajo, beneficiando a toda la sociedad boliviana."
            },
            new MockQuestion
            {
                Id = "q4",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "En Bolivia, ¿qué ley nacional garantiza a las mujeres una vida libre de violencia y tipifica el delito de feminicidio?",
                Options = new()
                {
                    ("opt4a", "La Ley General del Trabajo."),
                    ("opt4b", "La Ley N° 348 (Ley Integral para Garantizar a las Mujeres una Vida Libre de Violencia)."),
                    ("opt4c", "El Código de las Familias."),
                    ("opt4d", "La Ley N° 004 de Lucha contra la Corrupción.")
                },
                CorrectOptionId = "opt4b",
                Explanation = "La Ley N° 348 es la norma integral específica que protege a las mujeres contra todo tipo de violencia física, psicológica, sexual o económica en Bolivia."
            },
            new MockQuestion
            {
                Id = "q5",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "Para una familia boliviana, ¿cuál es el objetivo primordial de elaborar un presupuesto mensual en bolivianos (Bs.)?",
                Options = new()
                {
                    ("opt5a", "Maximizar el límite de uso de las tarjetas de crédito."),
                    ("opt5b", "Planificar y controlar los ingresos frente a los gastos, priorizando el ahorro y pago de deudas."),
                    ("opt5c", "Evitar por completo cualquier tipo de consumo cultural o recreativo familiar."),
                    ("opt5d", "Reportar todos los consumos directamente al Servicio de Impuestos Nacionales.")
                },
                CorrectOptionId = "opt5b",
                Explanation = "Un presupuesto permite ordenar las finanzas familiares, asegurando que se cubran las necesidades básicas y se destine un porcentaje al ahorro antes de gastar."
            },
            new MockQuestion
            {
                Id = "q6",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Cuál es una de las principales amenazas para la biodiversidad del Parque Nacional Madidi en Bolivia?",
                Options = new()
                {
                    ("opt6a", "La reforestación con árboles nativos de la Amazonía."),
                    ("opt6b", "La deforestación ilegal y la contaminación de ríos por minería aurífera sin control."),
                    ("opt6c", "El incremento del turismo ecológico regulado y sostenible."),
                    ("opt6d", "El desarrollo de técnicas agrícolas tradicionales de rotación de cultivos.")
                },
                CorrectOptionId = "opt6b",
                Explanation = "La minería aurífera ilegal que libera mercurio en los ríos y la deforestación representan graves peligros para la fauna y comunidades indígenas del Madidi."
            },
            new MockQuestion
            {
                Id = "q7",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "En el mercado laboral boliviano, ¿qué describe el concepto de 'brecha salarial de género'?",
                Options = new()
                {
                    ("opt7a", "La diferencia promedio en los ingresos percibidos por hombres y mujeres que realizan trabajos de igual valor."),
                    ("opt7b", "La diferencia de edad promedio en la que se jubilan los hombres y las mujeres bolivianas."),
                    ("opt7c", "La cantidad de feriados anuales que corresponden por ley a cada género."),
                    ("opt7d", "La brecha en la cantidad de horas destinadas al descanso semanal.")
                },
                CorrectOptionId = "opt7a",
                Explanation = "La brecha salarial es el indicador que muestra que, en promedio, las mujeres ganan menos que los hombres por realizar trabajos con similares responsabilidades."
            },
            new MockQuestion
            {
                Id = "q8",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿A qué derecho de la mujer boliviana hace referencia específica el principio constitucional de 'paridad y alternancia' en cargos de elección política?",
                Options = new()
                {
                    ("opt8a", "Al derecho a la salud reproductiva."),
                    ("opt8b", "Al derecho a la participación política equitativa en la toma de decisiones y representación pública."),
                    ("opt8c", "Al derecho a percibir el subsidio de lactancia maternal."),
                    ("opt8d", "Al derecho de acceso prioritario a créditos de vivienda social.")
                },
                CorrectOptionId = "opt8b",
                Explanation = "La paridad y alternancia garantizan que las listas de candidatos políticos en Bolivia tengan una distribución equitativa de 50% mujeres y 50% hombres de forma intercalada."
            },
            new MockQuestion
            {
                Id = "q9",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Cuál es el beneficio ambiental más directo de reciclar envases plásticos y latas en nuestras ciudades bolivianas?",
                Options = new()
                {
                    ("opt9a", "Incrementar la temperatura en las zonas urbanas."),
                    ("opt9b", "Reducir la saturación de los vertederos municipales (como Alpacoma o Kara Kara) y ahorrar materias primas."),
                    ("opt9c", "Generar lluvias más frecuentes en los valles del país."),
                    ("opt9d", "Eliminar la necesidad de tratamiento de agua potable.")
                },
                CorrectOptionId = "opt9b",
                Explanation = "Al reciclar, evitamos que estos materiales tarden cientos de años en degradarse en los saturados rellenos sanitarios de ciudades como La Paz o Cochabamba."
            },
            new MockQuestion
            {
                Id = "q10",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "En Bolivia, ¿cuál de las siguientes opciones describe la duración legal de la licencia de maternidad pagada para madres asalariadas?",
                Options = new()
                {
                    ("opt10a", "Un total de 30 días calendario únicamente."),
                    ("opt10b", "Un total de 90 días calendario (45 días antes del parto y 45 días después)."),
                    ("opt10c", "Un total de 180 días calendario."),
                    ("opt10d", "La licencia no tiene una duración fija y depende del acuerdo con el empleador.")
                },
                CorrectOptionId = "opt10b",
                Explanation = "La legislación boliviana otorga a las madres trabajadoras una licencia de maternidad remunerada de 90 días en total, divididos en etapas prenatal y postnatal, garantizando su estabilidad laboral."
            },
            new MockQuestion
            {
                Id = "q11",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "¿Qué entidad administra actualmente los aportes para la jubilación (pensiones) de los trabajadores bolivianos?",
                Options = new()
                {
                    ("opt11a", "La Gestora Pública de la Seguridad Social de Largo Plazo."),
                    ("opt11b", "El Banco Central de Bolivia de forma directa."),
                    ("opt11c", "Cada banco comercial de manera individual, según su criterio."),
                    ("opt11d", "Ninguna entidad; el aporte para la jubilación es completamente voluntario.")
                },
                CorrectOptionId = "opt11a",
                Explanation = "La Gestora Pública de la Seguridad Social de Largo Plazo es la entidad que administra los aportes jubilatorios de los trabajadores en Bolivia."
            },
            new MockQuestion
            {
                Id = "q12",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "Antes de otorgar un préstamo, un banco boliviano consulta la Central de Información Crediticia (CIRC). ¿Para qué sirve esta central?",
                Options = new()
                {
                    ("opt12a", "Para registrar el historial de pagos y el nivel de endeudamiento de las personas."),
                    ("opt12b", "Para funcionar como un club de ahorro voluntario entre clientes."),
                    ("opt12c", "Para cobrar un impuesto adicional sobre cada crédito otorgado."),
                    ("opt12d", "Para emitir un seguro de vida obligatorio junto al préstamo.")
                },
                CorrectOptionId = "opt12a",
                Explanation = "La CIRC, administrada por la ASFI, centraliza el historial crediticio de las personas para que las entidades financieras evalúen el riesgo antes de prestar dinero."
            },
            new MockQuestion
            {
                Id = "q13",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "Muchas familias bolivianas reciben remesas de familiares migrantes en el exterior. ¿Cuál es la recomendación financiera más prudente al recibirlas?",
                Options = new()
                {
                    ("opt13a", "Gastar la totalidad de inmediato en bienes de consumo no esenciales."),
                    ("opt13b", "Destinar una parte al ahorro o a una inversión productiva en lugar de consumirla por completo."),
                    ("opt13c", "Guardar siempre el dinero en efectivo dentro de la casa, sin usar el sistema financiero."),
                    ("opt13d", "Evitar informarse sobre el tipo de cambio o las comisiones de envío.")
                },
                CorrectOptionId = "opt13b",
                Explanation = "Destinar parte de la remesa al ahorro formal o a una inversión productiva ayuda a la familia a generar estabilidad financiera a largo plazo, en vez de depender solo del consumo inmediato."
            },
            new MockQuestion
            {
                Id = "q14",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "Bolivia es reconocida internacionalmente como cuna de las microfinanzas modernas. ¿Qué institución boliviana fue pionera mundial en microcrédito?",
                Options = new()
                {
                    ("opt14a", "BancoSol."),
                    ("opt14b", "El Banco Mercantil Santa Cruz."),
                    ("opt14c", "El Banco de Desarrollo Productivo."),
                    ("opt14d", "El Banco Unión.")
                },
                CorrectOptionId = "opt14a",
                Explanation = "BancoSol, surgido de una ONG de microcrédito en los años 90, es reconocido mundialmente como uno de los primeros bancos comerciales especializados en microfinanzas."
            },
            new MockQuestion
            {
                Id = "q15",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "¿Por qué es importante considerar la inflación al planificar un ahorro de largo plazo en bolivianos?",
                Options = new()
                {
                    ("opt15a", "Porque la inflación reduce el poder adquisitivo del dinero si el ahorro no genera un rendimiento superior a ella."),
                    ("opt15b", "Porque la inflación siempre incrementa automáticamente el valor real de los ahorros."),
                    ("opt15c", "Porque la inflación es un fenómeno que solo afecta las finanzas del Estado."),
                    ("opt15d", "Porque la inflación elimina automáticamente cualquier deuda pendiente.")
                },
                CorrectOptionId = "opt15a",
                Explanation = "Si el rendimiento del ahorro es menor a la inflación, el dinero pierde poder de compra con el tiempo, por lo que es clave buscar instrumentos que protejan o superen ese efecto."
            },
            new MockQuestion
            {
                Id = "q16",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Por qué el Salar de Uyuni es estratégicamente importante para Bolivia en términos de recursos naturales?",
                Options = new()
                {
                    ("opt16a", "Por sus reservas de litio, un mineral clave para las baterías de vehículos eléctricos."),
                    ("opt16b", "Por ser una gran reserva de petróleo crudo."),
                    ("opt16c", "Por su producción histórica de carbón mineral."),
                    ("opt16d", "Por ser el único yacimiento de oro del país.")
                },
                CorrectOptionId = "opt16a",
                Explanation = "El Salar de Uyuni contiene una de las mayores reservas de litio del mundo, un recurso estratégico para la fabricación de baterías y la transición energética global."
            },
            new MockQuestion
            {
                Id = "q17",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Qué evento ambiental afectó gravemente los bosques secos de la Chiquitania, en Santa Cruz, durante 2019?",
                Options = new()
                {
                    ("opt17a", "Grandes incendios forestales originados por quemas agrícolas descontroladas."),
                    ("opt17b", "Una inundación causada por el deshielo de glaciares cercanos."),
                    ("opt17c", "Una plaga masiva de langostas."),
                    ("opt17d", "Un terremoto de gran magnitud.")
                },
                CorrectOptionId = "opt17a",
                Explanation = "En 2019, la Chiquitania sufrió incendios forestales masivos, en gran parte provocados por quemas agrícolas que se salieron de control en época seca, destruyendo millones de hectáreas."
            },
            new MockQuestion
            {
                Id = "q18",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Qué ocurrió con el Lago Poopó, en Oruro, alertando sobre el cambio climático y el mal uso del agua en Bolivia?",
                Options = new()
                {
                    ("opt18a", "Se secó casi por completo debido a la sequía, el desvío de afluentes y el cambio climático."),
                    ("opt18b", "Se congeló de forma permanente durante todo el año."),
                    ("opt18c", "Aumentó su nivel hasta inundar la ciudad de Oruro."),
                    ("opt18d", "Se convirtió en una fuente directa de agua potable para consumo humano.")
                },
                CorrectOptionId = "opt18a",
                Explanation = "El Lago Poopó, antes el segundo más grande de Bolivia, se secó casi en su totalidad por la sequía prolongada, el desvío de sus afluentes para riego y minería, y el cambio climático."
            },
            new MockQuestion
            {
                Id = "q19",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "El glaciar Chacaltaya, cercano a La Paz, es un caso emblemático a nivel mundial porque...",
                Options = new()
                {
                    ("opt19a", "Desapareció por completo debido al calentamiento global, siendo uno de los primeros glaciares tropicales en extinguirse."),
                    ("opt19b", "Aumentó considerablemente su tamaño en los últimos años."),
                    ("opt19c", "Se transformó en una laguna de aguas termales."),
                    ("opt19d", "Nunca tuvo hielo permanente en su historia.")
                },
                CorrectOptionId = "opt19a",
                Explanation = "El Chacaltaya, que llegó a albergar la pista de esquí más alta del mundo, se derritió por completo en la década de 2009, convirtiéndose en un símbolo global del retroceso glaciar por el calentamiento global."
            },
            new MockQuestion
            {
                Id = "q20",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Cuál es la forma correcta de desechar aparatos electrónicos en desuso (celulares, baterías, cargadores) en ciudades bolivianas?",
                Options = new()
                {
                    ("opt20a", "Llevarlos a puntos de acopio o campañas de reciclaje electrónico especializado."),
                    ("opt20b", "Tirarlos junto con la basura orgánica doméstica común."),
                    ("opt20c", "Quemarlos directamente en el patio de la casa."),
                    ("opt20d", "Enterrarlos en cualquier terreno baldío cercano.")
                },
                CorrectOptionId = "opt20a",
                Explanation = "Los residuos electrónicos contienen metales pesados y componentes tóxicos que deben tratarse en puntos de acopio especializados para evitar la contaminación de suelo y agua."
            },
            new MockQuestion
            {
                Id = "q21",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Qué reconoce la normativa laboral boliviana vigente respecto a la licencia de paternidad?",
                Options = new()
                {
                    ("opt21a", "Otorga a los padres trabajadores un permiso remunerado tras el nacimiento de su hijo o hija."),
                    ("opt21b", "No reconoce ningún tipo de licencia para los padres trabajadores."),
                    ("opt21c", "Obliga a los padres a tomar un año sabático sin goce de haberes."),
                    ("opt21d", "Solo aplica para funcionarios públicos de alto rango.")
                },
                CorrectOptionId = "opt21a",
                Explanation = "La normativa laboral boliviana reconoce a los padres trabajadores el derecho a una licencia de paternidad remunerada, promoviendo la corresponsabilidad en el cuidado familiar."
            },
            new MockQuestion
            {
                Id = "q22",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "En el contexto de equidad de género en Bolivia, ¿qué se entiende por 'economía del cuidado' o trabajo doméstico no remunerado?",
                Options = new()
                {
                    ("opt22a", "Las labores del hogar y cuidado familiar que recaen mayoritariamente en mujeres, sin remuneración ni reconocimiento económico."),
                    ("opt22b", "El salario que reciben formalmente las trabajadoras del hogar."),
                    ("opt22c", "Un impuesto que pagan las familias por contratar personal doméstico."),
                    ("opt22d", "Un bono estatal que reciben todas las madres bolivianas.")
                },
                CorrectOptionId = "opt22a",
                Explanation = "La economía del cuidado visibiliza el trabajo doméstico y de cuidado que, históricamente, recae de forma desproporcionada en las mujeres sin remuneración ni valoración económica."
            },
            new MockQuestion
            {
                Id = "q23",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "En Bolivia, ¿qué representa la brecha de género en carreras STEM (ciencia, tecnología, ingeniería y matemáticas)?",
                Options = new()
                {
                    ("opt23a", "La menor proporción de mujeres que estudian y ejercen estas carreras en comparación con los hombres."),
                    ("opt23b", "La diferencia de horarios de clases asignados a hombres y mujeres."),
                    ("opt23c", "La cantidad de universidades públicas frente a las privadas."),
                    ("opt23d", "El número de becas que reciben exclusivamente los hombres por ley.")
                },
                CorrectOptionId = "opt23a",
                Explanation = "La brecha de género en STEM refleja la menor participación histórica de mujeres en estas áreas de estudio y trabajo, pese a que su desempeño es igual de competente que el de los hombres."
            },
            new MockQuestion
            {
                Id = "q24",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Qué describe el término 'techo de cristal' aplicado al ámbito laboral boliviano?",
                Options = new()
                {
                    ("opt24a", "Barreras invisibles que dificultan que las mujeres asciendan a cargos gerenciales o directivos."),
                    ("opt24b", "Un límite legal que impide contratar mujeres en ciertas empresas."),
                    ("opt24c", "Una política de cuotas obligatoria vigente en todas las empresas privadas."),
                    ("opt24d", "El techo físico de las oficinas bancarias en edificios antiguos.")
                },
                CorrectOptionId = "opt24a",
                Explanation = "El 'techo de cristal' describe barreras informales y culturales que limitan el ascenso de las mujeres a puestos de alta dirección, aun cuando cuentan con la misma o mayor preparación que sus pares hombres."
            },
            new MockQuestion
            {
                Id = "q25",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Por qué es relevante fomentar el acceso a crédito y capacitación financiera para mujeres emprendedoras en Bolivia?",
                Options = new()
                {
                    ("opt25a", "Porque históricamente han enfrentado mayores barreras de acceso a financiamiento formal para sus negocios."),
                    ("opt25b", "Porque las mujeres no pueden abrir cuentas bancarias en Bolivia."),
                    ("opt25c", "Porque la ley prohíbe que las mujeres sean propietarias de un negocio."),
                    ("opt25d", "Porque no existen emprendedoras mujeres en el país.")
                },
                CorrectOptionId = "opt25a",
                Explanation = "Las mujeres emprendedoras han enfrentado tradicionalmente mayores barreras para acceder a crédito formal y capacitación, por lo que impulsar su inclusión financiera fortalece la economía familiar y nacional."
            },
            new MockQuestion
            {
                Id = "q26",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Qué busca proteger la Ley N° 243 en Bolivia?",
                Options = new()
                {
                    ("opt26a", "A las mujeres candidatas o electas de acoso y violencia política."),
                    ("opt26b", "Los derechos de propiedad intelectual de las empresas."),
                    ("opt26c", "El acceso universal y gratuito a la salud pública."),
                    ("opt26d", "La jornada laboral máxima de 8 horas diarias.")
                },
                CorrectOptionId = "opt26a",
                Explanation = "La Ley N° 243 (Ley Contra el Acoso y Violencia Política hacia las Mujeres) protege a las mujeres candidatas, electas o en ejercicio de funciones públicas de agresiones destinadas a impedir su participación política."
            },
            new MockQuestion
            {
                Id = "q27",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "En el área rural boliviana, ¿qué representa un avance importante en los derechos patrimoniales de la mujer?",
                Options = new()
                {
                    ("opt27a", "El reconocimiento legal de la copropiedad de la tierra entre hombres y mujeres en los títulos agrarios."),
                    ("opt27b", "La prohibición legal de que las mujeres hereden tierras."),
                    ("opt27c", "La obligación de vender la tierra al cumplir la mayoría de edad."),
                    ("opt27d", "La exclusión de las mujeres de las organizaciones campesinas y comunitarias.")
                },
                CorrectOptionId = "opt27a",
                Explanation = "El reconocimiento de la copropiedad de la tierra a nombre de ambos cónyuges en los títulos agrarios ha sido un avance clave para garantizar la seguridad patrimonial de las mujeres rurales en Bolivia."
            },
            new MockQuestion
            {
                Id = "q28",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Qué son los SLIM (Servicios Legales Integrales Municipales) en Bolivia?",
                Options = new()
                {
                    ("opt28a", "Instancias municipales que brindan atención legal, psicológica y social gratuita a víctimas de violencia."),
                    ("opt28b", "Bancos especializados exclusivamente en créditos para mujeres."),
                    ("opt28c", "Escuelas técnicas destinadas únicamente a mujeres."),
                    ("opt28d", "Un impuesto municipal destinado a financiar obras públicas.")
                },
                CorrectOptionId = "opt28a",
                Explanation = "Los SLIM son instancias de los gobiernos municipales que brindan orientación y atención legal, psicológica y social gratuita, principalmente a mujeres víctimas de violencia."
            },
            new MockQuestion
            {
                Id = "q29",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Qué protección brinda el Código de las Familias boliviano en relación con el matrimonio de niñas y adolescentes?",
                Options = new()
                {
                    ("opt29a", "Establece una edad mínima legal para contraer matrimonio, protegiendo a las menores de uniones forzadas."),
                    ("opt29b", "Permite el matrimonio a cualquier edad si los padres lo autorizan."),
                    ("opt29c", "Obliga a las mujeres a casarse antes de cumplir 18 años."),
                    ("opt29d", "No regula ninguna edad mínima para contraer matrimonio.")
                },
                CorrectOptionId = "opt29a",
                Explanation = "El Código de las Familias establece una edad mínima legal para el matrimonio, como medida de protección frente a uniones forzadas o tempranas de niñas y adolescentes."
            },
            new MockQuestion
            {
                Id = "q30",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Qué protege el concepto de 'violencia obstétrica', reconocido en la normativa de salud boliviana?",
                Options = new()
                {
                    ("opt30a", "El derecho de las mujeres a un trato digno y humanizado durante el embarazo, el parto y el posparto."),
                    ("opt30b", "El derecho exclusivo a que todos los partos sean por cesárea."),
                    ("opt30c", "La gratuidad total de todos los medicamentos genéricos del país."),
                    ("opt30d", "El derecho a elegir el sexo biológico del bebé antes del nacimiento.")
                },
                CorrectOptionId = "opt30a",
                Explanation = "La violencia obstétrica se refiere al maltrato, la falta de información o el trato deshumanizado que puede sufrir una mujer durante la atención de su embarazo, parto o posparto, y su prevención es un derecho reconocido en salud."
            },
            new MockQuestion
            {
                Id = "q31",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "¿Qué cubre el Seguro Obligatorio de Accidentes de Tránsito (SOAT) vigente en Bolivia?",
                Options = new()
                {
                    ("opt31a", "Gastos médicos e indemnizaciones a las víctimas de accidentes de tránsito, sin importar quién tuvo la culpa."),
                    ("opt31b", "Únicamente los daños materiales del vehículo asegurado."),
                    ("opt31c", "El mantenimiento preventivo periódico del vehículo."),
                    ("opt31d", "Las multas de tránsito que el conductor tenga impagas.")
                },
                CorrectOptionId = "opt31a",
                Explanation = "El SOAT cubre la atención médica y las indemnizaciones a las víctimas de un accidente de tránsito, independientemente de la responsabilidad, priorizando la protección de la vida."
            },
            new MockQuestion
            {
                Id = "q32",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "En Bolivia, los pagos mediante código QR interoperable, impulsados por el Banco Central de Bolivia, permiten principalmente...",
                Options = new()
                {
                    ("opt32a", "Realizar transferencias y pagos electrónicos entre distintos bancos y billeteras de forma rápida y sin efectivo."),
                    ("opt32b", "Obtener automáticamente un préstamo bancario preaprobado."),
                    ("opt32c", "Eliminar por completo la necesidad de tener una cuenta registrada en una entidad financiera."),
                    ("opt32d", "Garantizar de forma automática tasas de interés preferenciales.")
                },
                CorrectOptionId = "opt32a",
                Explanation = "El QR interoperable boliviano permite pagar y transferir dinero entre cuentas de distintas entidades financieras de forma inmediata, promoviendo la inclusión financiera y reduciendo el uso de efectivo."
            },
            new MockQuestion
            {
                Id = "q33",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "En el sistema financiero boliviano, ¿cuál es la diferencia entre la 'tasa de interés activa' y la 'tasa de interés pasiva'?",
                Options = new()
                {
                    ("opt33a", "La activa es la que cobra el banco por los préstamos y la pasiva es la que paga por los depósitos de ahorro."),
                    ("opt33b", "Son exactamente el mismo concepto, solo con distinto nombre."),
                    ("opt33c", "La activa solo aplica a empresas y la pasiva únicamente a personas naturales."),
                    ("opt33d", "La tasa pasiva siempre es más alta que la tasa activa.")
                },
                CorrectOptionId = "opt33a",
                Explanation = "La tasa activa es el interés que cobra la entidad financiera al prestar dinero, mientras que la tasa pasiva es el interés que paga a sus clientes por sus depósitos de ahorro o plazo fijo."
            },
            new MockQuestion
            {
                Id = "q34",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "¿Cuál es una señal de alerta común de un esquema de estafa piramidal o financiera en Bolivia?",
                Options = new()
                {
                    ("opt34a", "Prometer rendimientos muy altos y garantizados en poco tiempo, pagando a los primeros participantes con el dinero de los nuevos."),
                    ("opt34b", "Estar registrada y supervisada por la Autoridad de Supervisión del Sistema Financiero (ASFI)."),
                    ("opt34c", "Ofrecer tasas de interés similares a las del mercado financiero formal."),
                    ("opt34d", "Entregar contratos y comprobantes verificables de cada operación realizada.")
                },
                CorrectOptionId = "opt34a",
                Explanation = "Las estafas piramidales suelen atraer víctimas prometiendo ganancias extraordinarias y garantizadas, sosteniéndose únicamente con el dinero de nuevos participantes hasta que colapsan."
            },
            new MockQuestion
            {
                Id = "q35",
                Category = "educacion_financiera",
                CategoryLabel = "Educación Financiera",
                CategoryIconKey = "icon_finance",
                Title = "¿Cuál es el propósito principal de contar con un 'fondo de emergencia' dentro de las finanzas personales?",
                Options = new()
                {
                    ("opt35a", "Cubrir gastos imprevistos, como salud o pérdida de empleo, sin recurrir a deudas costosas."),
                    ("opt35b", "Reemplazar por completo el aporte destinado a la jubilación."),
                    ("opt35c", "Usarse exclusivamente para viajes y vacaciones familiares."),
                    ("opt35d", "Evitar tener que abrir una cuenta de ahorros formal en un banco.")
                },
                CorrectOptionId = "opt35a",
                Explanation = "Un fondo de emergencia brinda respaldo financiero ante imprevistos, evitando que la familia deba endeudarse a tasas altas para cubrir gastos urgentes."
            },
            new MockQuestion
            {
                Id = "q36",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Qué distinción otorgada por la UNESCO tiene el Parque Nacional Noel Kempff Mercado en Bolivia?",
                Options = new()
                {
                    ("opt36a", "Patrimonio Natural de la Humanidad."),
                    ("opt36b", "Patrimonio Cultural Inmaterial de la Humanidad."),
                    ("opt36c", "Reserva mundial oficial de agua potable."),
                    ("opt36d", "Capital mundial de la biodiversidad marina.")
                },
                CorrectOptionId = "opt36a",
                Explanation = "El Parque Nacional Noel Kempff Mercado, en el departamento de Santa Cruz, fue declarado Patrimonio Natural de la Humanidad por la UNESCO gracias a su excepcional biodiversidad."
            },
            new MockQuestion
            {
                Id = "q37",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Cuál es una de las principales causas de la mala calidad del aire en ciudades como La Paz y Cochabamba?",
                Options = new()
                {
                    ("opt37a", "La quema de basura a cielo abierto y las emisiones de los vehículos."),
                    ("opt37b", "La altura sobre el nivel del mar en la que se ubica la ciudad."),
                    ("opt37c", "El exceso de áreas verdes y parques urbanos."),
                    ("opt37d", "La ausencia total de industrias en la región metropolitana.")
                },
                CorrectOptionId = "opt37a",
                Explanation = "La quema de basura, el parque automotor en crecimiento y las emisiones vehiculares son factores clave detrás de los episodios de mala calidad del aire en las principales ciudades bolivianas."
            },
            new MockQuestion
            {
                Id = "q38",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Por qué el retroceso de los glaciares andinos preocupa al abastecimiento de agua de ciudades como La Paz y El Alto?",
                Options = new()
                {
                    ("opt38a", "Porque los glaciares son una fuente natural de agua dulce que alimenta ríos y reservorios usados para consumo humano."),
                    ("opt38b", "Porque los glaciares generan toda la electricidad que consume la ciudad."),
                    ("opt38c", "Porque su deshielo aumenta la producción agrícola de forma indefinida."),
                    ("opt38d", "Porque no tienen ninguna relación con el suministro de agua potable.")
                },
                CorrectOptionId = "opt38a",
                Explanation = "Los glaciares andinos actúan como reservas naturales de agua que alimentan cuencas y sistemas de abastecimiento; su retroceso por el calentamiento global amenaza la disponibilidad futura de agua en las ciudades del altiplano."
            },
            new MockQuestion
            {
                Id = "q39",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "En las zonas productoras de quinua real del altiplano sur boliviano, ¿qué riesgo ambiental genera el monocultivo intensivo sin rotación ni descanso de la tierra?",
                Options = new()
                {
                    ("opt39a", "La erosión y degradación de los suelos, reduciendo su fertilidad a largo plazo."),
                    ("opt39b", "Un aumento indefinido e ilimitado de la fertilidad del suelo."),
                    ("opt39c", "La eliminación total de la necesidad de lluvia para el cultivo."),
                    ("opt39d", "La desaparición completa y permanente de las plagas agrícolas.")
                },
                CorrectOptionId = "opt39a",
                Explanation = "El monocultivo intensivo de quinua sin periodos de descanso ni rotación agota los nutrientes del suelo altiplánico y acelera su erosión, poniendo en riesgo la sostenibilidad de la producción futura."
            },
            new MockQuestion
            {
                Id = "q40",
                Category = "medio_ambiente",
                CategoryLabel = "Medio Ambiente",
                CategoryIconKey = "icon_environment",
                Title = "¿Cuál es el principal objetivo de las áreas protegidas en Bolivia, como los parques nacionales?",
                Options = new()
                {
                    ("opt40a", "Conservar la biodiversidad, los ecosistemas y los recursos naturales para las futuras generaciones."),
                    ("opt40b", "Permitir la explotación minera sin ningún tipo de restricción."),
                    ("opt40c", "Fomentar la construcción urbana descontrolada dentro de sus límites."),
                    ("opt40d", "Eliminar por completo el acceso de las comunidades indígenas a su territorio.")
                },
                CorrectOptionId = "opt40a",
                Explanation = "Las áreas protegidas buscan conservar ecosistemas, especies y recursos naturales estratégicos, muchas veces en coexistencia con el manejo sostenible por parte de comunidades locales e indígenas."
            },
            new MockQuestion
            {
                Id = "q41",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Qué protección debe existir en un entorno laboral boliviano frente al acoso sexual laboral?",
                Options = new()
                {
                    ("opt41a", "Mecanismos de denuncia, protección a la víctima y sanción al agresor dentro de la empresa o institución."),
                    ("opt41b", "La obligación de la víctima de renunciar a su puesto para evitar conflictos."),
                    ("opt41c", "La prohibición de que las mujeres trabajen en equipos mixtos."),
                    ("opt41d", "La exclusión total de este tema de los reglamentos internos de trabajo.")
                },
                CorrectOptionId = "opt41a",
                Explanation = "Un entorno laboral seguro debe contar con canales claros de denuncia, protección a la persona afectada y sanciones efectivas para quien comete acoso sexual laboral."
            },
            new MockQuestion
            {
                Id = "q42",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "En las organizaciones sociales y sindicales campesino-indígenas de Bolivia, ¿qué busca el principio de alternancia de género en cargos de dirigencia?",
                Options = new()
                {
                    ("opt42a", "Garantizar que mujeres y hombres ocupen de forma equitativa y rotativa los cargos de decisión."),
                    ("opt42b", "Que solo los hombres puedan ejercer como dirigentes titulares."),
                    ("opt42c", "Que las mujeres únicamente participen en cargos administrativos menores."),
                    ("opt42d", "Eliminar la participación femenina en las asambleas comunitarias.")
                },
                CorrectOptionId = "opt42a",
                Explanation = "La alternancia de género busca que la representación y toma de decisiones en las organizaciones sociales se distribuya de forma equitativa entre mujeres y hombres."
            },
            new MockQuestion
            {
                Id = "q43",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Qué significa el concepto de 'corresponsabilidad' en las tareas del hogar y el cuidado familiar?",
                Options = new()
                {
                    ("opt43a", "Que todos los miembros del hogar, sin importar su género, comparten de manera equitativa las tareas domésticas y de cuidado."),
                    ("opt43b", "Que solo la madre es responsable de la crianza de los hijos e hijas."),
                    ("opt43c", "Que el Estado asume por completo el cuidado infantil de cada familia."),
                    ("opt43d", "Que las tareas del hogar deben delegarse siempre a terceros contratados.")
                },
                CorrectOptionId = "opt43a",
                Explanation = "La corresponsabilidad implica que las tareas del hogar y el cuidado de hijos u otros familiares se repartan de manera equitativa entre todos los miembros del hogar, sin recaer únicamente en las mujeres."
            },
            new MockQuestion
            {
                Id = "q44",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "En el área rural de Bolivia, ¿qué desafío enfrentan comúnmente las mujeres dedicadas a la agricultura?",
                Options = new()
                {
                    ("opt44a", "Menor acceso a titulación de tierras, créditos agrícolas y asistencia técnica en comparación con los hombres."),
                    ("opt44b", "Una participación laboral nula en las actividades agrícolas familiares."),
                    ("opt44c", "Una remuneración siempre mayor a la de los hombres del mismo sector."),
                    ("opt44d", "La prohibición legal de trabajar la tierra en el área rural.")
                },
                CorrectOptionId = "opt44a",
                Explanation = "Las mujeres rurales bolivianas frecuentemente enfrentan mayores barreras para acceder a la titulación de tierras, créditos productivos y asistencia técnica, pese a su rol activo en la agricultura familiar."
            },
            new MockQuestion
            {
                Id = "q45",
                Category = "equidad_genero",
                CategoryLabel = "Equidad de Género",
                CategoryIconKey = "icon_gender",
                Title = "¿Qué describe la 'brecha digital de género' en el contexto boliviano?",
                Options = new()
                {
                    ("opt45a", "La menor proporción de mujeres, especialmente en áreas rurales, con acceso y habilidades para usar internet y tecnología digital."),
                    ("opt45b", "La diferencia de precios de internet entre hombres y mujeres."),
                    ("opt45c", "Una ley que prohíbe a las mujeres comprar equipos celulares."),
                    ("opt45d", "El número de antenas de telefonía instaladas por departamento.")
                },
                CorrectOptionId = "opt45a",
                Explanation = "La brecha digital de género refleja que las mujeres, sobre todo en zonas rurales, tienen menor acceso a internet, dispositivos y capacitación digital que los hombres, limitando sus oportunidades educativas y económicas."
            },
            new MockQuestion
            {
                Id = "q46",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Qué es la FELCV (Fuerza Especial de Lucha Contra la Violencia) en Bolivia?",
                Options = new()
                {
                    ("opt46a", "Una unidad policial especializada en la atención y protección de víctimas de violencia intrafamiliar y de género."),
                    ("opt46b", "Un banco estatal de créditos exclusivos para mujeres."),
                    ("opt46c", "Una ONG internacional sin relación con el Estado boliviano."),
                    ("opt46d", "Un impuesto destinado a la construcción de escuelas.")
                },
                CorrectOptionId = "opt46a",
                Explanation = "La FELCV es una unidad especializada de la Policía Boliviana dedicada a la prevención, atención y protección de víctimas de violencia intrafamiliar y de género."
            },
            new MockQuestion
            {
                Id = "q47",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Cuál es la función de las 'casas de acogida' para mujeres en situación de violencia en Bolivia?",
                Options = new()
                {
                    ("opt47a", "Brindar refugio temporal, protección y contención a mujeres y sus hijos e hijas mientras se resuelve su situación de riesgo."),
                    ("opt47b", "Ser un centro de detención para las víctimas de violencia."),
                    ("opt47c", "Funcionar como oficinas exclusivas de trámites de divorcio."),
                    ("opt47d", "Cobrar una tarifa mensual por el servicio de protección brindado.")
                },
                CorrectOptionId = "opt47a",
                Explanation = "Las casas de acogida ofrecen refugio temporal, seguridad y apoyo integral a mujeres víctimas de violencia y sus hijos e hijas mientras se gestiona su protección definitiva."
            },
            new MockQuestion
            {
                Id = "q48",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Qué garantiza el derecho a la salud sexual y reproductiva de las mujeres en Bolivia?",
                Options = new()
                {
                    ("opt48a", "El acceso a información, educación y servicios de salud para decidir libre y responsablemente sobre su cuerpo y su maternidad."),
                    ("opt48b", "La obligación de tener un número mínimo de hijos por familia."),
                    ("opt48c", "La prohibición total del acceso a métodos anticonceptivos."),
                    ("opt48d", "La eliminación de los controles prenatales gratuitos.")
                },
                CorrectOptionId = "opt48a",
                Explanation = "El derecho a la salud sexual y reproductiva garantiza a las mujeres información, educación y acceso a servicios de salud para tomar decisiones libres e informadas sobre su cuerpo y maternidad."
            },
            new MockQuestion
            {
                Id = "q49",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "Bolivia es reconocida a nivel mundial por tener una de las proporciones más altas de mujeres en su Asamblea Legislativa Plurinacional. ¿A qué principio constitucional se debe principalmente este logro?",
                Options = new()
                {
                    ("opt49a", "Al principio de paridad y alternancia de género aplicado a las listas de candidaturas."),
                    ("opt49b", "A una cuota informal que no está respaldada por ninguna ley."),
                    ("opt49c", "A la decisión voluntaria de cada partido político, sin obligación legal alguna."),
                    ("opt49d", "A un sorteo aleatorio de candidatos sin ningún criterio de género.")
                },
                CorrectOptionId = "opt49a",
                Explanation = "El principio constitucional de paridad y alternancia obliga a que las listas de candidaturas se conformen de manera equitativa entre hombres y mujeres, lo que llevó a Bolivia a tener una de las asambleas legislativas con mayor proporción de mujeres del mundo."
            },
            new MockQuestion
            {
                Id = "q50",
                Category = "derechos_mujer",
                CategoryLabel = "Derechos de la Mujer",
                CategoryIconKey = "icon_women",
                Title = "¿Qué se entiende por acoso sexual callejero, un tipo de violencia que afecta a muchas mujeres en el transporte público y las calles de Bolivia?",
                Options = new()
                {
                    ("opt50a", "Conductas de connotación sexual no deseadas, como comentarios, gestos o tocamientos, ejercidas en espacios públicos sin consentimiento."),
                    ("opt50b", "Cualquier saludo cordial entre desconocidos en la calle."),
                    ("opt50c", "Una infracción de tránsito relacionada con el exceso de velocidad."),
                    ("opt50d", "Un delito que solo puede ocurrir dentro de una vivienda particular.")
                },
                CorrectOptionId = "opt50a",
                Explanation = "El acoso sexual callejero abarca conductas de connotación sexual no deseadas y ejercidas sin consentimiento en espacios públicos, afectando la libertad y seguridad de las mujeres en su vida cotidiana."
            }
        };

        static TriviaMockDatabase()
        {
            var timer = new System.Threading.Timer(state =>
            {
                try
                {
                    var now = BoliviaClock.Now;
                    var expired = Sessions.Where(kv => (now - kv.Value.LastAccessedAt) > SessionTimeout)
                                          .Select(kv => kv.Key)
                                          .ToList();
                    foreach (var id in expired)
                    {
                        Sessions.TryRemove(id, out var _);
                        var deptEntries = DepartmentSessions.Where(d => d.Value == id).Select(d => d.Key).ToList();
                        foreach (var dept in deptEntries)
                        {
                            DepartmentSessions.TryRemove(dept, out var _);
                        }
                    }
                }
                catch { }
            }, null, SessionTimeout, SessionTimeout);
        }

        // Maximum trivia attempts a user gets per week before NextAttemptsResetAt.
        public const int WeeklyAttemptsMax = 3;

        // Computes the next Monday 00:00:00 in Bolivia local time — used both to seed a new
        // user's quota window and to recompute it once the current window has elapsed.
        public static DateTimeOffset ComputeNextWeeklyResetAt()
        {
            var now = BoliviaClock.Now;
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)now.DayOfWeek + 7) % 7;
            if (daysUntilMonday == 0) daysUntilMonday = 7;
            var nextMonday = now.Date.AddDays(daysUntilMonday);
            return new DateTimeOffset(nextMonday, BoliviaClock.Offset);
        }

        // Lazily resets the weekly quota once its window has elapsed. There's no persistence
        // or cron here, so the reset is computed on read/write access instead — the same
        // "check on access" pattern a real backend would use for a per-user counter.
        public static void EnsureWeeklyAttemptsFresh(UserState user)
        {
            if (BoliviaClock.Now >= user.NextAttemptsResetAt)
            {
                user.WeeklyAttemptsLeft = WeeklyAttemptsMax;
                user.NextAttemptsResetAt = ComputeNextWeeklyResetAt();
            }
        }

        // Only these two identities exist in the mock "database" — mirrors a real backend
        // where id_user_profile must resolve to an actual registered account.
        private sealed record UserSeed(string DocumentNumber, string Name, string Gender, int Age);

        private static readonly Dictionary<string, UserSeed> ValidUsers = new()
        {
            { "1", new UserSeed("123456", "John", "male", 28) },
            { "2", new UserSeed("654321", "Fiora", "female", 34) }
        };

        public static UserState? GetOrCreateUser(string idUserProfile)
        {
            if (!ValidUsers.TryGetValue(idUserProfile, out var seed))
            {
                return null;
            }

            return Users.GetOrAdd(idUserProfile, id =>
            {
                var state = new UserState
                {
                    IdUserProfile = id,
                    DocumentNumber = seed.DocumentNumber,
                    Name = seed.Name,
                    Gender = seed.Gender,
                    Age = seed.Age,
                    AccountOpeningBranch = "LP",
                    WeeklyAttemptsLeft = WeeklyAttemptsMax,
                    NextAttemptsResetAt = ComputeNextWeeklyResetAt(),
                    TotalXp = 250,
                    TotalYastaCoins = 50
                };

                InitializeUserDepartments(state);
                return state;
            });
        }

        public static void InitializeUserDepartments(UserState user)
        {
            var deptCodes = new[] { "LP", "SC", "CB", "OR", "PT", "TJ", "CH", "BE", "PD" };
            var deptNames = new Dictionary<string, string>
            {
                { "LP", "La Paz" }, { "SC", "Santa Cruz" }, { "CB", "Cochabamba" },
                { "OR", "Oruro" }, { "PT", "Potosí" }, { "TJ", "Tarija" },
                { "CH", "Chuquisaca" }, { "BE", "Beni" }, { "PD", "Pando" }
            };

            foreach (var code in deptCodes)
            {
                bool isOpening = string.Equals(code, user.AccountOpeningBranch, StringComparison.OrdinalIgnoreCase);
                var dept = new DepartmentProgressPb
                {
                    Code = code,
                    Name = deptNames[code],
                    Status = isOpening ? "unlocked" : "locked",
                    CompletedQuestions = 0,
                    TotalQuestions = MockQuestions.Count
                };
                user.Departments[code] = dept;
            }
        }

        public static void UnlockOtherDepartments(UserState user)
        {
            foreach (var kv in user.Departments)
            {
                if (kv.Value.Status == "locked")
                {
                    kv.Value.Status = "unlocked";
                }
            }
        }

        public static TriviaSessionState? GetSession(string sessionId)
        {
            if (Sessions.TryGetValue(sessionId, out var session))
            {
                return session;
            }
            return null;
        }

        public static TriviaSessionState? GetActiveSessionForDepartment(string idUserProfile, string departmentCode)
        {
            string key = $"{idUserProfile}_{departmentCode.ToUpper()}";
            if (DepartmentSessions.TryGetValue(key, out string? sessionId))
            {
                var session = GetSession(sessionId);
                if (session != null && session.CurrentQuestionIndex < MockQuestions.Count)
                {
                    return session;
                }
            }
            return null;
        }

        // Fisher-Yates shuffle over the question ids so every new session gets its own random
        // playthrough order instead of replaying the same fixed sequence every time.
        private static List<string> ShuffleQuestionIds()
        {
            var ids = MockQuestions.Select(q => q.Id).ToList();
            for (int i = ids.Count - 1; i > 0; i--)
            {
                int j = Random.Shared.Next(i + 1);
                (ids[i], ids[j]) = (ids[j], ids[i]);
            }
            return ids;
        }

        public static TriviaSessionState StartNewSession(string idUserProfile, string departmentCode)
        {
            string sessionId = Guid.NewGuid().ToString();
            var session = new TriviaSessionState
            {
                SessionId = sessionId,
                IdUserProfile = idUserProfile,
                DepartmentCode = departmentCode.ToUpper(),
                CurrentQuestionIndex = 0,
                CorrectAnswersCount = 0,
                QuestionOrder = ShuffleQuestionIds()
            };

            Sessions[sessionId] = session;
            string key = $"{idUserProfile}_{departmentCode.ToUpper()}";
            DepartmentSessions[key] = sessionId;

            return session;
        }

        public static void TerminateSession(string sessionId)
        {
            if (Sessions.TryRemove(sessionId, out var session))
            {
                string key = $"{session.IdUserProfile}_{session.DepartmentCode}";
                DepartmentSessions.TryRemove(key, out _);
            }
        }

        public static List<MockQuestion> GetQuestions() => MockQuestions;

        public static MockQuestion? GetQuestion(string questionId)
        {
            return MockQuestions.FirstOrDefault(q => q.Id == questionId);
        }

        // Resolves the question at a given position within a session's own shuffled order,
        // instead of the global (fixed) MockQuestions order.
        public static MockQuestion? GetQuestionForSession(TriviaSessionState session, int index)
        {
            if (index < 0 || index >= session.QuestionOrder.Count)
                return null;
            return GetQuestion(session.QuestionOrder[index]);
        }

        public static int GetTotalQuestionsCount() => MockQuestions.Count;
    }
}
