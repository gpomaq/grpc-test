using static GrpcTest.Services.TriviaCatalog;

namespace GrpcTest.Services
{
    public static partial class QuestionBank
    {
        private static TriviaCategory BuildDerechosMujer() => new TriviaCategory
        {
            Code = "derechos_mujer",
            Label = "Derechos de la Mujer",
            IconKey = "icon_women",
            Description = "Leyes, servicios y derechos que protegen la vida, la salud y el patrimonio de las mujeres en Bolivia.",
            Topics = new List<TriviaTopic>
            {
                new TriviaTopic
                {
                    Id = "dm_marco_legal",
                    Title = "Marco legal",
                    Description = "Las principales leyes bolivianas que reconocen y protegen los derechos de las mujeres.",
                    IconKey = "icon_law",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: leyes que protegen a las mujeres en Bolivia",
                        Description = "Imagen que resume la Ley N° 348, la Ley N° 243, la Ley N° 2450 y otras normas clave, con ejemplos de aplicación práctica.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/dm_marco_legal_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("dm_marco_legal_q01",
                            "En Bolivia, ¿qué ley nacional garantiza a las mujeres una vida libre de violencia y tipifica el delito de feminicidio?",
                            new[]
                            {
                                "La Ley General del Trabajo.",
                                "La Ley N° 348 (Ley Integral para Garantizar a las Mujeres una Vida Libre de Violencia).",
                                "El Código de las Familias.",
                                "La Ley N° 004 de Lucha contra la Corrupción."
                            }, 1,
                            "La Ley N° 348 es la norma integral específica que protege a las mujeres contra todo tipo de violencia física, psicológica, sexual o económica en Bolivia."),
                        Q("dm_marco_legal_q02",
                            "¿Qué busca proteger la Ley N° 243 en Bolivia?",
                            new[]
                            {
                                "A las mujeres candidatas o electas de acoso y violencia política.",
                                "Los derechos de propiedad intelectual de las empresas.",
                                "El acceso universal y gratuito a la salud pública.",
                                "La jornada laboral máxima de 8 horas diarias."
                            }, 0,
                            "La Ley N° 243 (Ley Contra el Acoso y Violencia Política hacia las Mujeres) protege a candidatas, electas o autoridades en ejercicio frente a agresiones destinadas a impedir su participación política."),
                        Q("dm_marco_legal_q03",
                            "¿Qué protección brinda el Código de las Familias boliviano en relación con el matrimonio de niñas y adolescentes?",
                            new[]
                            {
                                "Establece una edad mínima legal para contraer matrimonio, protegiendo a las menores de uniones forzadas.",
                                "Permite el matrimonio a cualquier edad si los padres lo autorizan.",
                                "Obliga a las mujeres a casarse antes de cumplir 18 años.",
                                "No regula ninguna edad mínima para contraer matrimonio."
                            }, 0,
                            "El Código de las Familias fija una edad mínima legal para el matrimonio como medida de protección frente a uniones forzadas o tempranas de niñas y adolescentes."),
                        Q("dm_marco_legal_q04",
                            "¿Qué tipos de violencia reconoce la Ley N° 348 además de la violencia física?",
                            new[]
                            {
                                "Psicológica, sexual, económica, patrimonial, laboral, mediática y otras formas contempladas en la norma.",
                                "Únicamente la violencia física con lesiones visibles.",
                                "Solamente la violencia ocurrida en el ámbito laboral.",
                                "Solo la violencia cometida por desconocidos."
                            }, 0,
                            "La Ley N° 348 reconoce más de una decena de tipos de violencia, lo que permite denunciar situaciones que no dejan marcas físicas pero causan daño real."),
                        Q("dm_marco_legal_q05",
                            "¿Qué es el feminicidio según la legislación boliviana?",
                            new[]
                            {
                                "El delito de dar muerte a una mujer por razones de género, con sanción penal específica y agravada.",
                                "Cualquier homicidio ocurrido dentro de un domicilio.",
                                "Una falta administrativa sancionada con multa.",
                                "Un delito que solo aplica en el ámbito laboral."
                            }, 0,
                            "Tipificar el feminicidio de forma autónoma permite visibilizar estadísticamente estos crímenes y aplicar la sanción más alta prevista en el Código Penal."),
                        Q("dm_marco_legal_q06",
                            "La Ley N° 2450 regula en Bolivia...",
                            new[]
                            {
                                "El trabajo asalariado del hogar, reconociendo derechos laborales a las trabajadoras del hogar.",
                                "El régimen tributario de las pequeñas empresas.",
                                "El funcionamiento de las cooperativas mineras.",
                                "El sistema de transporte público urbano."
                            }, 0,
                            "La Ley N° 2450 reconoció derechos laborales (salario, descanso, aguinaldo, vacaciones) a un sector históricamente invisibilizado y mayoritariamente femenino."),
                        Q("dm_marco_legal_q07",
                            "¿Qué establece la Constitución Política del Estado boliviano respecto a la igualdad entre mujeres y hombres?",
                            new[]
                            {
                                "Reconoce la igualdad de derechos y prohíbe toda forma de discriminación por razón de sexo o género.",
                                "Establece derechos diferenciados según el género.",
                                "Delega la definición de igualdad a cada municipio.",
                                "No hace referencia alguna a la igualdad de género."
                            }, 0,
                            "La Constitución de 2009 incorporó de forma expresa la igualdad de género y la prohibición de discriminación, base de todas las leyes posteriores en la materia."),
                        Q("dm_marco_legal_q08",
                            "¿Qué es una medida de protección dictada a favor de una mujer en situación de violencia?",
                            new[]
                            {
                                "Una orden que dispone, por ejemplo, la salida del agresor del domicilio o la prohibición de acercarse a la víctima.",
                                "Una multa económica que paga la víctima.",
                                "Un permiso para que el agresor permanezca en el domicilio.",
                                "Un trámite exclusivamente voluntario sin efecto legal."
                            }, 0,
                            "Las medidas de protección son inmediatas y buscan cortar el riesgo mientras avanza el proceso; su incumplimiento tiene consecuencias legales."),
                        Q("dm_marco_legal_q09",
                            "En el ámbito laboral, la Ley N° 348 también contempla...",
                            new[]
                            {
                                "La violencia laboral, que incluye discriminación, acoso y condiciones que afectan la dignidad de la trabajadora.",
                                "Únicamente conflictos salariales colectivos.",
                                "Solo situaciones ocurridas fuera del horario de trabajo.",
                                "Exclusivamente casos del sector público."
                            }, 0,
                            "Reconocer la violencia laboral permite denunciar prácticas como el hostigamiento, la exclusión sistemática o el trato humillante en el trabajo."),
                        Q("dm_marco_legal_q10",
                            "¿Qué es la violencia mediática según la normativa boliviana?",
                            new[]
                            {
                                "La difusión de mensajes o imágenes que estereotipan, humillan o cosifican a las mujeres a través de medios de comunicación.",
                                "El uso de teléfonos móviles en horario laboral.",
                                "La publicidad de productos financieros.",
                                "La interrupción del servicio de internet."
                            }, 0,
                            "La forma en que los medios representan a las mujeres influye en la normalización de la desigualdad; por eso la norma la incluye como un tipo de violencia."),
                        Q("dm_marco_legal_q11",
                            "El derecho de las mujeres a la identidad y a llevar sus apellidos implica que...",
                            new[]
                            {
                                "Ninguna mujer está obligada a cambiar o perder sus apellidos al casarse.",
                                "Debe adoptar obligatoriamente el apellido del cónyuge.",
                                "Pierde su documento de identidad al contraer matrimonio.",
                                "Solo puede usar un apellido durante el matrimonio."
                            }, 0,
                            "La identidad es un derecho personal: el matrimonio no modifica los apellidos de una persona en la legislación boliviana."),
                        Q("dm_marco_legal_q12",
                            "¿Qué garantiza la Ley contra el Racismo y toda forma de Discriminación en relación con las mujeres indígenas?",
                            new[]
                            {
                                "Protección frente a la discriminación múltiple por género, origen étnico, idioma o vestimenta.",
                                "Un régimen tributario especial para comunidades.",
                                "La exención del deber de portar documentos de identidad.",
                                "El acceso exclusivo a cargos públicos."
                            }, 0,
                            "Las mujeres indígenas suelen enfrentar discriminación superpuesta; la norma reconoce esa dimensión múltiple del trato discriminatorio."),
                        Q("dm_marco_legal_q13",
                            "¿Qué rol cumple el Ministerio Público (Fiscalía) en un caso de violencia contra la mujer?",
                            new[]
                            {
                                "Dirige la investigación penal, solicita medidas de protección y presenta la acusación ante el juez.",
                                "Brinda alojamiento permanente a la víctima.",
                                "Emite las facturas de los servicios médicos.",
                                "Administra las casas de acogida municipales."
                            }, 0,
                            "Conocer qué hace cada instancia (FELCV, SLIM, Fiscalía, juzgados) evita que la víctima se pierda entre trámites y abandone el proceso."),
                        Q("dm_marco_legal_q14",
                            "¿Qué significa que los delitos de violencia contra la mujer sean de acción pública en Bolivia?",
                            new[]
                            {
                                "Que el Estado debe investigarlos y perseguirlos de oficio, sin depender exclusivamente de que la víctima mantenga la denuncia.",
                                "Que cualquier persona puede sancionar directamente al agresor.",
                                "Que el proceso se resuelve únicamente con una conciliación privada.",
                                "Que los casos se publican obligatoriamente en medios de comunicación."
                            }, 0,
                            "La acción pública busca evitar que la presión sobre la víctima para retirar la denuncia deje el hecho en la impunidad."),
                        Q("dm_marco_legal_q15",
                            "La normativa boliviana prohíbe la conciliación en casos de violencia porque...",
                            new[]
                            {
                                "Existe una relación desigual de poder y la conciliación puede exponer nuevamente a la víctima al agresor.",
                                "Los procesos judiciales resultan más económicos.",
                                "La conciliación está prohibida en todos los ámbitos del derecho.",
                                "Las partes nunca llegan a acuerdos en ningún caso."
                            }, 0,
                            "En contextos de violencia no hay igualdad entre las partes; por eso la norma restringe la conciliación, especialmente cuando existe reincidencia.")
                    }
                },
                new TriviaTopic
                {
                    Id = "dm_violencia",
                    Title = "Prevención de violencia",
                    Description = "Señales de alerta, rutas de denuncia y servicios de protección disponibles en Bolivia.",
                    IconKey = "icon_shield",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: ruta de denuncia paso a paso",
                        Description = "Imagen que explica a dónde acudir ante un hecho de violencia (FELCV, SLIM, Fiscalía), qué documentos llevar y qué medidas de protección se pueden solicitar.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/dm_violencia_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("dm_violencia_q01",
                            "¿Qué es la FELCV (Fuerza Especial de Lucha Contra la Violencia) en Bolivia?",
                            new[]
                            {
                                "Una unidad policial especializada en la atención y protección de víctimas de violencia intrafamiliar y de género.",
                                "Un banco estatal de créditos exclusivos para mujeres.",
                                "Una ONG internacional sin relación con el Estado boliviano.",
                                "Un impuesto destinado a la construcción de escuelas."
                            }, 0,
                            "La FELCV es una unidad especializada de la Policía Boliviana dedicada a la prevención, atención y protección de víctimas de violencia intrafamiliar y de género."),
                        Q("dm_violencia_q02",
                            "¿Qué son los SLIM (Servicios Legales Integrales Municipales)?",
                            new[]
                            {
                                "Instancias municipales que brindan atención legal, psicológica y social gratuita a víctimas de violencia.",
                                "Bancos especializados exclusivamente en créditos para mujeres.",
                                "Escuelas técnicas destinadas únicamente a mujeres.",
                                "Un impuesto municipal destinado a financiar obras públicas."
                            }, 0,
                            "Los SLIM son instancias de los gobiernos municipales que brindan orientación y atención legal, psicológica y social gratuita, principalmente a mujeres víctimas de violencia."),
                        Q("dm_violencia_q03",
                            "¿Cuál es la función de las 'casas de acogida' para mujeres en situación de violencia?",
                            new[]
                            {
                                "Brindar refugio temporal, protección y contención a mujeres y sus hijos e hijas mientras se resuelve su situación de riesgo.",
                                "Ser un centro de detención para las víctimas de violencia.",
                                "Funcionar como oficinas exclusivas de trámites de divorcio.",
                                "Cobrar una tarifa mensual por el servicio de protección brindado."
                            }, 0,
                            "Las casas de acogida ofrecen refugio temporal, seguridad y apoyo integral a mujeres víctimas de violencia y a sus hijos e hijas mientras se gestiona su protección."),
                        Q("dm_violencia_q04",
                            "¿Qué se entiende por acoso sexual callejero, frecuente en el transporte público y las calles?",
                            new[]
                            {
                                "Conductas de connotación sexual no deseadas, como comentarios, gestos o tocamientos, ejercidas en espacios públicos sin consentimiento.",
                                "Cualquier saludo cordial entre desconocidos en la calle.",
                                "Una infracción de tránsito relacionada con el exceso de velocidad.",
                                "Un delito que solo puede ocurrir dentro de una vivienda particular."
                            }, 0,
                            "El acoso callejero abarca conductas de connotación sexual no deseadas en espacios públicos, afectando la libertad y seguridad de las mujeres en su vida cotidiana."),
                        Q("dm_violencia_q05",
                            "¿Cuál es una señal de alerta temprana de violencia en una relación de pareja?",
                            new[]
                            {
                                "El control sobre con quién habla, cómo se viste, en qué gasta su dinero o revisar su teléfono sin permiso.",
                                "Que la pareja respete sus decisiones personales.",
                                "Que ambos mantengan amistades propias.",
                                "Que compartan las decisiones económicas del hogar."
                            }, 0,
                            "El control y el aislamiento suelen anteceder a la violencia física; reconocerlos a tiempo permite buscar ayuda antes de que la situación escale."),
                        Q("dm_violencia_q06",
                            "¿Qué es la violencia económica o patrimonial dentro de una relación?",
                            new[]
                            {
                                "Controlar o retener el dinero, impedir trabajar, o destruir o apropiarse de bienes de la mujer para someterla.",
                                "Compartir los gastos del hogar de forma equitativa.",
                                "Ahorrar en conjunto para una meta familiar.",
                                "Contratar un seguro de vida familiar."
                            }, 0,
                            "La dependencia económica forzada es uno de los principales motivos por los que muchas mujeres no logran salir de una relación violenta."),
                        Q("dm_violencia_q07",
                            "¿Qué debe hacer una persona que presencia o sospecha un caso de violencia contra una mujer?",
                            new[]
                            {
                                "Denunciar ante la FELCV, el SLIM o la Fiscalía; la denuncia puede ser presentada por terceros.",
                                "Guardar silencio porque es un asunto exclusivamente privado.",
                                "Enfrentar directamente al agresor por cuenta propia.",
                                "Esperar a que la situación se resuelva sola."
                            }, 0,
                            "La violencia contra la mujer no es un asunto privado: cualquier persona puede denunciar y activar los servicios de protección."),
                        Q("dm_violencia_q08",
                            "Al acudir a denunciar, ¿qué es útil llevar si se cuenta con ello?",
                            new[]
                            {
                                "Documento de identidad, certificados médicos, fotografías, mensajes o cualquier otro elemento que respalde el hecho.",
                                "Únicamente el testimonio de un familiar directo.",
                                "Un monto de dinero para cubrir la denuncia.",
                                "Una autorización escrita del agresor."
                            }, 0,
                            "La denuncia es gratuita y no requiere abogado para iniciarse; los elementos de prueba fortalecen la investigación, pero su ausencia no impide denunciar."),
                        Q("dm_violencia_q09",
                            "¿Qué es el 'ciclo de la violencia' en una relación de pareja?",
                            new[]
                            {
                                "Un patrón que alterna tensión, agresión y reconciliación, y que tiende a repetirse e intensificarse con el tiempo.",
                                "Un episodio aislado que nunca vuelve a ocurrir.",
                                "Un proceso judicial de tres etapas.",
                                "Una terapia de pareja estructurada."
                            }, 0,
                            "La fase de reconciliación o 'luna de miel' genera esperanza de cambio y explica por qué muchas víctimas retiran denuncias; conocer el ciclo ayuda a romperlo."),
                        Q("dm_violencia_q10",
                            "¿Qué es la violencia digital contra las mujeres?",
                            new[]
                            {
                                "Hostigamiento, amenazas o difusión de imágenes íntimas sin consentimiento a través de medios digitales.",
                                "El uso de aplicaciones bancarias en el celular.",
                                "La publicación de fotografías propias en redes sociales.",
                                "La suscripción a servicios de streaming."
                            }, 0,
                            "La difusión no consentida de imágenes íntimas es una forma de violencia con efectos graves; conviene preservar las pruebas y denunciar de inmediato."),
                        Q("dm_violencia_q11",
                            "Un plan de seguridad personal para una mujer en riesgo incluye principalmente...",
                            new[]
                            {
                                "Identificar un lugar seguro, tener documentos y números clave a mano, y acordar una señal de alerta con alguien de confianza.",
                                "Permanecer aislada sin informar a nadie.",
                                "Confrontar al agresor a solas.",
                                "Eliminar todo contacto con instituciones de apoyo."
                            }, 0,
                            "Anticipar una ruta de salida y contar con apoyos identificados reduce el riesgo en el momento crítico, que suele ser el de mayor peligro."),
                        Q("dm_violencia_q12",
                            "¿Qué impacto tiene la violencia intrafamiliar sobre los hijos e hijas que la presencian?",
                            new[]
                            {
                                "Genera afectación emocional y de desarrollo, y son reconocidos también como víctimas de esa violencia.",
                                "No los afecta mientras no reciban agresiones directas.",
                                "Mejora su capacidad de resolución de conflictos.",
                                "Es un asunto ajeno a su bienestar."
                            }, 0,
                            "Niñas y niños expuestos a violencia presentan efectos en su salud mental y aprendizaje, y la normativa los considera víctimas que requieren protección."),
                        Q("dm_violencia_q13",
                            "¿Por qué la atención psicológica es parte de la respuesta integral a la violencia?",
                            new[]
                            {
                                "Porque el daño emocional persiste después del hecho y el acompañamiento sostiene a la víctima durante el proceso.",
                                "Porque reemplaza la denuncia ante las autoridades.",
                                "Porque es un requisito para acceder a un crédito bancario.",
                                "Porque sustituye las medidas de protección judiciales."
                            }, 0,
                            "El acompañamiento psicológico y social reduce el abandono del proceso judicial y es parte de los servicios que brindan los SLIM."),
                        Q("dm_violencia_q14",
                            "¿Qué actitud contribuye a prevenir la violencia desde el entorno cercano?",
                            new[]
                            {
                                "No naturalizar comentarios ni 'bromas' machistas y creer y acompañar a quien relata una situación de violencia.",
                                "Recomendar a la víctima que evite denunciar para no generar problemas.",
                                "Considerar el tema como un asunto exclusivo de la pareja.",
                                "Cuestionar la conducta o vestimenta de la víctima."
                            }, 0,
                            "Las respuestas del entorno son decisivas: cuestionar a la víctima incrementa el aislamiento, mientras que creerle facilita la búsqueda de ayuda."),
                        Q("dm_violencia_q15",
                            "La prevención de la violencia en el ámbito educativo y laboral se logra principalmente con...",
                            new[]
                            {
                                "Protocolos claros, formación permanente y canales de denuncia seguros y confidenciales.",
                                "Charlas anuales sin ningún mecanismo de seguimiento.",
                                "Reglamentos que no contemplan el tema.",
                                "La sanción exclusiva de los casos que llegan a medios de comunicación."
                            }, 0,
                            "Un protocolo conocido por todos, con responsables definidos y confidencialidad garantizada, es lo que convierte una política en protección real.")
                    }
                },
                new TriviaTopic
                {
                    Id = "dm_salud",
                    Title = "Salud y maternidad",
                    Description = "Derechos de salud sexual, reproductiva y atención digna durante el embarazo y el parto.",
                    IconKey = "icon_health",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: tus derechos en el embarazo y el parto",
                        Description = "Imagen con los derechos de la mujer gestante en Bolivia: controles prenatales, licencia de maternidad, subsidios y trato digno en el parto.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/dm_salud_derechos.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("dm_salud_q01",
                            "En Bolivia, ¿cuál es la duración legal de la licencia de maternidad pagada para madres asalariadas?",
                            new[]
                            {
                                "Un total de 30 días calendario únicamente.",
                                "Un total de 90 días calendario (45 días antes del parto y 45 días después).",
                                "Un total de 180 días calendario.",
                                "No tiene duración fija y depende del acuerdo con el empleador."
                            }, 1,
                            "La legislación boliviana otorga a las madres trabajadoras una licencia de maternidad remunerada de 90 días en total, divididos en etapas prenatal y postnatal."),
                        Q("dm_salud_q02",
                            "¿Qué protege el concepto de 'violencia obstétrica', reconocido en la normativa de salud boliviana?",
                            new[]
                            {
                                "El derecho de las mujeres a un trato digno y humanizado durante el embarazo, el parto y el posparto.",
                                "El derecho exclusivo a que todos los partos sean por cesárea.",
                                "La gratuidad total de todos los medicamentos genéricos del país.",
                                "El derecho a elegir el sexo biológico del bebé antes del nacimiento."
                            }, 0,
                            "La violencia obstétrica se refiere al maltrato, la falta de información o el trato deshumanizado durante la atención del embarazo, parto o posparto; su prevención es un derecho reconocido."),
                        Q("dm_salud_q03",
                            "¿Qué garantiza el derecho a la salud sexual y reproductiva de las mujeres en Bolivia?",
                            new[]
                            {
                                "El acceso a información, educación y servicios de salud para decidir libre y responsablemente sobre su cuerpo y su maternidad.",
                                "La obligación de tener un número mínimo de hijos por familia.",
                                "La prohibición total del acceso a métodos anticonceptivos.",
                                "La eliminación de los controles prenatales gratuitos."
                            }, 0,
                            "Este derecho garantiza información, educación y acceso a servicios para tomar decisiones libres e informadas sobre el cuerpo y la maternidad."),
                        Q("dm_salud_q04",
                            "¿Para qué sirven los controles prenatales durante el embarazo?",
                            new[]
                            {
                                "Detectar a tiempo riesgos para la madre y el bebé, y hacer seguimiento del desarrollo del embarazo.",
                                "Cumplir un trámite administrativo sin utilidad médica.",
                                "Definir el nombre del recién nacido.",
                                "Sustituir la atención del parto."
                            }, 0,
                            "El control prenatal oportuno y periódico es una de las medidas más efectivas para reducir la mortalidad materna y neonatal."),
                        Q("dm_salud_q05",
                            "El parto con adecuación intercultural en Bolivia consiste en...",
                            new[]
                            {
                                "Respetar prácticas culturales de la mujer, como la posición vertical o el acompañamiento familiar, dentro del servicio de salud.",
                                "Atender el parto exclusivamente en el domicilio sin personal de salud.",
                                "Prohibir la presencia de familiares en todo momento.",
                                "Imponer un único protocolo sin considerar la cultura de la paciente."
                            }, 0,
                            "La adecuación intercultural aumenta la confianza de las mujeres en el sistema de salud y, con ello, el número de partos atendidos por personal capacitado."),
                        Q("dm_salud_q06",
                            "¿Qué derecho tiene una mujer respecto a la información médica antes de un procedimiento?",
                            new[]
                            {
                                "Recibir información clara y otorgar su consentimiento informado antes de cualquier intervención.",
                                "Aceptar cualquier procedimiento sin explicación previa.",
                                "Delegar obligatoriamente la decisión a un familiar varón.",
                                "Renunciar a conocer su diagnóstico."
                            }, 0,
                            "El consentimiento informado es un derecho: ninguna intervención debe realizarse sin explicar riesgos, alternativas y beneficios en lenguaje comprensible."),
                        Q("dm_salud_q07",
                            "El subsidio prenatal y de lactancia en Bolivia consiste en...",
                            new[]
                            {
                                "Una prestación en productos o su equivalente, otorgada durante el embarazo y los primeros meses de vida del bebé.",
                                "Un préstamo bancario obligatorio para la madre.",
                                "Un descuento sobre el salario de la trabajadora.",
                                "Un impuesto adicional aplicado al empleador."
                            }, 0,
                            "Estos subsidios buscan asegurar una nutrición adecuada de la madre y del recién nacido durante una etapa crítica del desarrollo."),
                        Q("dm_salud_q08",
                            "¿Por qué la detección temprana del cáncer de cuello uterino es prioritaria en Bolivia?",
                            new[]
                            {
                                "Porque es una de las principales causas de muerte por cáncer en mujeres y es altamente prevenible con controles periódicos.",
                                "Porque no existe ningún método de detección disponible.",
                                "Porque afecta únicamente a mujeres mayores de 70 años.",
                                "Porque no tiene tratamiento en ninguna etapa."
                            }, 0,
                            "El Papanicolaou y la vacuna contra el VPH son herramientas efectivas de prevención; la detección temprana cambia radicalmente el pronóstico."),
                        Q("dm_salud_q09",
                            "La lactancia materna es promovida en Bolivia porque...",
                            new[]
                            {
                                "Aporta nutrientes y defensas esenciales al bebé y beneficia también la salud de la madre.",
                                "Es un requisito legal para inscribir al bebé en el registro civil.",
                                "Reemplaza todas las vacunas del esquema infantil.",
                                "Solo se recomienda en el área rural."
                            }, 0,
                            "La normativa protege la lactancia con licencias, salas y horarios específicos, reconociendo su impacto en la salud infantil."),
                        Q("dm_salud_q10",
                            "¿Qué es la salud mental materna y por qué importa?",
                            new[]
                            {
                                "El bienestar emocional durante el embarazo y el posparto; su descuido puede derivar en depresión posparto que requiere atención.",
                                "Un trámite administrativo del seguro de salud.",
                                "Un tema sin relación con el embarazo.",
                                "Una condición que solo afecta a madres primerizas menores de edad."
                            }, 0,
                            "La depresión posparto es frecuente y tratable; identificarla a tiempo protege a la madre y el vínculo con su bebé."),
                        Q("dm_salud_q11",
                            "El embarazo adolescente representa un problema de salud pública porque...",
                            new[]
                            {
                                "Implica mayores riesgos médicos y suele interrumpir la trayectoria educativa y económica de la adolescente.",
                                "No conlleva ningún riesgo diferenciado.",
                                "Mejora las oportunidades laborales de la joven.",
                                "Solo ocurre en zonas urbanas."
                            }, 0,
                            "Prevenirlo requiere educación integral en sexualidad, acceso a servicios amigables para adolescentes y protección frente a la violencia sexual."),
                        Q("dm_salud_q12",
                            "El acceso a métodos anticonceptivos en el sistema público boliviano busca...",
                            new[]
                            {
                                "Permitir decidir de forma libre e informada el número y espaciamiento de los hijos.",
                                "Imponer un límite obligatorio de hijos por familia.",
                                "Restringir el acceso a la información médica.",
                                "Sustituir los controles de salud periódicos."
                            }, 0,
                            "La planificación familiar voluntaria e informada es un componente central de los derechos reproductivos reconocidos en la normativa de salud."),
                        Q("dm_salud_q13",
                            "¿Qué derecho tiene una mujer respecto a la confidencialidad de su atención médica?",
                            new[]
                            {
                                "Que su información clínica se mantenga reservada y no se comparta sin su autorización.",
                                "Que su historia clínica sea de acceso público.",
                                "Que su diagnóstico se informe primero a su empleador.",
                                "Que su expediente se publique en el establecimiento de salud."
                            }, 0,
                            "La confidencialidad es especialmente relevante en salud sexual y reproductiva y en casos de violencia, donde la privacidad protege a la paciente."),
                        Q("dm_salud_q14",
                            "El personal de salud que atiende a una víctima de violencia sexual debe...",
                            new[]
                            {
                                "Brindar atención inmediata, gratuita y sin revictimización, además de activar la ruta de protección y denuncia.",
                                "Exigir una denuncia policial previa para atenderla.",
                                "Cobrar por los servicios de emergencia.",
                                "Derivar el caso sin brindar atención médica."
                            }, 0,
                            "La atención debe ser inmediata y prioritaria; condicionarla a una denuncia previa vulnera derechos y agrava el daño."),
                        Q("dm_salud_q15",
                            "La mortalidad materna se reduce principalmente cuando...",
                            new[]
                            {
                                "Aumentan los controles prenatales, el parto atendido por personal calificado y la atención oportuna de emergencias obstétricas.",
                                "Se reducen los controles durante el embarazo.",
                                "Los partos se atienden sin ningún acompañamiento profesional.",
                                "Se posterga la atención de complicaciones."
                            }, 0,
                            "El acceso oportuno a servicios de calidad, especialmente en áreas rurales y dispersas, es el factor decisivo para reducir las muertes maternas evitables.")
                    }
                },
                new TriviaTopic
                {
                    Id = "dm_patrimonio",
                    Title = "Derechos patrimoniales",
                    Description = "Tierra, herencia, vivienda y autonomía económica: el patrimonio también es un derecho.",
                    IconKey = "icon_property",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: patrimonio y autonomía económica de las mujeres",
                        Description = "Imagen sobre copropiedad de la tierra, derechos hereditarios, régimen patrimonial del matrimonio y acceso a crédito y vivienda.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/dm_patrimonio_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("dm_patrimonio_q01",
                            "En el área rural boliviana, ¿qué representa un avance importante en los derechos patrimoniales de la mujer?",
                            new[]
                            {
                                "El reconocimiento legal de la copropiedad de la tierra entre hombres y mujeres en los títulos agrarios.",
                                "La prohibición legal de que las mujeres hereden tierras.",
                                "La obligación de vender la tierra al cumplir la mayoría de edad.",
                                "La exclusión de las mujeres de las organizaciones campesinas y comunitarias."
                            }, 0,
                            "El reconocimiento de la copropiedad a nombre de ambos cónyuges en los títulos agrarios ha sido un avance clave para la seguridad patrimonial de las mujeres rurales."),
                        Q("dm_patrimonio_q02",
                            "¿Qué derecho hereditario tiene la cónyuge o conviviente al fallecer su pareja en Bolivia?",
                            new[]
                            {
                                "Es heredera forzosa junto con los hijos, según lo establecido en el Código de las Familias y el Código Civil.",
                                "No tiene ningún derecho sobre los bienes del fallecido.",
                                "Solo hereda si no existen hijos ni parientes de ningún grado.",
                                "Debe renunciar a su parte a favor de la familia del fallecido."
                            }, 0,
                            "Conocer los derechos hereditarios evita que muchas mujeres queden desprotegidas patrimonialmente tras el fallecimiento de su pareja."),
                        Q("dm_patrimonio_q03",
                            "En el matrimonio o la unión libre reconocida en Bolivia, los bienes adquiridos durante la convivencia...",
                            new[]
                            {
                                "Se consideran comunes a ambos, salvo excepciones legales, aunque figuren a nombre de uno solo.",
                                "Pertenecen exclusivamente a quien figura en el documento.",
                                "Se reparten en un 80% y 20% por regla general.",
                                "No tienen ningún régimen patrimonial definido."
                            }, 0,
                            "La comunidad de gananciales protege el aporte de ambos, incluido el trabajo doméstico no remunerado, al momento de una separación."),
                        Q("dm_patrimonio_q04",
                            "La unión libre o de hecho estable en Bolivia produce...",
                            new[]
                            {
                                "Los mismos efectos patrimoniales que el matrimonio cuando cumple los requisitos legales.",
                                "Ningún efecto patrimonial en ninguna circunstancia.",
                                "Efectos únicamente sobre los bienes muebles.",
                                "Efectos solo si la pareja tiene hijos en común."
                            }, 0,
                            "El reconocimiento de la unión libre protege a numerosas familias bolivianas que no formalizan su relación mediante matrimonio civil."),
                        Q("dm_patrimonio_q05",
                            "¿Por qué es importante que una mujer figure en el título de propiedad de la vivienda familiar?",
                            new[]
                            {
                                "Porque le otorga seguridad jurídica sobre su patrimonio y respalda su posición ante una separación o herencia.",
                                "Porque es un requisito para votar en elecciones.",
                                "Porque reduce automáticamente los impuestos municipales.",
                                "Porque le impide vender el inmueble en el futuro."
                            }, 0,
                            "La titularidad registrada es la que se reconoce ante terceros; su ausencia deja a muchas mujeres sin respaldo pese a haber aportado durante años."),
                        Q("dm_patrimonio_q06",
                            "La asistencia familiar (pensión de alimentos) para hijos e hijas es...",
                            new[]
                            {
                                "Una obligación legal de ambos progenitores, exigible judicialmente y prioritaria frente a otras deudas.",
                                "Un aporte voluntario sin ninguna consecuencia legal.",
                                "Una obligación exclusiva de la madre.",
                                "Un beneficio que otorga el Estado directamente."
                            }, 0,
                            "El incumplimiento de la asistencia familiar tiene consecuencias legales; su cobro efectivo es clave para la economía de los hogares monoparentales."),
                        Q("dm_patrimonio_q07",
                            "Las políticas de vivienda social en Bolivia priorizan en muchos casos a mujeres jefas de hogar porque...",
                            new[]
                            {
                                "Enfrentan mayor riesgo de precariedad habitacional y sostienen solas el hogar.",
                                "Están exentas de pagar cualquier cuota.",
                                "No requieren evaluación de capacidad de pago.",
                                "Reciben la vivienda sin ningún trámite."
                            }, 0,
                            "La priorización reconoce una vulnerabilidad estructural, sin eliminar los requisitos de evaluación y pago propios de cada programa."),
                        Q("dm_patrimonio_q08",
                            "¿Qué implica la autonomía económica para una mujer?",
                            new[]
                            {
                                "Contar con ingresos propios y capacidad de decidir sobre ellos, lo que reduce su vulnerabilidad frente a la violencia.",
                                "Depender exclusivamente de los ingresos de su pareja.",
                                "Renunciar a la propiedad de sus bienes.",
                                "Delegar todas sus decisiones financieras a terceros."
                            }, 0,
                            "La autonomía económica es un factor protector: la dependencia financiera es uno de los principales obstáculos para salir de una relación violenta."),
                        Q("dm_patrimonio_q09",
                            "Una mujer casada en Bolivia, ¿necesita autorización de su esposo para abrir una cuenta o solicitar un crédito?",
                            new[]
                            {
                                "No: tiene plena capacidad jurídica para contratar por sí misma.",
                                "Sí, siempre requiere autorización escrita del cónyuge.",
                                "Solo si el monto supera un salario mínimo.",
                                "Sí, salvo que cuente con un empleo formal."
                            }, 0,
                            "La capacidad jurídica plena es un derecho reconocido; ninguna entidad puede exigir la autorización del cónyuge para operaciones personales."),
                        Q("dm_patrimonio_q10",
                            "En caso de divorcio, la distribución de los bienes gananciales considera...",
                            new[]
                            {
                                "El aporte de ambos cónyuges durante la unión, incluido el trabajo doméstico y de cuidado.",
                                "Únicamente los ingresos monetarios documentados de cada uno.",
                                "La decisión unilateral del cónyuge con mayores ingresos.",
                                "El orden en que aparecen los nombres en el certificado de matrimonio."
                            }, 0,
                            "Reconocer el trabajo doméstico como aporte al patrimonio común es un avance central para la equidad en los procesos de divorcio."),
                        Q("dm_patrimonio_q11",
                            "El acceso de las mujeres al crédito productivo se fortalece cuando...",
                            new[]
                            {
                                "Se admiten garantías no convencionales y se ofrecen productos adaptados a sus actividades económicas.",
                                "Se exige exclusivamente garantía hipotecaria.",
                                "Se elimina toda evaluación crediticia.",
                                "Se restringen los montos disponibles para emprendimientos."
                            }, 0,
                            "Como muchas mujeres no figuran como titulares de inmuebles, exigir solo garantía hipotecaria las excluye del financiamiento formal."),
                        Q("dm_patrimonio_q12",
                            "El derecho a la identidad y a contar con documentos personales es relevante para el patrimonio porque...",
                            new[]
                            {
                                "Sin cédula de identidad no es posible titular bienes, abrir cuentas ni acceder a créditos o programas sociales.",
                                "Los documentos solo sirven para trámites de viaje.",
                                "No guarda relación con los derechos patrimoniales.",
                                "Solo se requiere en el área urbana."
                            }, 0,
                            "El subregistro de documentación, más frecuente en mujeres rurales, se traduce en exclusión económica y patrimonial concreta."),
                        Q("dm_patrimonio_q13",
                            "¿Qué es la violencia patrimonial contra la mujer?",
                            new[]
                            {
                                "La sustracción, destrucción u ocultamiento de bienes, documentos o recursos económicos destinada a controlarla o someterla.",
                                "El ahorro conjunto acordado entre la pareja.",
                                "La compra de bienes a nombre de ambos.",
                                "La contratación de un seguro patrimonial."
                            }, 0,
                            "Retener documentos, vender bienes comunes sin consentimiento o esconder ingresos son formas de violencia reconocidas por la Ley N° 348."),
                        Q("dm_patrimonio_q14",
                            "Las mujeres productoras que se asocian en cooperativas o asociaciones logran...",
                            new[]
                            {
                                "Mejor acceso a mercados, financiamiento y asistencia técnica que actuando de forma individual.",
                                "Perder la propiedad sobre su producción.",
                                "Renunciar a sus derechos patrimoniales personales.",
                                "Quedar excluidas del sistema financiero formal."
                            }, 0,
                            "La asociatividad mejora el poder de negociación y facilita el acceso a servicios financieros y a compras estatales."),
                        Q("dm_patrimonio_q15",
                            "Elaborar un testamento o registrar correctamente los bienes sirve para...",
                            new[]
                            {
                                "Prevenir conflictos familiares y asegurar que los derechos patrimoniales se respeten conforme a la ley.",
                                "Evitar el pago de cualquier impuesto de forma permanente.",
                                "Transferir bienes sin ningún requisito legal.",
                                "Anular los derechos de los herederos forzosos."
                            }, 0,
                            "El orden documental protege sobre todo a quienes tienen menos poder de negociación dentro de la familia, situación frecuente para las mujeres.")
                    }
                }
            }
        };
    }
}
