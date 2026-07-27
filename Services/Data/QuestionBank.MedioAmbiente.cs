using static GrpcTest.Services.TriviaCatalog;

namespace GrpcTest.Services
{
    public static partial class QuestionBank
    {
        private static TriviaCategory BuildMedioAmbiente() => new TriviaCategory
        {
            Code = "medio_ambiente",
            Label = "Medio Ambiente",
            IconKey = "icon_environment",
            Description = "Conoce los desafíos ambientales de Bolivia y las acciones cotidianas que marcan la diferencia.",
            Topics = new List<TriviaTopic>
            {
                new TriviaTopic
                {
                    Id = "ma_reciclaje",
                    Title = "Reciclaje y residuos",
                    Description = "Separa, reduce y reutiliza: qué hacer con la basura antes de que llegue al relleno sanitario.",
                    IconKey = "icon_recycle",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: cómo separar tus residuos en casa",
                        Description = "Imagen con el código de colores de contenedores, ejemplos de residuos reciclables, orgánicos y especiales, y puntos de acopio urbanos.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ma_reciclaje_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ma_reciclaje_q01",
                            "¿Cuál es el beneficio ambiental más directo de reciclar envases plásticos y latas en nuestras ciudades bolivianas?",
                            new[]
                            {
                                "Incrementar la temperatura en las zonas urbanas.",
                                "Reducir la saturación de los vertederos municipales (como Alpacoma o Kara Kara) y ahorrar materias primas.",
                                "Generar lluvias más frecuentes en los valles del país.",
                                "Eliminar la necesidad de tratamiento de agua potable."
                            }, 1,
                            "Al reciclar evitamos que estos materiales tarden cientos de años en degradarse en los saturados rellenos sanitarios de ciudades como La Paz o Cochabamba."),
                        Q("ma_reciclaje_q02",
                            "¿Cuál es la forma correcta de desechar aparatos electrónicos en desuso (celulares, baterías, cargadores)?",
                            new[]
                            {
                                "Llevarlos a puntos de acopio o campañas de reciclaje electrónico especializado.",
                                "Tirarlos junto con la basura orgánica doméstica común.",
                                "Quemarlos directamente en el patio de la casa.",
                                "Enterrarlos en cualquier terreno baldío cercano."
                            }, 0,
                            "Los residuos electrónicos contienen metales pesados y componentes tóxicos que deben tratarse en puntos de acopio especializados para evitar la contaminación de suelo y agua."),
                        Q("ma_reciclaje_q03",
                            "En la regla de las 3R, ¿cuál es el orden de prioridad correcto?",
                            new[]
                            {
                                "Reducir, reutilizar y luego reciclar.",
                                "Reciclar, reducir y luego reutilizar.",
                                "Reutilizar, reciclar y luego reducir.",
                                "El orden es indistinto, las tres tienen el mismo impacto."
                            }, 0,
                            "Reciclar consume energía y transporte: lo más efectivo es primero no generar el residuo (reducir), luego darle una segunda vida (reutilizar) y recién después reciclarlo."),
                        Q("ma_reciclaje_q04",
                            "¿Qué se puede hacer con los residuos orgánicos de cocina (cáscaras, restos de verduras) en un hogar boliviano?",
                            new[]
                            {
                                "Compostarlos para obtener abono natural y reducir el volumen de basura enviada al relleno.",
                                "Mezclarlos con pilas y baterías usadas.",
                                "Quemarlos en la calle cada semana.",
                                "Verterlos por el desagüe con abundante detergente."
                            }, 0,
                            "Los orgánicos representan buena parte de la basura domiciliaria; compostarlos produce abono y reduce la generación de gases en los rellenos sanitarios."),
                        Q("ma_reciclaje_q05",
                            "¿Por qué es importante enjuagar y aplastar los envases plásticos antes de llevarlos a un punto de acopio?",
                            new[]
                            {
                                "Porque reduce el volumen a transportar y evita que los restos de alimento contaminen el material reciclable.",
                                "Porque así el plástico se convierte en vidrio.",
                                "Porque de lo contrario el municipio cobra una multa por envase.",
                                "Porque los envases limpios no necesitan reciclarse."
                            }, 0,
                            "Un material contaminado con restos orgánicos suele terminar descartado; limpiarlo y compactarlo mejora su valor y facilita la logística del reciclaje."),
                        Q("ma_reciclaje_q06",
                            "¿Qué papel cumplen las personas recolectoras de material reciclable en las ciudades bolivianas?",
                            new[]
                            {
                                "Recuperan gran parte del material aprovechable que de otro modo terminaría en el relleno sanitario.",
                                "Solo generan desorden sin ningún aporte ambiental.",
                                "Se encargan exclusivamente del barrido de calles municipales.",
                                "Administran los rellenos sanitarios del país."
                            }, 0,
                            "Buena parte del reciclaje en Bolivia se sostiene en el trabajo de recolectores y recolectoras; separar los residuos en casa hace su labor más segura y productiva."),
                        Q("ma_reciclaje_q07",
                            "¿Cuánto tiempo puede tardar aproximadamente una botella de plástico PET en degradarse en el ambiente?",
                            new[]
                            {
                                "Cientos de años, según las condiciones del entorno.",
                                "Un par de semanas.",
                                "Alrededor de seis meses.",
                                "Se degrada de forma inmediata al contacto con el agua."
                            }, 0,
                            "El PET puede permanecer siglos en el ambiente fragmentándose en microplásticos, que llegan a ríos, suelos y a la cadena alimentaria."),
                        Q("ma_reciclaje_q08",
                            "¿Qué problema genera quemar basura a cielo abierto, práctica todavía común en algunas zonas del país?",
                            new[]
                            {
                                "Libera gases y partículas tóxicas que dañan la salud respiratoria y contaminan el aire.",
                                "Purifica el aire del vecindario.",
                                "Convierte los residuos en abono natural.",
                                "Reduce la temperatura del entorno inmediato."
                            }, 0,
                            "La quema de residuos, sobre todo plásticos, emite dioxinas y material particulado; es una de las causas de la mala calidad del aire en varias ciudades bolivianas."),
                        Q("ma_reciclaje_q09",
                            "¿Qué residuos se consideran 'especiales' y no deben ir a la basura común?",
                            new[]
                            {
                                "Pilas, baterías, medicamentos vencidos, aceites usados y focos fluorescentes.",
                                "Cáscaras de fruta y restos de verdura.",
                                "Papel de cuaderno usado.",
                                "Botellas de vidrio enteras."
                            }, 0,
                            "Estos residuos contienen sustancias peligrosas que requieren canales de disposición diferenciados para no contaminar suelos y aguas subterráneas."),
                        Q("ma_reciclaje_q10",
                            "¿Cuál es el impacto de llevar bolsas reutilizables al mercado o a la feria?",
                            new[]
                            {
                                "Reduce de manera significativa el consumo de bolsas plásticas de un solo uso a lo largo del año.",
                                "Aumenta el costo de los productos comprados.",
                                "No tiene ningún efecto ambiental medible.",
                                "Obliga al comerciante a pagar un impuesto extra."
                            }, 0,
                            "Una familia puede evitar cientos de bolsas plásticas al año con este simple cambio de hábito, reduciendo residuos difíciles de reciclar."),
                        Q("ma_reciclaje_q11",
                            "El vidrio se considera un material especialmente valioso para el reciclaje porque...",
                            new[]
                            {
                                "Puede reciclarse indefinidamente sin perder calidad ni propiedades.",
                                "Se degrada naturalmente en pocos meses.",
                                "No requiere ningún proceso industrial para reutilizarse.",
                                "Pierde calidad tras el primer reciclado y debe descartarse."
                            }, 0,
                            "El vidrio admite ciclos ilimitados de reciclaje, y fundirlo requiere menos energía que producirlo desde materia prima virgen."),
                        Q("ma_reciclaje_q12",
                            "En términos ambientales, ¿qué significa la 'economía circular'?",
                            new[]
                            {
                                "Un modelo donde los materiales se mantienen en uso el mayor tiempo posible mediante reutilización, reparación y reciclaje.",
                                "Un modelo donde se produce y se descarta lo más rápido posible.",
                                "Un sistema de préstamos bancarios para empresas industriales.",
                                "La circulación de productos importados exclusivamente."
                            }, 0,
                            "La economía circular contrasta con el modelo lineal 'extraer-usar-tirar' y busca que los residuos de un proceso se conviertan en insumos de otro."),
                        Q("ma_reciclaje_q13",
                            "¿Qué es el 'punto verde' o punto de acopio en un barrio o institución?",
                            new[]
                            {
                                "Un espacio destinado a recibir materiales reciclables separados por tipo para su posterior aprovechamiento.",
                                "Un parque urbano sin árboles.",
                                "Una oficina municipal de cobro de impuestos.",
                                "Un sitio para arrojar residuos peligrosos sin control."
                            }, 0,
                            "Los puntos de acopio conectan la separación domiciliaria con las empresas recicladoras, y su éxito depende de que la gente entregue material limpio y clasificado."),
                        Q("ma_reciclaje_q14",
                            "¿Qué efecto tiene el consumo responsable (comprar solo lo necesario, preferir productos duraderos) sobre los residuos?",
                            new[]
                            {
                                "Reduce la generación de basura en origen, que es la forma más eficaz de disminuir el impacto ambiental.",
                                "Aumenta el volumen de residuos en los rellenos.",
                                "No modifica la cantidad de residuos generados.",
                                "Solo afecta a las empresas, nunca a los hogares."
                            }, 0,
                            "El residuo que no se genera no necesita transporte, tratamiento ni disposición final: reducir en origen es siempre más eficiente que gestionar después."),
                        Q("ma_reciclaje_q15",
                            "¿Qué son los microplásticos y por qué preocupan?",
                            new[]
                            {
                                "Fragmentos plásticos diminutos que contaminan agua y suelos y pueden ingresar a la cadena alimentaria.",
                                "Envases plásticos de tamaño pequeño usados en cosmética.",
                                "Un tipo de plástico biodegradable en 24 horas.",
                                "Residuos que desaparecen al contacto con la luz solar."
                            }, 0,
                            "Los microplásticos provienen de la fragmentación de residuos plásticos y ya se detectan en ríos, peces y agua de consumo, con efectos aún en estudio sobre la salud.")
                    }
                },
                new TriviaTopic
                {
                    Id = "ma_agua",
                    Title = "Agua y glaciares",
                    Description = "El agua en Bolivia: glaciares que retroceden, lagos que se secan y cómo cuidarla cada día.",
                    IconKey = "icon_water",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: el agua que baja de los Andes",
                        Description = "Imagen sobre el retroceso de los glaciares andinos, el abastecimiento de agua en La Paz y El Alto, y prácticas de uso eficiente en el hogar.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ma_agua_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ma_agua_q01",
                            "¿Qué ocurrió con el Lago Poopó, en Oruro, alertando sobre el cambio climático y el mal uso del agua en Bolivia?",
                            new[]
                            {
                                "Se secó casi por completo debido a la sequía, el desvío de afluentes y el cambio climático.",
                                "Se congeló de forma permanente durante todo el año.",
                                "Aumentó su nivel hasta inundar la ciudad de Oruro.",
                                "Se convirtió en una fuente directa de agua potable para consumo humano."
                            }, 0,
                            "El Lago Poopó, antes el segundo más grande de Bolivia, se secó casi en su totalidad por la sequía prolongada, el desvío de sus afluentes para riego y minería, y el cambio climático."),
                        Q("ma_agua_q02",
                            "El glaciar Chacaltaya, cercano a La Paz, es un caso emblemático a nivel mundial porque...",
                            new[]
                            {
                                "Desapareció por completo debido al calentamiento global, siendo uno de los primeros glaciares tropicales en extinguirse.",
                                "Aumentó considerablemente su tamaño en los últimos años.",
                                "Se transformó en una laguna de aguas termales.",
                                "Nunca tuvo hielo permanente en su historia."
                            }, 0,
                            "El Chacaltaya, que llegó a albergar la pista de esquí más alta del mundo, se derritió por completo alrededor de 2009, convirtiéndose en un símbolo global del retroceso glaciar."),
                        Q("ma_agua_q03",
                            "¿Por qué el retroceso de los glaciares andinos preocupa al abastecimiento de agua de ciudades como La Paz y El Alto?",
                            new[]
                            {
                                "Porque los glaciares son una fuente natural de agua dulce que alimenta ríos y reservorios usados para consumo humano.",
                                "Porque los glaciares generan toda la electricidad que consume la ciudad.",
                                "Porque su deshielo aumenta la producción agrícola de forma indefinida.",
                                "Porque no tienen ninguna relación con el suministro de agua potable."
                            }, 0,
                            "Los glaciares andinos actúan como reservas naturales que regulan el caudal de las cuencas; su retroceso amenaza la disponibilidad de agua en las ciudades del altiplano."),
                        Q("ma_agua_q04",
                            "¿Qué es una cuenca hidrográfica?",
                            new[]
                            {
                                "El territorio cuyas aguas de lluvia y deshielo drenan hacia un mismo río, lago o sistema hídrico.",
                                "Un depósito artificial construido por el municipio.",
                                "El conjunto de tuberías de agua potable de una ciudad.",
                                "Una zona donde está prohibido el uso de agua."
                            }, 0,
                            "Gestionar el agua por cuenca permite entender que lo que ocurre aguas arriba (deforestación, contaminación minera) afecta directamente a las comunidades de aguas abajo."),
                        Q("ma_agua_q05",
                            "¿Qué efecto tiene la contaminación minera con mercurio en los ríos amazónicos bolivianos?",
                            new[]
                            {
                                "El mercurio se acumula en peces y afecta la salud de las comunidades que los consumen.",
                                "Purifica el agua eliminando bacterias.",
                                "Incrementa la reproducción de especies nativas.",
                                "No tiene efectos porque el mercurio se evapora de inmediato."
                            }, 0,
                            "El mercurio usado en la minería aurífera se bioacumula en la cadena trófica; comunidades indígenas amazónicas presentan niveles preocupantes por el consumo de pescado."),
                        Q("ma_agua_q06",
                            "¿Cuál es una práctica efectiva de ahorro de agua en el hogar?",
                            new[]
                            {
                                "Cerrar la llave mientras se enjabona o cepilla los dientes y reparar de inmediato las fugas.",
                                "Dejar correr el agua para que 'se enfríe' durante varios minutos.",
                                "Lavar el vehículo con manguera abierta todos los días.",
                                "Regar el jardín al mediodía, con máxima radiación solar."
                            }, 0,
                            "Una fuga de goteo continuo puede desperdiciar miles de litros al año; los cambios de hábito y las reparaciones oportunas son las medidas más costo-efectivas."),
                        Q("ma_agua_q07",
                            "La denominada 'crisis del agua' que afectó a La Paz en 2016 dejó como principal lección que...",
                            new[]
                            {
                                "La dependencia de pocas represas y el estrés hídrico exigen planificación, diversificación de fuentes y uso eficiente.",
                                "El agua es un recurso infinito en el altiplano.",
                                "Las lluvias siempre garantizan el abastecimiento urbano.",
                                "No es necesario invertir en infraestructura hídrica."
                            }, 0,
                            "El racionamiento de 2016 evidenció la vulnerabilidad del sistema de abastecimiento frente a sequías y al retroceso glaciar, y la necesidad de gestionar la demanda."),
                        Q("ma_agua_q08",
                            "¿Por qué el Lago Titicaca requiere acciones binacionales de protección?",
                            new[]
                            {
                                "Porque es compartido por Bolivia y Perú, y la contaminación de sus afluentes afecta a ambos países.",
                                "Porque pertenece exclusivamente a un municipio boliviano.",
                                "Porque no recibe ningún tipo de contaminación.",
                                "Porque su superficie es demasiado pequeña para gestionarse localmente."
                            }, 0,
                            "La contaminación por aguas residuales y residuos sólidos en la bahía interior y sus afluentes exige coordinación entre ambos países y sus municipios ribereños."),
                        Q("ma_agua_q09",
                            "¿Qué es el 'estrés hídrico' de una región?",
                            new[]
                            {
                                "La situación en la que la demanda de agua supera la disponibilidad o la calidad del recurso es insuficiente.",
                                "El aumento de la presión del agua en las tuberías.",
                                "La cantidad de lluvia que cae en un solo día.",
                                "El costo mensual de la factura de agua potable."
                            }, 0,
                            "Buena parte del altiplano y de los valles bolivianos presenta estrés hídrico estacional, agravado por el crecimiento urbano y la variabilidad climática."),
                        Q("ma_agua_q10",
                            "¿Qué función ambiental cumplen los bofedales y humedales altoandinos?",
                            new[]
                            {
                                "Almacenan y regulan agua, sostienen la ganadería de camélidos y albergan biodiversidad única.",
                                "Impiden por completo el crecimiento de vegetación nativa.",
                                "Sirven únicamente como terrenos para construcción urbana.",
                                "Aceleran la evaporación total de las cuencas."
                            }, 0,
                            "Los bofedales funcionan como esponjas naturales que regulan el caudal de las cuencas altoandinas y son clave para las comunidades que crían llamas y alpacas."),
                        Q("ma_agua_q11",
                            "¿Qué es el tratamiento de aguas residuales y por qué importa en las ciudades bolivianas?",
                            new[]
                            {
                                "Es el proceso que depura las aguas servidas antes de devolverlas al ambiente, evitando contaminar ríos y lagos.",
                                "Es el proceso de embotellar agua para la venta.",
                                "Es la extracción de agua subterránea para riego.",
                                "Es la desalinización del agua de mar."
                            }, 0,
                            "Cuando la cobertura de tratamiento es baja, las aguas servidas llegan crudas a los ríos, afectando la salud, la agricultura de riego y los ecosistemas acuáticos."),
                        Q("ma_agua_q12",
                            "La cosecha o siembra de agua de lluvia en comunidades rurales consiste en...",
                            new[]
                            {
                                "Captar y almacenar agua de lluvia en atajados, tanques o q'ochas para usarla en época seca.",
                                "Sembrar cultivos únicamente durante la época de lluvias.",
                                "Extraer agua de pozos profundos sin límite.",
                                "Desviar ríos hacia zonas urbanas."
                            }, 0,
                            "Estas técnicas, algunas de origen ancestral, permiten a las comunidades altoandinas y de valle enfrentar la estacionalidad de las lluvias y las sequías."),
                        Q("ma_agua_q13",
                            "¿Qué relación existe entre la deforestación y la disponibilidad de agua?",
                            new[]
                            {
                                "Los bosques regulan el ciclo del agua y su pérdida reduce la infiltración, aumentando la erosión y las inundaciones.",
                                "La deforestación incrementa de forma permanente el caudal de los ríos.",
                                "No existe ninguna relación entre bosques y agua.",
                                "Los bosques consumen agua sin aportar ningún beneficio hídrico."
                            }, 0,
                            "Al perder cobertura boscosa, el suelo retiene menos agua: aumentan las crecidas en época de lluvia y la escasez en época seca."),
                        Q("ma_agua_q14",
                            "¿Por qué se recomienda no arrojar aceite de cocina usado por el desagüe?",
                            new[]
                            {
                                "Porque contamina grandes volúmenes de agua y obstruye el sistema de alcantarillado.",
                                "Porque el aceite mejora el funcionamiento de las tuberías.",
                                "Porque el aceite se disuelve completamente en el agua sin efecto alguno.",
                                "Porque es la única forma de eliminarlo correctamente."
                            }, 0,
                            "Un litro de aceite puede contaminar miles de litros de agua; conviene almacenarlo en un envase cerrado y entregarlo en puntos de acopio para su reciclaje."),
                        Q("ma_agua_q15",
                            "El derecho humano al agua, reconocido en la Constitución boliviana, implica que...",
                            new[]
                            {
                                "El Estado debe garantizar el acceso al agua potable como derecho fundamental, no como mercancía.",
                                "El agua puede privatizarse libremente sin regulación.",
                                "Solo las áreas urbanas tienen derecho al servicio.",
                                "El acceso al agua depende exclusivamente de la capacidad de pago."
                            }, 0,
                            "La Constitución boliviana reconoce el acceso al agua y al saneamiento como derechos fundamentales, y prohíbe su concesión o privatización.")
                    }
                },
                new TriviaTopic
                {
                    Id = "ma_biodiversidad",
                    Title = "Biodiversidad y áreas protegidas",
                    Description = "Bolivia es uno de los países megadiversos del planeta: conoce sus parques y las amenazas que enfrentan.",
                    IconKey = "icon_biodiversity",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: áreas protegidas de Bolivia",
                        Description = "Imagen con las principales áreas protegidas nacionales, su biodiversidad, las comunidades que las habitan y las amenazas actuales.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ma_biodiversidad_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ma_biodiversidad_q01",
                            "¿Cuál es una de las principales amenazas para la biodiversidad del Parque Nacional Madidi?",
                            new[]
                            {
                                "La reforestación con árboles nativos de la Amazonía.",
                                "La deforestación ilegal y la contaminación de ríos por minería aurífera sin control.",
                                "El incremento del turismo ecológico regulado y sostenible.",
                                "El desarrollo de técnicas agrícolas tradicionales de rotación de cultivos."
                            }, 1,
                            "La minería aurífera ilegal que libera mercurio en los ríos y la deforestación representan graves peligros para la fauna y las comunidades indígenas del Madidi."),
                        Q("ma_biodiversidad_q02",
                            "¿Qué distinción otorgada por la UNESCO tiene el Parque Nacional Noel Kempff Mercado?",
                            new[]
                            {
                                "Patrimonio Natural de la Humanidad.",
                                "Patrimonio Cultural Inmaterial de la Humanidad.",
                                "Reserva mundial oficial de agua potable.",
                                "Capital mundial de la biodiversidad marina."
                            }, 0,
                            "El Parque Nacional Noel Kempff Mercado, en Santa Cruz, fue declarado Patrimonio Natural de la Humanidad por la UNESCO gracias a su excepcional biodiversidad."),
                        Q("ma_biodiversidad_q03",
                            "¿Cuál es el principal objetivo de las áreas protegidas en Bolivia, como los parques nacionales?",
                            new[]
                            {
                                "Conservar la biodiversidad, los ecosistemas y los recursos naturales para las futuras generaciones.",
                                "Permitir la explotación minera sin ningún tipo de restricción.",
                                "Fomentar la construcción urbana descontrolada dentro de sus límites.",
                                "Eliminar por completo el acceso de las comunidades indígenas a su territorio."
                            }, 0,
                            "Las áreas protegidas buscan conservar ecosistemas, especies y recursos estratégicos, muchas veces en coexistencia con el manejo sostenible por parte de comunidades locales e indígenas."),
                        Q("ma_biodiversidad_q04",
                            "¿Qué evento ambiental afectó gravemente los bosques secos de la Chiquitania, en Santa Cruz, durante 2019?",
                            new[]
                            {
                                "Grandes incendios forestales originados por quemas agrícolas descontroladas.",
                                "Una inundación causada por el deshielo de glaciares cercanos.",
                                "Una plaga masiva de langostas.",
                                "Un terremoto de gran magnitud."
                            }, 0,
                            "En 2019 la Chiquitania sufrió incendios forestales masivos, en gran parte provocados por quemas agrícolas que se salieron de control en época seca, destruyendo millones de hectáreas."),
                        Q("ma_biodiversidad_q05",
                            "En las zonas productoras de quinua real del altiplano sur, ¿qué riesgo ambiental genera el monocultivo intensivo sin rotación ni descanso de la tierra?",
                            new[]
                            {
                                "La erosión y degradación de los suelos, reduciendo su fertilidad a largo plazo.",
                                "Un aumento indefinido e ilimitado de la fertilidad del suelo.",
                                "La eliminación total de la necesidad de lluvia para el cultivo.",
                                "La desaparición completa y permanente de las plagas agrícolas."
                            }, 0,
                            "El monocultivo intensivo sin periodos de descanso ni rotación agota los nutrientes del suelo altiplánico y acelera su erosión, poniendo en riesgo la producción futura."),
                        Q("ma_biodiversidad_q06",
                            "¿Por qué se dice que Bolivia es un país 'megadiverso'?",
                            new[]
                            {
                                "Porque concentra una enorme variedad de ecosistemas y especies, desde el altiplano hasta la Amazonía.",
                                "Porque tiene la mayor superficie continental de Sudamérica.",
                                "Porque cuenta con salida directa a dos océanos.",
                                "Porque posee un único ecosistema muy extendido."
                            }, 0,
                            "Bolivia figura entre los países con mayor diversidad biológica del planeta gracias a su gradiente altitudinal, que va de los Andes a las llanuras amazónicas y chaqueñas."),
                        Q("ma_biodiversidad_q07",
                            "¿Qué es un TIOC (Territorio Indígena Originario Campesino) en relación con la conservación?",
                            new[]
                            {
                                "Un territorio con gestión indígena reconocida legalmente, donde el manejo tradicional contribuye a conservar los ecosistemas.",
                                "Un área destinada exclusivamente a la explotación forestal industrial.",
                                "Una reserva militar sin presencia de población civil.",
                                "Un parque urbano administrado por el municipio."
                            }, 0,
                            "Muchos territorios indígenas se superponen con áreas protegidas y su gestión tradicional ha sido clave para mantener bosques en buen estado de conservación."),
                        Q("ma_biodiversidad_q08",
                            "El Parque Nacional Sajama, en Oruro, se caracteriza por...",
                            new[]
                            {
                                "Ser la primera área protegida creada en Bolivia y albergar los bosques de queñua más altos del mundo.",
                                "Ser una reserva marina de aguas profundas.",
                                "Estar ubicado en plena llanura amazónica.",
                                "No contar con presencia de comunidades locales."
                            }, 0,
                            "Creado en 1939, el Sajama fue la primera área protegida del país y protege ecosistemas altoandinos únicos, incluidos bosques de queñua a más de 4.000 metros."),
                        Q("ma_biodiversidad_q09",
                            "¿Qué amenaza representa el tráfico ilegal de fauna silvestre en Bolivia?",
                            new[]
                            {
                                "Reduce las poblaciones de especies nativas y rompe el equilibrio de los ecosistemas.",
                                "Fortalece la reproducción de las especies traficadas.",
                                "Es una práctica autorizada si el animal es pequeño.",
                                "Solo afecta a especies introducidas, nunca a las nativas."
                            }, 0,
                            "El tráfico de loros, tortugas, felinos y otras especies es un delito que diezma poblaciones silvestres y suele implicar altísima mortalidad durante el transporte."),
                        Q("ma_biodiversidad_q10",
                            "La deforestación por expansión de la frontera agrícola en el oriente boliviano provoca principalmente...",
                            new[]
                            {
                                "Pérdida de hábitat, liberación de carbono y mayor riesgo de incendios y erosión.",
                                "Aumento permanente de la fertilidad del suelo tropical.",
                                "Incremento de la biodiversidad local.",
                                "Reducción automática de las emisiones de gases de efecto invernadero."
                            }, 0,
                            "El chaqueo y el desmonte para agricultura y ganadería son los principales motores de la pérdida de bosque en Bolivia, con impactos sobre el clima y la biodiversidad."),
                        Q("ma_biodiversidad_q11",
                            "¿Qué es una especie endémica?",
                            new[]
                            {
                                "Una especie que solo existe de forma natural en una región determinada y en ningún otro lugar del mundo.",
                                "Una especie introducida desde otro continente.",
                                "Una especie que se encuentra en todos los continentes.",
                                "Una especie criada únicamente en cautiverio."
                            }, 0,
                            "Bolivia alberga numerosas especies endémicas, como la rana gigante del Titicaca o el paraba barba azul, cuya extinción local significaría su desaparición definitiva."),
                        Q("ma_biodiversidad_q12",
                            "El chaqueo o quema para habilitar terrenos agrícolas se vuelve especialmente peligroso cuando...",
                            new[]
                            {
                                "Se realiza en época seca y con vientos fuertes, condiciones en las que el fuego escapa de control.",
                                "Se ejecuta bajo lluvia intensa y con suelos húmedos.",
                                "Se realiza en superficies muy reducidas y controladas.",
                                "Se hace con autorización y supervisión técnica."
                            }, 0,
                            "La combinación de sequía, vientos y quemas sin control ha originado los grandes incendios forestales que Bolivia enfrenta casi cada año."),
                        Q("ma_biodiversidad_q13",
                            "¿Qué beneficio aporta el turismo comunitario bien gestionado en áreas protegidas?",
                            new[]
                            {
                                "Genera ingresos locales que incentivan la conservación y reducen la presión sobre actividades extractivas.",
                                "Obliga a las comunidades a abandonar el área protegida.",
                                "Elimina la necesidad de guardaparques.",
                                "Aumenta el desmonte dentro del parque."
                            }, 0,
                            "Cuando el turismo se organiza con la propia comunidad y con límites de carga, la conservación pasa a tener un valor económico directo para quienes habitan el territorio."),
                        Q("ma_biodiversidad_q14",
                            "¿Cuál es la función de los guardaparques en las áreas protegidas bolivianas?",
                            new[]
                            {
                                "Proteger y monitorear el área, prevenir actividades ilegales y apoyar la gestión junto a las comunidades.",
                                "Cobrar impuestos municipales dentro del parque.",
                                "Construir carreteras dentro del área protegida.",
                                "Autorizar la caza comercial de especies nativas."
                            }, 0,
                            "Los guardaparques son la primera línea de defensa de las áreas protegidas, muchas veces con recursos limitados frente a extensiones enormes."),
                        Q("ma_biodiversidad_q15",
                            "¿Por qué la Amazonía boliviana es relevante frente al cambio climático global?",
                            new[]
                            {
                                "Porque sus bosques almacenan grandes cantidades de carbono y regulan el clima y las lluvias regionales.",
                                "Porque no cumple ninguna función climática relevante.",
                                "Porque su clima es idéntico al del altiplano.",
                                "Porque carece de biodiversidad significativa."
                            }, 0,
                            "Los bosques amazónicos capturan y almacenan carbono y participan en el transporte de humedad continental; su pérdida acelera el calentamiento global.")
                    }
                },
                new TriviaTopic
                {
                    Id = "ma_energia",
                    Title = "Energía y cambio climático",
                    Description = "Energías renovables, litio y calidad del aire: la transición energética vista desde Bolivia.",
                    IconKey = "icon_energy",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: la transición energética en Bolivia",
                        Description = "Imagen sobre las plantas solares y eólicas del país, el rol del litio y qué puede hacer un hogar para reducir su huella de carbono.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ma_energia_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ma_energia_q01",
                            "En Bolivia, ¿cuál de las siguientes opciones representa una fuente de energía renovable clave en pleno desarrollo en el altiplano?",
                            new[]
                            {
                                "El carbón mineral.",
                                "La energía solar fotovoltaica (como en la planta solar de Oruro).",
                                "El gas natural licuado.",
                                "La energía de fisión nuclear."
                            }, 1,
                            "Bolivia cuenta con plantas solares que aprovechan la alta radiación del altiplano para generar energía limpia y renovable."),
                        Q("ma_energia_q02",
                            "¿Por qué el Salar de Uyuni es estratégicamente importante para Bolivia en términos de recursos naturales?",
                            new[]
                            {
                                "Por sus reservas de litio, un mineral clave para las baterías de vehículos eléctricos.",
                                "Por ser una gran reserva de petróleo crudo.",
                                "Por su producción histórica de carbón mineral.",
                                "Por ser el único yacimiento de oro del país."
                            }, 0,
                            "El Salar de Uyuni contiene una de las mayores reservas de litio del mundo, recurso estratégico para la fabricación de baterías y la transición energética global."),
                        Q("ma_energia_q03",
                            "¿Cuál es una de las principales causas de la mala calidad del aire en ciudades como La Paz y Cochabamba?",
                            new[]
                            {
                                "La quema de basura a cielo abierto y las emisiones de los vehículos.",
                                "La altura sobre el nivel del mar en la que se ubica la ciudad.",
                                "El exceso de áreas verdes y parques urbanos.",
                                "La ausencia total de industrias en la región metropolitana."
                            }, 0,
                            "La quema de residuos, el crecimiento del parque automotor y las emisiones vehiculares son factores clave detrás de la mala calidad del aire en las principales ciudades bolivianas."),
                        Q("ma_energia_q04",
                            "¿Qué son los gases de efecto invernadero?",
                            new[]
                            {
                                "Gases como el CO2 y el metano que retienen calor en la atmósfera y, en exceso, elevan la temperatura global.",
                                "Gases que enfrían la atmósfera de forma natural.",
                                "Gases utilizados exclusivamente en la industria alimentaria.",
                                "Gases que destruyen únicamente la capa de ozono, sin efecto climático."
                            }, 0,
                            "El efecto invernadero es natural y necesario; el problema es su intensificación por emisiones humanas provenientes de combustibles fósiles, deforestación y ganadería."),
                        Q("ma_energia_q05",
                            "¿Qué representa la 'huella de carbono' de una persona u organización?",
                            new[]
                            {
                                "La cantidad total de gases de efecto invernadero emitidos por sus actividades.",
                                "La cantidad de agua consumida al año.",
                                "El número de árboles plantados en su vida.",
                                "La superficie de terreno que ocupa físicamente."
                            }, 0,
                            "Medir la huella de carbono permite identificar qué actividades (transporte, energía, consumo) generan más emisiones y dónde conviene reducirlas."),
                        Q("ma_energia_q06",
                            "La central hidroeléctrica es una fuente de energía renovable, pero su construcción puede generar...",
                            new[]
                            {
                                "Impactos sobre ecosistemas fluviales y desplazamiento de comunidades si no se evalúa adecuadamente.",
                                "Cero impacto ambiental en cualquier circunstancia.",
                                "Emisiones de gases superiores a las de una termoeléctrica a carbón.",
                                "Un aumento inmediato de la deforestación en el altiplano."
                            }, 0,
                            "Ser renovable no significa ser inocua: los grandes embalses transforman ríos, afectan la fauna acuática y pueden requerir el traslado de poblaciones."),
                        Q("ma_energia_q07",
                            "¿Qué ventaja tiene el uso de focos LED frente a los focos incandescentes tradicionales?",
                            new[]
                            {
                                "Consumen mucha menos electricidad y duran varias veces más, reduciendo emisiones y costo de la factura.",
                                "Consumen más energía pero iluminan igual.",
                                "Deben reemplazarse cada mes.",
                                "No pueden usarse en instalaciones domiciliarias."
                            }, 0,
                            "El cambio a iluminación LED es una de las medidas de eficiencia energética más simples y rentables para un hogar."),
                        Q("ma_energia_q08",
                            "El gas natural, principal recurso energético boliviano, se considera un combustible...",
                            new[]
                            {
                                "Fósil: emite menos CO2 que el carbón o el diésel, pero sigue siendo una fuente no renovable.",
                                "Totalmente renovable e inagotable.",
                                "Libre de emisiones de gases de efecto invernadero.",
                                "De origen solar y por lo tanto limpio."
                            }, 0,
                            "El gas natural es un combustible fósil de transición: es menos intensivo en carbono que otros hidrocarburos, pero su uso continúa aportando emisiones."),
                        Q("ma_energia_q09",
                            "El parque eólico de Qollpana, en Cochabamba, es relevante porque...",
                            new[]
                            {
                                "Fue el primer proyecto de generación eólica conectado al sistema eléctrico nacional boliviano.",
                                "Es la mayor planta de gas natural del país.",
                                "Genera energía a partir de residuos sólidos urbanos.",
                                "Se trata de una represa hidroeléctrica de gran escala."
                            }, 0,
                            "Qollpana marcó el inicio de la generación eólica en Bolivia y abrió el camino a nuevos parques en el país."),
                        Q("ma_energia_q10",
                            "¿Qué significa 'eficiencia energética' en un hogar o una empresa?",
                            new[]
                            {
                                "Obtener el mismo servicio (luz, calor, movimiento) consumiendo menos energía.",
                                "Consumir toda la energía disponible sin restricciones.",
                                "Reemplazar la electricidad por velas.",
                                "Aumentar la potencia contratada al máximo."
                            }, 0,
                            "La energía más limpia y barata es la que no se consume: la eficiencia reduce costos y emisiones sin sacrificar el servicio."),
                        Q("ma_energia_q11",
                            "¿Cómo afecta el cambio climático a la agricultura boliviana?",
                            new[]
                            {
                                "Altera los patrones de lluvia, intensifica sequías, heladas y granizadas, y afecta los rendimientos de los cultivos.",
                                "Garantiza cosechas abundantes todos los años.",
                                "No tiene ningún efecto sobre la producción agrícola.",
                                "Elimina por completo las plagas agrícolas."
                            }, 0,
                            "La variabilidad climática creciente afecta especialmente a la agricultura familiar de secano, que depende directamente del régimen de lluvias."),
                        Q("ma_energia_q12",
                            "El transporte público masivo y sistemas como el teleférico de La Paz contribuyen al ambiente porque...",
                            new[]
                            {
                                "Reducen la cantidad de vehículos particulares circulando y, con ello, las emisiones por pasajero.",
                                "Incrementan el consumo de combustibles fósiles por persona.",
                                "Aumentan la congestión vehicular en la ciudad.",
                                "No tienen ningún efecto sobre las emisiones urbanas."
                            }, 0,
                            "Mover a muchas personas con menos vehículos reduce las emisiones por pasajero-kilómetro y mejora la calidad del aire urbano."),
                        Q("ma_energia_q13",
                            "¿Qué es la industrialización del litio que Bolivia impulsa?",
                            new[]
                            {
                                "El proceso de transformar el litio extraído en productos de mayor valor, como carbonato de litio y baterías.",
                                "La exportación del salar completo sin procesamiento.",
                                "La conversión del litio en gas natural.",
                                "La prohibición total de explotar el recurso."
                            }, 0,
                            "El objetivo es no quedarse en la exportación de materia prima, sino generar valor agregado y empleo con la cadena productiva del litio."),
                        Q("ma_energia_q14",
                            "¿Qué medida individual reduce de forma significativa la huella de carbono del hogar?",
                            new[]
                            {
                                "Preferir transporte público o caminata, reducir el desperdicio de alimentos y usar electrodomésticos eficientes.",
                                "Dejar los aparatos electrónicos encendidos permanentemente.",
                                "Quemar residuos en el patio de la casa.",
                                "Comprar productos de un solo uso con mayor frecuencia."
                            }, 0,
                            "Transporte, alimentación y consumo eléctrico son los tres grandes componentes de la huella de un hogar; ahí están las mejores oportunidades de reducción."),
                        Q("ma_energia_q15",
                            "¿Qué es la adaptación al cambio climático, a diferencia de la mitigación?",
                            new[]
                            {
                                "Ajustar sistemas y comunidades para reducir daños ante impactos ya inevitables, mientras la mitigación busca reducir las emisiones.",
                                "Son exactamente lo mismo con distinto nombre.",
                                "Consiste únicamente en plantar árboles.",
                                "Se refiere a la exportación de tecnologías limpias."
                            }, 0,
                            "Bolivia, altamente vulnerable por su geografía, necesita ambas estrategias: reducir emisiones y, a la vez, preparar ciudades, cuencas y agricultura para impactos ya en curso.")
                    }
                }
            }
        };
    }
}
