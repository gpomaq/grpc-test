using static GrpcTest.Services.TriviaCatalog;

namespace GrpcTest.Services
{
    public static partial class QuestionBank
    {
        private static TriviaCategory BuildEquidadGenero() => new TriviaCategory
        {
            Code = "equidad_genero",
            Label = "Equidad de Género",
            IconKey = "icon_gender",
            Description = "Igualdad de oportunidades en el trabajo, el hogar, el liderazgo y la educación.",
            Topics = new List<TriviaTopic>
            {
                new TriviaTopic
                {
                    Id = "eg_trabajo",
                    Title = "Equidad laboral",
                    Description = "Brecha salarial, acceso al empleo y ambientes de trabajo libres de discriminación.",
                    IconKey = "icon_work",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: equidad laboral, derechos y buenas prácticas",
                        Description = "Imagen sobre brecha salarial, igualdad de oportunidades en la contratación y mecanismos de prevención del acoso laboral en Bolivia.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/eg_trabajo_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("eg_trabajo_q01",
                            "¿Cuál es el propósito central de promover la equidad de género en las empresas e instituciones en Bolivia?",
                            new[]
                            {
                                "Garantizar igualdad de oportunidades, trato y remuneración justa sin importar el género.",
                                "Establecer que los hombres trabajen turnos más largos en áreas operativas.",
                                "Restringir la participación de mujeres en puestos jerárquicos o de toma de decisiones.",
                                "Dividir las tareas de oficina estrictamente bajo roles tradicionales de género."
                            }, 0,
                            "La equidad de género busca eliminar barreras históricas para asegurar oportunidades iguales y salarios justos por el mismo trabajo, beneficiando a toda la sociedad boliviana."),
                        Q("eg_trabajo_q02",
                            "En el mercado laboral boliviano, ¿qué describe el concepto de 'brecha salarial de género'?",
                            new[]
                            {
                                "La diferencia promedio en los ingresos percibidos por hombres y mujeres que realizan trabajos de igual valor.",
                                "La diferencia de edad promedio en la que se jubilan hombres y mujeres.",
                                "La cantidad de feriados anuales que corresponden por ley a cada género.",
                                "La brecha en la cantidad de horas destinadas al descanso semanal."
                            }, 0,
                            "La brecha salarial muestra que, en promedio, las mujeres ganan menos que los hombres por realizar trabajos con similares responsabilidades."),
                        Q("eg_trabajo_q03",
                            "¿Qué protección debe existir en un entorno laboral boliviano frente al acoso sexual laboral?",
                            new[]
                            {
                                "Mecanismos de denuncia, protección a la víctima y sanción al agresor dentro de la empresa o institución.",
                                "La obligación de la víctima de renunciar a su puesto para evitar conflictos.",
                                "La prohibición de que las mujeres trabajen en equipos mixtos.",
                                "La exclusión total de este tema de los reglamentos internos de trabajo."
                            }, 0,
                            "Un entorno laboral seguro debe contar con canales claros de denuncia, protección a la persona afectada y sanciones efectivas para quien comete acoso sexual laboral."),
                        Q("eg_trabajo_q04",
                            "¿Por qué es relevante fomentar el acceso a crédito y capacitación financiera para mujeres emprendedoras en Bolivia?",
                            new[]
                            {
                                "Porque históricamente han enfrentado mayores barreras de acceso a financiamiento formal para sus negocios.",
                                "Porque las mujeres no pueden abrir cuentas bancarias en Bolivia.",
                                "Porque la ley prohíbe que las mujeres sean propietarias de un negocio.",
                                "Porque no existen emprendedoras mujeres en el país."
                            }, 0,
                            "Las mujeres emprendedoras han enfrentado tradicionalmente mayores barreras para acceder a crédito formal y capacitación, por lo que impulsar su inclusión financiera fortalece la economía familiar y nacional."),
                        Q("eg_trabajo_q05",
                            "En el área rural de Bolivia, ¿qué desafío enfrentan comúnmente las mujeres dedicadas a la agricultura?",
                            new[]
                            {
                                "Menor acceso a titulación de tierras, créditos agrícolas y asistencia técnica en comparación con los hombres.",
                                "Una participación laboral nula en las actividades agrícolas familiares.",
                                "Una remuneración siempre mayor a la de los hombres del mismo sector.",
                                "La prohibición legal de trabajar la tierra en el área rural."
                            }, 0,
                            "Las mujeres rurales bolivianas enfrentan mayores barreras para acceder a titulación de tierras, créditos productivos y asistencia técnica, pese a su rol activo en la agricultura familiar."),
                        Q("eg_trabajo_q06",
                            "¿Qué significa el principio de 'a igual trabajo, igual remuneración'?",
                            new[]
                            {
                                "Que dos personas que desempeñan funciones de igual valor deben percibir el mismo salario, sin distinción de género.",
                                "Que todos los empleados de una empresa deben ganar exactamente lo mismo.",
                                "Que el salario debe fijarse según la antigüedad únicamente.",
                                "Que los aumentos salariales se sortean entre el personal."
                            }, 0,
                            "El principio no exige salarios idénticos para todos, sino que el género no sea un factor que determine diferencias de remuneración por trabajos equivalentes."),
                        Q("eg_trabajo_q07",
                            "La 'segregación ocupacional' por género se refiere a...",
                            new[]
                            {
                                "La concentración de mujeres y hombres en distintos rubros u ocupaciones por estereotipos, con distinta valoración salarial.",
                                "La separación física de oficinas por género.",
                                "La prohibición de contratar personal de un solo género.",
                                "El uso de uniformes distintos según el cargo."
                            }, 0,
                            "Las ocupaciones feminizadas (cuidado, limpieza, educación inicial) suelen estar peor remuneradas, lo que explica parte de la brecha salarial."),
                        Q("eg_trabajo_q08",
                            "¿Qué establece la normativa laboral boliviana respecto a la estabilidad de la trabajadora en estado de gestación?",
                            new[]
                            {
                                "Goza de inamovilidad laboral durante el embarazo y hasta que su hijo o hija cumpla un año de edad.",
                                "Puede ser despedida libremente durante el embarazo.",
                                "Debe renunciar obligatoriamente al confirmar el embarazo.",
                                "Pierde todos sus beneficios sociales al dar a luz."
                            }, 0,
                            "La inamovilidad laboral protege a madres y padres de familia en ese periodo, evitando despidos por causa del embarazo o del nacimiento."),
                        Q("eg_trabajo_q09",
                            "En un proceso de selección de personal, una práctica que promueve la equidad es...",
                            new[]
                            {
                                "Evaluar competencias y experiencia con criterios objetivos, sin preguntar por estado civil, embarazo o planes familiares.",
                                "Preferir siempre candidatos de un género específico.",
                                "Consultar sobre planes de maternidad antes de contratar.",
                                "Publicar avisos dirigidos exclusivamente a hombres."
                            }, 0,
                            "Preguntar por embarazo o planes familiares es una práctica discriminatoria frecuente que limita el acceso de las mujeres al empleo formal."),
                        Q("eg_trabajo_q10",
                            "¿Qué se entiende por 'trabajo del hogar remunerado' en Bolivia?",
                            new[]
                            {
                                "Una actividad laboral reconocida por ley, con derecho a salario, descanso, aguinaldo y seguridad social.",
                                "Una ayuda familiar informal sin ningún derecho laboral.",
                                "Una actividad exclusiva de personas menores de edad.",
                                "Un trabajo que no puede formalizarse mediante contrato."
                            }, 0,
                            "La Ley N° 2450 regula el trabajo asalariado del hogar en Bolivia, reconociendo derechos laborales a un sector históricamente invisibilizado y mayoritariamente femenino."),
                        Q("eg_trabajo_q11",
                            "¿Qué impacto tiene la informalidad laboral sobre las mujeres en Bolivia?",
                            new[]
                            {
                                "Las expone a menores ingresos, sin seguro de salud, aportes a pensiones ni protección ante despidos.",
                                "Les garantiza mejores condiciones que el empleo formal.",
                                "No representa ninguna diferencia respecto del trabajo formal.",
                                "Solo afecta a los trabajadores hombres."
                            }, 0,
                            "Una alta proporción de mujeres trabaja en el sector informal, lo que se traduce en ingresos inestables y ausencia de cobertura de seguridad social a lo largo de su vida."),
                        Q("eg_trabajo_q12",
                            "Las salas de lactancia y los horarios de lactancia en el trabajo buscan...",
                            new[]
                            {
                                "Permitir que las madres trabajadoras continúen la lactancia sin abandonar su empleo.",
                                "Reducir el salario de la trabajadora durante ese periodo.",
                                "Sustituir la licencia de maternidad.",
                                "Ser un beneficio opcional sin ningún sustento normativo."
                            }, 0,
                            "El derecho a la hora de lactancia es una medida concreta de conciliación entre vida laboral y familiar que ayuda a evitar la salida de mujeres del mercado laboral."),
                        Q("eg_trabajo_q13",
                            "¿Qué es una política de equidad de género dentro de una empresa?",
                            new[]
                            {
                                "Un conjunto de medidas formales para asegurar igualdad en contratación, remuneración, ascensos y prevención del acoso.",
                                "Una campaña publicitaria dirigida a clientas mujeres.",
                                "Un descuento en productos para empleadas mujeres.",
                                "Una obligación de contratar únicamente mujeres."
                            }, 0,
                            "Una política efectiva se mide con indicadores: composición por género en cada nivel, brecha salarial interna y número de casos atendidos, no solo con declaraciones."),
                        Q("eg_trabajo_q14",
                            "La 'penalización por maternidad' en el mercado laboral describe...",
                            new[]
                            {
                                "La pérdida de ingresos y oportunidades de ascenso que muchas mujeres experimentan tras tener hijos.",
                                "Una multa que paga la empresa al Estado por cada nacimiento.",
                                "Un descuento salarial legalmente establecido.",
                                "Un beneficio adicional otorgado a las madres trabajadoras."
                            }, 0,
                            "Los estudios muestran que la trayectoria salarial de las mujeres suele quebrarse tras la maternidad, mientras que la de los hombres no se ve afectada de la misma forma."),
                        Q("eg_trabajo_q15",
                            "¿Por qué se recomienda medir y publicar la brecha salarial interna de una organización?",
                            new[]
                            {
                                "Porque lo que se mide se puede corregir: transparentar la brecha permite fijar metas y hacer seguimiento.",
                                "Porque es un requisito para exportar productos.",
                                "Porque obliga a reducir los salarios más altos.",
                                "Porque reemplaza la necesidad de políticas internas."
                            }, 0,
                            "Sin datos desagregados por género no es posible identificar en qué niveles o áreas se produce la desigualdad ni evaluar si las medidas adoptadas funcionan.")
                    }
                },
                new TriviaTopic
                {
                    Id = "eg_cuidado",
                    Title = "Corresponsabilidad y cuidado",
                    Description = "El trabajo doméstico y de cuidado: quién lo hace, cuánto vale y cómo se comparte.",
                    IconKey = "icon_care",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: la economía del cuidado explicada",
                        Description = "Imagen sobre el trabajo doméstico no remunerado, su peso en la economía y cómo distribuirlo de forma corresponsable en el hogar.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/eg_cuidado_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("eg_cuidado_q01",
                            "En el contexto de equidad de género en Bolivia, ¿qué se entiende por 'economía del cuidado' o trabajo doméstico no remunerado?",
                            new[]
                            {
                                "Las labores del hogar y cuidado familiar que recaen mayoritariamente en mujeres, sin remuneración ni reconocimiento económico.",
                                "El salario que reciben formalmente las trabajadoras del hogar.",
                                "Un impuesto que pagan las familias por contratar personal doméstico.",
                                "Un bono estatal que reciben todas las madres bolivianas."
                            }, 0,
                            "La economía del cuidado visibiliza el trabajo doméstico y de cuidado que, históricamente, recae de forma desproporcionada en las mujeres sin remuneración ni valoración económica."),
                        Q("eg_cuidado_q02",
                            "¿Qué significa el concepto de 'corresponsabilidad' en las tareas del hogar y el cuidado familiar?",
                            new[]
                            {
                                "Que todos los miembros del hogar, sin importar su género, comparten de manera equitativa las tareas domésticas y de cuidado.",
                                "Que solo la madre es responsable de la crianza de los hijos e hijas.",
                                "Que el Estado asume por completo el cuidado infantil de cada familia.",
                                "Que las tareas del hogar deben delegarse siempre a terceros contratados."
                            }, 0,
                            "La corresponsabilidad implica repartir de manera equitativa las tareas del hogar y el cuidado entre todos los miembros, sin que recaigan únicamente en las mujeres."),
                        Q("eg_cuidado_q03",
                            "¿Qué reconoce la normativa laboral boliviana vigente respecto a la licencia de paternidad?",
                            new[]
                            {
                                "Otorga a los padres trabajadores un permiso remunerado tras el nacimiento de su hijo o hija.",
                                "No reconoce ningún tipo de licencia para los padres trabajadores.",
                                "Obliga a los padres a tomar un año sabático sin goce de haberes.",
                                "Solo aplica para funcionarios públicos de alto rango."
                            }, 0,
                            "La normativa laboral boliviana reconoce a los padres trabajadores el derecho a una licencia de paternidad remunerada, promoviendo la corresponsabilidad en el cuidado familiar."),
                        Q("eg_cuidado_q04",
                            "¿Por qué se dice que el trabajo de cuidado no remunerado sostiene la economía de un país?",
                            new[]
                            {
                                "Porque hace posible que el resto de las personas puedan estudiar y trabajar, aunque no se contabilice en el PIB.",
                                "Porque genera ingresos fiscales directos para el Estado.",
                                "Porque reemplaza a la actividad industrial del país.",
                                "Porque es una actividad exclusivamente recreativa."
                            }, 0,
                            "Varios estudios estiman que, valorizado, el trabajo doméstico no remunerado equivaldría a una porción muy significativa del PIB; sin él, ninguna economía funcionaría."),
                        Q("eg_cuidado_q05",
                            "La llamada 'doble jornada' que enfrentan muchas mujeres se refiere a...",
                            new[]
                            {
                                "Cumplir con un empleo remunerado y, además, con la mayor parte del trabajo doméstico y de cuidado en casa.",
                                "Trabajar dos turnos consecutivos en la misma empresa.",
                                "Estudiar y trabajar simultáneamente.",
                                "Tener dos contratos laborales formales a la vez."
                            }, 0,
                            "La doble jornada explica por qué muchas mujeres disponen de menos tiempo para descanso, formación o participación social que sus pares hombres."),
                        Q("eg_cuidado_q06",
                            "¿Qué es una encuesta de uso del tiempo y para qué sirve?",
                            new[]
                            {
                                "Un instrumento estadístico que mide cuántas horas dedican hombres y mujeres a trabajo remunerado, doméstico y de cuidado.",
                                "Un registro de las horas extras pagadas en las empresas.",
                                "Una encuesta sobre preferencias de entretenimiento.",
                                "Un control de asistencia laboral."
                            }, 0,
                            "Estas encuestas hacen visible la desigual distribución del tiempo entre géneros y son la base para diseñar políticas de cuidado."),
                        Q("eg_cuidado_q07",
                            "Los centros infantiles y guarderías comunitarias contribuyen a la equidad de género porque...",
                            new[]
                            {
                                "Liberan tiempo de cuidado, permitiendo que madres y padres estudien, trabajen o emprendan.",
                                "Eliminan la responsabilidad parental sobre los hijos.",
                                "Son un servicio dirigido exclusivamente a familias de altos ingresos.",
                                "Reemplazan la educación escolar obligatoria."
                            }, 0,
                            "La infraestructura de cuidado es una condición material para que las mujeres puedan incorporarse y permanecer en el mercado laboral en igualdad de condiciones."),
                        Q("eg_cuidado_q08",
                            "El cuidado de personas adultas mayores o con discapacidad en el hogar suele recaer en...",
                            new[]
                            {
                                "Mujeres de la familia, quienes muchas veces reducen o abandonan su trabajo remunerado para asumirlo.",
                                "Instituciones públicas en la totalidad de los casos.",
                                "Empresas privadas contratadas por el Estado.",
                                "Los hombres de la familia en la mayoría de los hogares."
                            }, 0,
                            "El envejecimiento poblacional aumenta la demanda de cuidados, que hoy se resuelve principalmente con trabajo femenino no remunerado dentro de las familias."),
                        Q("eg_cuidado_q09",
                            "¿Qué es un 'sistema integral de cuidados' como política pública?",
                            new[]
                            {
                                "Un conjunto articulado de servicios, licencias y regulaciones para redistribuir el cuidado entre Estado, familias, empresas y comunidad.",
                                "Un seguro privado de salud para adultos mayores.",
                                "Un programa de becas escolares.",
                                "Un subsidio único entregado por nacimiento."
                            }, 0,
                            "La idea central es dejar de tratar el cuidado como un asunto privado y familiar para convertirlo en una responsabilidad social compartida."),
                        Q("eg_cuidado_q10",
                            "Repartir tareas domésticas con niños y niñas desde pequeños, sin distinción de género, ayuda a...",
                            new[]
                            {
                                "Formar adultos corresponsables y romper la transmisión de estereotipos sobre 'tareas de mujeres' y 'tareas de hombres'.",
                                "Reducir su rendimiento escolar de forma permanente.",
                                "Limitar su autonomía personal.",
                                "Retrasar su desarrollo emocional."
                            }, 0,
                            "La distribución de tareas en la infancia es una de las formas más efectivas de romper el ciclo de desigualdad en las siguientes generaciones."),
                        Q("eg_cuidado_q11",
                            "La 'carga mental' del hogar se refiere a...",
                            new[]
                            {
                                "La tarea invisible de planificar, recordar y organizar todo lo que el hogar necesita, que suele recaer en las mujeres.",
                                "El esfuerzo físico de las tareas de limpieza.",
                                "El estrés exclusivamente laboral.",
                                "El tiempo dedicado al entretenimiento familiar."
                            }, 0,
                            "Además de ejecutar tareas, alguien debe pensarlas y coordinarlas; esa gestión permanente es una carga real que rara vez se distribuye."),
                        Q("eg_cuidado_q12",
                            "¿Qué efecto tiene sobre la jubilación de las mujeres el haber dedicado años al cuidado no remunerado?",
                            new[]
                            {
                                "Acumulan menos aportes a la seguridad social, lo que se traduce en pensiones más bajas o inexistentes.",
                                "Reciben una pensión mayor como compensación automática.",
                                "No tiene ninguna incidencia en su jubilación.",
                                "Se les reconoce el doble de aportes por cada año dedicado al hogar."
                            }, 0,
                            "Las interrupciones laborales por cuidado se traducen en menor densidad de aportes y, décadas después, en una brecha de pensiones entre hombres y mujeres."),
                        Q("eg_cuidado_q13",
                            "El teletrabajo y los horarios flexibles pueden favorecer la equidad si...",
                            new[]
                            {
                                "Se aplican a todos los géneros y se acompañan de corresponsabilidad real, evitando que la mujer sume trabajo y cuidado al mismo tiempo.",
                                "Se otorgan únicamente a las mujeres con hijos pequeños.",
                                "Se usan para exigir disponibilidad las 24 horas.",
                                "Sustituyen por completo las licencias de maternidad y paternidad."
                            }, 0,
                            "Si la flexibilidad se ofrece solo a mujeres, refuerza el estereotipo de que el cuidado es asunto femenino y puede penalizar sus carreras."),
                        Q("eg_cuidado_q14",
                            "Que un padre use efectivamente su licencia de paternidad es importante porque...",
                            new[]
                            {
                                "Fortalece el vínculo con su hijo o hija y normaliza que el cuidado también es responsabilidad masculina.",
                                "Reduce los derechos de la madre trabajadora.",
                                "Es un requisito para cobrar el aguinaldo.",
                                "Sustituye la licencia de maternidad de la madre."
                            }, 0,
                            "El uso real de las licencias por parte de los padres es uno de los indicadores más claros de avance hacia la corresponsabilidad en el cuidado."),
                        Q("eg_cuidado_q15",
                            "En hogares monoparentales encabezados por mujeres, un desafío frecuente es...",
                            new[]
                            {
                                "Combinar la generación de ingresos con el cuidado sin redes de apoyo suficientes, lo que aumenta el riesgo de pobreza.",
                                "Contar con exceso de tiempo libre disponible.",
                                "Tener ingresos superiores al promedio nacional.",
                                "Estar exentas de responsabilidades de cuidado."
                            }, 0,
                            "La jefatura femenina de hogar, cada vez más frecuente en Bolivia, exige políticas de cuidado accesibles para evitar que la falta de apoyo se traduzca en pobreza.")
                    }
                },
                new TriviaTopic
                {
                    Id = "eg_liderazgo",
                    Title = "Liderazgo y participación",
                    Description = "Paridad, alternancia y techo de cristal: mujeres en los espacios de decisión.",
                    IconKey = "icon_leadership",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: paridad y alternancia en Bolivia",
                        Description = "Imagen que resume cómo funcionan la paridad y la alternancia en listas electorales y cuál es la participación femenina en espacios de decisión.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/eg_liderazgo_paridad.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("eg_liderazgo_q01",
                            "¿Qué describe el término 'techo de cristal' aplicado al ámbito laboral boliviano?",
                            new[]
                            {
                                "Barreras invisibles que dificultan que las mujeres asciendan a cargos gerenciales o directivos.",
                                "Un límite legal que impide contratar mujeres en ciertas empresas.",
                                "Una política de cuotas obligatoria vigente en todas las empresas privadas.",
                                "El techo físico de las oficinas bancarias en edificios antiguos."
                            }, 0,
                            "El 'techo de cristal' describe barreras informales y culturales que limitan el ascenso de las mujeres a puestos de alta dirección, aun con igual o mayor preparación que sus pares hombres."),
                        Q("eg_liderazgo_q02",
                            "En las organizaciones sociales y sindicales campesino-indígenas de Bolivia, ¿qué busca el principio de alternancia de género en cargos de dirigencia?",
                            new[]
                            {
                                "Garantizar que mujeres y hombres ocupen de forma equitativa y rotativa los cargos de decisión.",
                                "Que solo los hombres puedan ejercer como dirigentes titulares.",
                                "Que las mujeres únicamente participen en cargos administrativos menores.",
                                "Eliminar la participación femenina en las asambleas comunitarias."
                            }, 0,
                            "La alternancia de género busca que la representación y la toma de decisiones en las organizaciones sociales se distribuya de forma equitativa entre mujeres y hombres."),
                        Q("eg_liderazgo_q03",
                            "Bolivia es reconocida mundialmente por su alta proporción de mujeres en la Asamblea Legislativa Plurinacional. ¿A qué principio constitucional se debe principalmente?",
                            new[]
                            {
                                "Al principio de paridad y alternancia de género aplicado a las listas de candidaturas.",
                                "A una cuota informal sin respaldo legal.",
                                "A la decisión voluntaria de cada partido político, sin obligación alguna.",
                                "A un sorteo aleatorio de candidatos sin criterio de género."
                            }, 0,
                            "El principio de paridad y alternancia obliga a conformar listas equitativas e intercaladas, lo que llevó a Bolivia a tener una de las asambleas con mayor proporción de mujeres del mundo."),
                        Q("eg_liderazgo_q04",
                            "¿Qué significa exactamente 'paridad' en las listas de candidaturas?",
                            new[]
                            {
                                "Que las listas deben conformarse con 50% de mujeres y 50% de hombres.",
                                "Que las mujeres solo pueden postularse como suplentes.",
                                "Que se elige por sorteo el género de cada candidatura.",
                                "Que al menos un 10% de las candidaturas sean mujeres."
                            }, 0,
                            "La paridad establece una composición equitativa; la alternancia complementa exigiendo que los nombres se intercalen para que las mujeres no queden solo al final de la lista."),
                        Q("eg_liderazgo_q05",
                            "El 'suelo pegajoso', complemento del techo de cristal, describe...",
                            new[]
                            {
                                "La dificultad de muchas mujeres para salir de empleos de baja calificación y baja remuneración.",
                                "Un beneficio adicional para trabajadoras con antigüedad.",
                                "El acceso preferente de las mujeres a cargos directivos.",
                                "Un tipo de contrato laboral temporal."
                            }, 0,
                            "Mientras el techo de cristal frena el ascenso a la cúpula, el suelo pegajoso mantiene a muchas mujeres atrapadas en la base de la estructura ocupacional."),
                        Q("eg_liderazgo_q06",
                            "¿Por qué se promueve la presencia de mujeres en directorios y comités ejecutivos?",
                            new[]
                            {
                                "Porque la diversidad en la toma de decisiones mejora la calidad de los análisis y refleja mejor a la sociedad y a la clientela.",
                                "Porque la ley obliga a que los directorios sean exclusivamente femeninos.",
                                "Porque reduce automáticamente los costos operativos.",
                                "Porque las decisiones se toman más rápido con menos debate."
                            }, 0,
                            "La evidencia muestra que equipos diversos consideran más perspectivas y riesgos, además de representar mejor a la base de clientes de una organización."),
                        Q("eg_liderazgo_q07",
                            "Un programa de mentoría para mujeres profesionales busca principalmente...",
                            new[]
                            {
                                "Acompañar su desarrollo, ampliar sus redes de contacto y prepararlas para asumir cargos de mayor responsabilidad.",
                                "Reemplazar la formación técnica formal.",
                                "Limitar su acceso a cargos operativos.",
                                "Sustituir los procesos de evaluación de desempeño."
                            }, 0,
                            "Las redes profesionales y el patrocinio interno son factores decisivos en las promociones, y suelen estar menos disponibles para las mujeres."),
                        Q("eg_liderazgo_q08",
                            "La violencia política hacia las mujeres afecta el liderazgo porque...",
                            new[]
                            {
                                "Busca impedir o limitar el ejercicio de sus funciones mediante presión, acoso o amenazas, forzando renuncias.",
                                "Solo ocurre en el ámbito privado del hogar.",
                                "Es una práctica sin consecuencias legales en Bolivia.",
                                "Afecta únicamente a candidatos hombres."
                            }, 0,
                            "Bolivia fue pionera en legislar contra el acoso y la violencia política hacia las mujeres, precisamente porque la paridad formal no basta si no se garantiza el ejercicio efectivo del cargo."),
                        Q("eg_liderazgo_q09",
                            "En las comunidades andinas, el principio del 'chacha-warmi' hace referencia a...",
                            new[]
                            {
                                "La complementariedad entre hombre y mujer en el ejercicio de la autoridad comunitaria.",
                                "Un cargo exclusivamente masculino dentro de la comunidad.",
                                "Una festividad agrícola sin relación con la organización social.",
                                "La prohibición de que las mujeres participen en asambleas."
                            }, 0,
                            "El chacha-warmi expresa la autoridad ejercida en pareja; el desafío contemporáneo es que se traduzca en participación real de la mujer y no solo en presencia formal."),
                        Q("eg_liderazgo_q10",
                            "¿Qué es el 'sesgo inconsciente' en procesos de promoción laboral?",
                            new[]
                            {
                                "Preferencias automáticas basadas en estereotipos que influyen en las decisiones sin que la persona lo advierta.",
                                "Una política explícita de discriminación escrita en el reglamento.",
                                "Un error de cálculo en la planilla de sueldos.",
                                "Una evaluación basada exclusivamente en resultados medibles."
                            }, 0,
                            "Nombrar y medir los sesgos, junto con criterios de evaluación estructurados, reduce su impacto en decisiones de contratación y ascenso."),
                        Q("eg_liderazgo_q11",
                            "¿Qué aporta contar con referentes femeninas visibles en una organización o comunidad?",
                            new[]
                            {
                                "Muestra a otras mujeres y niñas que esos espacios son posibles, ampliando sus expectativas y aspiraciones.",
                                "Desincentiva la participación de nuevas generaciones.",
                                "No tiene ningún efecto sobre las trayectorias profesionales.",
                                "Solo beneficia a quien ocupa el cargo."
                            }, 0,
                            "La representación importa: es difícil aspirar a un rol que nunca se ha visto ocupado por alguien con una trayectoria similar a la propia."),
                        Q("eg_liderazgo_q12",
                            "Las mujeres en cargos directivos del sistema financiero boliviano contribuyen a...",
                            new[]
                            {
                                "Diseñar productos y servicios que consideran mejor las necesidades de las clientas y de las emprendedoras.",
                                "Reducir la cartera de clientes de la entidad.",
                                "Eliminar los procesos de evaluación de riesgo.",
                                "Restringir el acceso al crédito de los hombres."
                            }, 0,
                            "La diversidad en la toma de decisiones se traduce en una mejor comprensión de un segmento de clientas históricamente subatendido."),
                        Q("eg_liderazgo_q13",
                            "¿Qué es una 'acción afirmativa' en materia de equidad de género?",
                            new[]
                            {
                                "Una medida temporal orientada a corregir una desigualdad histórica, como cuotas o programas de formación dirigidos.",
                                "Una sanción penal contra empresas.",
                                "Una medida permanente que otorga privilegios sin plazo ni evaluación.",
                                "Una campaña publicitaria interna."
                            }, 0,
                            "Las acciones afirmativas buscan nivelar el punto de partida y se justifican mientras persista la brecha que pretenden corregir."),
                        Q("eg_liderazgo_q14",
                            "La participación de mujeres en organizaciones vecinales y juntas de barrio en Bolivia es relevante porque...",
                            new[]
                            {
                                "Incorpora a la agenda local necesidades cotidianas como agua, seguridad, salud y cuidado.",
                                "Duplica los costos administrativos de la organización.",
                                "Sustituye la función del gobierno municipal.",
                                "Solo tiene un valor simbólico sin efectos prácticos."
                            }, 0,
                            "Quienes viven de cerca la gestión cotidiana del hogar y el barrio suelen identificar prioridades que de otro modo no llegan a la agenda pública."),
                        Q("eg_liderazgo_q15",
                            "¿Qué indicador permite evaluar mejor el avance real de la equidad en una organización?",
                            new[]
                            {
                                "La proporción de mujeres en cada nivel jerárquico y su evolución en el tiempo, no solo el total de empleadas.",
                                "El número total de personas contratadas al año.",
                                "La cantidad de eventos internos realizados.",
                                "El presupuesto destinado a publicidad."
                            }, 0,
                            "Una organización puede tener mayoría de mujeres en la base y casi ninguna en la dirección; solo el desglose por nivel revela dónde se rompe la trayectoria.")
                    }
                },
                new TriviaTopic
                {
                    Id = "eg_educacion",
                    Title = "Educación y brecha digital",
                    Description = "Acceso a STEM, permanencia escolar y habilidades digitales para cerrar brechas.",
                    IconKey = "icon_education",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: mujeres, educación y tecnología",
                        Description = "Imagen sobre participación femenina en carreras STEM, permanencia escolar de las adolescentes y acceso a internet y dispositivos en zonas rurales.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/eg_educacion_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("eg_educacion_q01",
                            "En Bolivia, ¿qué representa la brecha de género en carreras STEM (ciencia, tecnología, ingeniería y matemáticas)?",
                            new[]
                            {
                                "La menor proporción de mujeres que estudian y ejercen estas carreras en comparación con los hombres.",
                                "La diferencia de horarios de clases asignados a hombres y mujeres.",
                                "La cantidad de universidades públicas frente a las privadas.",
                                "El número de becas que reciben exclusivamente los hombres por ley."
                            }, 0,
                            "La brecha en STEM refleja la menor participación histórica de mujeres en estas áreas, pese a que su desempeño es igual de competente que el de los hombres."),
                        Q("eg_educacion_q02",
                            "¿Qué describe la 'brecha digital de género' en el contexto boliviano?",
                            new[]
                            {
                                "La menor proporción de mujeres, especialmente rurales, con acceso y habilidades para usar internet y tecnología digital.",
                                "La diferencia de precios de internet entre hombres y mujeres.",
                                "Una ley que prohíbe a las mujeres comprar equipos celulares.",
                                "El número de antenas de telefonía instaladas por departamento."
                            }, 0,
                            "La brecha digital de género limita oportunidades educativas, laborales y de acceso a servicios financieros digitales, sobre todo en el área rural."),
                        Q("eg_educacion_q03",
                            "¿Cuál es una de las principales causas del abandono escolar de adolescentes mujeres en el área rural boliviana?",
                            new[]
                            {
                                "El embarazo adolescente y la asignación de tareas domésticas y de cuidado en el hogar.",
                                "La prohibición legal de que las mujeres estudien secundaria.",
                                "El exceso de becas disponibles para mujeres.",
                                "La ausencia total de escuelas en todo el territorio rural."
                            }, 0,
                            "El embarazo temprano, la distancia a los centros educativos y la carga de trabajo doméstico son factores que interrumpen la trayectoria escolar de las adolescentes."),
                        Q("eg_educacion_q04",
                            "¿Qué derecho tiene una estudiante embarazada en el sistema educativo boliviano?",
                            new[]
                            {
                                "Continuar sus estudios sin discriminación, con medidas que faciliten su permanencia en la escuela.",
                                "Ser expulsada del establecimiento educativo.",
                                "Cambiarse obligatoriamente a educación nocturna.",
                                "Suspender definitivamente su formación escolar."
                            }, 0,
                            "La normativa prohíbe la discriminación por embarazo en el ámbito educativo; garantizar la permanencia es clave para romper el ciclo de pobreza."),
                        Q("eg_educacion_q05",
                            "La alfabetización digital de mujeres adultas y adultas mayores es importante porque...",
                            new[]
                            {
                                "Les permite acceder a banca digital, trámites, información y oportunidades económicas de forma autónoma.",
                                "Es un requisito legal para cobrar la renta dignidad.",
                                "Sustituye la necesidad de educación formal.",
                                "Solo tiene fines recreativos."
                            }, 0,
                            "A medida que los servicios se digitalizan, quien no maneja herramientas básicas queda excluida de trámites, pagos y beneficios."),
                        Q("eg_educacion_q06",
                            "¿Qué efecto tienen los estereotipos escolares del tipo 'las matemáticas no son para niñas'?",
                            new[]
                            {
                                "Desalientan tempranamente el interés y la confianza de las niñas en áreas científicas, reduciendo su participación futura.",
                                "Mejoran el rendimiento académico de las estudiantes.",
                                "No influyen en las decisiones vocacionales.",
                                "Solo afectan a estudiantes universitarios."
                            }, 0,
                            "Las diferencias de rendimiento en matemáticas no responden a capacidad sino a expectativas y estímulos distintos desde la infancia."),
                        Q("eg_educacion_q07",
                            "¿Por qué importa que existan profesoras y profesionales mujeres en áreas técnicas?",
                            new[]
                            {
                                "Funcionan como referentes que amplían las expectativas de las estudiantes sobre su propio futuro profesional.",
                                "Reducen la exigencia académica de las materias.",
                                "Impiden que los hombres estudien esas carreras.",
                                "No influyen en las decisiones de las estudiantes."
                            }, 0,
                            "La presencia de referentes cercanas es uno de los factores más asociados a la elección de carreras técnicas por parte de las mujeres jóvenes."),
                        Q("eg_educacion_q08",
                            "La educación financiera dirigida a mujeres es especialmente relevante porque...",
                            new[]
                            {
                                "Fortalece su autonomía económica y su capacidad de decidir sobre ingresos, ahorro y crédito.",
                                "Es un requisito para abrir una cuenta bancaria.",
                                "Reemplaza la necesidad de contar con ingresos propios.",
                                "Solo aplica a mujeres con estudios universitarios."
                            }, 0,
                            "La autonomía económica es un factor protector frente a la violencia y una condición para tomar decisiones libres sobre el propio proyecto de vida."),
                        Q("eg_educacion_q09",
                            "¿Qué es la violencia digital o ciberacoso hacia mujeres y adolescentes?",
                            new[]
                            {
                                "Agresiones ejercidas por medios digitales, como difusión de imágenes íntimas sin consentimiento, hostigamiento o amenazas.",
                                "El uso excesivo de redes sociales durante el día.",
                                "Una falla técnica de las plataformas digitales.",
                                "El costo elevado de los planes de datos móviles."
                            }, 0,
                            "La violencia digital tiene efectos reales sobre la salud mental y la participación en línea; requiere prevención, denuncia y acompañamiento."),
                        Q("eg_educacion_q10",
                            "La formación técnica y profesional para mujeres en oficios tradicionalmente masculinos busca...",
                            new[]
                            {
                                "Abrir el acceso a empleos mejor remunerados y romper la segregación ocupacional.",
                                "Reemplazar la educación secundaria obligatoria.",
                                "Limitar sus oportunidades a un solo sector.",
                                "Reducir sus expectativas salariales."
                            }, 0,
                            "Los oficios técnicos suelen pagar mejor que las ocupaciones feminizadas; ampliar el acceso reduce la brecha de ingresos."),
                        Q("eg_educacion_q11",
                            "¿Qué es la 'deserción digital' de las estudiantes durante periodos de educación a distancia?",
                            new[]
                            {
                                "El abandono de clases virtuales por falta de dispositivos, conectividad o por asumir tareas de cuidado en casa.",
                                "El cambio de una plataforma educativa a otra.",
                                "La preferencia por clases presenciales.",
                                "La renuncia voluntaria a usar redes sociales."
                            }, 0,
                            "Durante la educación remota, la falta de equipos compartidos en el hogar y la carga doméstica afectaron de forma desproporcionada a las estudiantes mujeres."),
                        Q("eg_educacion_q12",
                            "La educación integral en sexualidad en las escuelas contribuye a...",
                            new[]
                            {
                                "Prevenir el embarazo adolescente y la violencia sexual mediante información oportuna y basada en evidencia.",
                                "Aumentar el abandono escolar.",
                                "Sustituir la formación en ciencias naturales.",
                                "Retrasar el ingreso a la educación primaria."
                            }, 0,
                            "La evidencia internacional muestra que la información adecuada y oportuna reduce riesgos y fortalece la capacidad de las y los adolescentes de protegerse."),
                        Q("eg_educacion_q13",
                            "¿Qué barrera enfrentan muchas mujeres rurales para acceder a formación continua?",
                            new[]
                            {
                                "La distancia, la falta de conectividad y la carga de trabajo doméstico y productivo simultáneo.",
                                "La prohibición legal de capacitarse.",
                                "El exceso de oferta formativa en sus comunidades.",
                                "La obligación de estudiar en el exterior."
                            }, 0,
                            "Diseñar capacitaciones cercanas, con horarios compatibles y apoyo para el cuidado, aumenta significativamente la participación de mujeres rurales."),
                        Q("eg_educacion_q14",
                            "¿Por qué el lenguaje inclusivo en materiales educativos tiene efecto sobre la equidad?",
                            new[]
                            {
                                "Porque nombrar explícitamente a mujeres y niñas contribuye a que se reconozcan como parte de esos espacios y roles.",
                                "Porque incrementa la extensión de los textos escolares.",
                                "Porque reemplaza el contenido técnico de las materias.",
                                "Porque es una exigencia de organismos internacionales sin efecto práctico."
                            }, 0,
                            "Lo que no se nombra tiende a no representarse: la forma en que se describen profesiones y roles en los materiales influye en las aspiraciones infantiles."),
                        Q("eg_educacion_q15",
                            "Un programa de becas dirigido a mujeres en carreras tecnológicas es un ejemplo de...",
                            new[]
                            {
                                "Acción afirmativa para corregir una brecha histórica de participación en esas áreas.",
                                "Discriminación contra los estudiantes hombres sin justificación alguna.",
                                "Una medida sin relación con la equidad de género.",
                                "Un beneficio permanente e incondicional para todas las mujeres."
                            }, 0,
                            "Estas becas buscan equilibrar un punto de partida desigual y se evalúan según su impacto en la matrícula y la titulación femenina en esas carreras.")
                    }
                }
            }
        };
    }
}
