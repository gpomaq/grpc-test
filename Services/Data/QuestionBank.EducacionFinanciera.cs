using static GrpcTest.Services.TriviaCatalog;

namespace GrpcTest.Services
{
    public static partial class QuestionBank
    {
        private static TriviaCategory BuildEducacionFinanciera() => new TriviaCategory
        {
            Code = "educacion_financiera",
            Label = "Educación Financiera",
            IconKey = "icon_finance",
            Description = "Aprende a manejar tu dinero, ahorrar con propósito y usar el crédito de forma responsable.",
            Topics = new List<TriviaTopic>
            {
                new TriviaTopic
                {
                    Id = "ef_presupuesto",
                    Title = "Presupuesto",
                    Description = "Ordena tus ingresos y gastos mensuales para que el dinero te alcance y te sobre.",
                    IconKey = "icon_budget",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: arma tu presupuesto familiar en 5 pasos",
                        Description = "Imagen con ejemplos en bolivianos para registrar ingresos, gastos fijos, gastos variables y metas de ahorro.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ef_presupuesto_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ef_presupuesto_q01",
                            "Para una familia boliviana, ¿cuál es el objetivo primordial de elaborar un presupuesto mensual en bolivianos (Bs.)?",
                            new[]
                            {
                                "Maximizar el límite de uso de las tarjetas de crédito.",
                                "Planificar y controlar los ingresos frente a los gastos, priorizando el ahorro y el pago de deudas.",
                                "Evitar por completo cualquier tipo de consumo cultural o recreativo familiar.",
                                "Reportar todos los consumos directamente al Servicio de Impuestos Nacionales."
                            }, 1,
                            "Un presupuesto permite ordenar las finanzas familiares, asegurando que se cubran las necesidades básicas y se destine un porcentaje al ahorro antes de gastar."),
                        Q("ef_presupuesto_q02",
                            "¿Cuál es la diferencia entre un gasto fijo y un gasto variable en un presupuesto mensual?",
                            new[]
                            {
                                "El gasto fijo se repite con el mismo monto cada mes (alquiler, colegiatura) y el variable cambia según el consumo (feria, transporte).",
                                "El gasto fijo se paga en efectivo y el variable siempre con tarjeta.",
                                "El gasto fijo lo paga el jefe de familia y el variable el resto del hogar.",
                                "No existe diferencia real; ambos términos significan lo mismo."
                            }, 0,
                            "Identificar cuáles gastos son fijos y cuáles variables permite saber qué parte del presupuesto se puede ajustar cuando los ingresos bajan."),
                        Q("ef_presupuesto_q03",
                            "La regla 50/30/20 sugiere distribuir el ingreso mensual. ¿Qué representa cada porcentaje?",
                            new[]
                            {
                                "50% impuestos, 30% alquiler y 20% transporte.",
                                "50% ahorro, 30% deudas y 20% necesidades.",
                                "50% necesidades básicas, 30% gustos personales y 20% ahorro o pago de deudas.",
                                "50% educación, 30% salud y 20% vestimenta."
                            }, 2,
                            "La regla 50/30/20 es una guía simple: la mitad del ingreso para lo indispensable, un 30% para gustos y al menos un 20% destinado a ahorro o a reducir deudas."),
                        Q("ef_presupuesto_q04",
                            "¿Por qué se recomienda registrar los 'gastos hormiga' (refrescos, snacks, pasajes extra) en el presupuesto?",
                            new[]
                            {
                                "Porque son montos pequeños pero frecuentes que, sumados al mes, representan una fuga importante de dinero.",
                                "Porque el banco exige ese detalle para mantener una cuenta de ahorros.",
                                "Porque son los únicos gastos que se pueden deducir de impuestos.",
                                "Porque sin ese registro no se puede abrir una cuenta bancaria."
                            }, 0,
                            "Los gastos hormiga pasan desapercibidos por su bajo monto individual, pero al acumularse pueden consumir una parte significativa del ingreso mensual."),
                        Q("ef_presupuesto_q05",
                            "Si en un mes tus gastos superan a tus ingresos, tu presupuesto está en...",
                            new[]
                            {
                                "Superávit, lo que permite ahorrar más.",
                                "Déficit, situación que suele cubrirse con deudas si no se corrige.",
                                "Equilibrio perfecto entre ingresos y egresos.",
                                "Inflación, un fenómeno exclusivo del presupuesto familiar."
                            }, 1,
                            "Un presupuesto deficitario significa que se gasta más de lo que ingresa; sostenerlo en el tiempo obliga a endeudarse y deteriora la salud financiera del hogar."),
                        Q("ef_presupuesto_q06",
                            "En Bolivia, los pagos mediante código QR interoperable impulsados por el Banco Central de Bolivia permiten principalmente...",
                            new[]
                            {
                                "Obtener automáticamente un préstamo bancario preaprobado.",
                                "Eliminar por completo la necesidad de tener una cuenta en una entidad financiera.",
                                "Realizar transferencias y pagos electrónicos entre distintos bancos y billeteras de forma rápida y sin efectivo.",
                                "Garantizar de forma automática tasas de interés preferenciales."
                            }, 2,
                            "El QR interoperable boliviano permite pagar y transferir dinero entre cuentas de distintas entidades financieras de forma inmediata, promoviendo la inclusión financiera y reduciendo el uso de efectivo."),
                        Q("ef_presupuesto_q07",
                            "¿Qué ventaja tiene revisar los extractos o el historial de movimientos de tu cuenta al cerrar el mes?",
                            new[]
                            {
                                "Permite detectar consumos olvidados, cobros duplicados o suscripciones que ya no usas.",
                                "Aumenta automáticamente la tasa de interés que paga el banco por tus ahorros.",
                                "Elimina la obligación de pagar las cuotas de un préstamo vigente.",
                                "Sustituye la necesidad de planificar el mes siguiente."
                            }, 0,
                            "Revisar los movimientos es el control de calidad del presupuesto: confirma que lo planificado coincide con lo realmente gastado y detecta cargos indebidos a tiempo."),
                        Q("ef_presupuesto_q08",
                            "Una familia recibe el aguinaldo en diciembre. ¿Cuál es el uso más prudente desde la perspectiva del presupuesto anual?",
                            new[]
                            {
                                "Gastarlo íntegramente en regalos y celebraciones de fin de año.",
                                "Destinar una parte a deudas caras o al fondo de emergencia y planificar el resto para los gastos de inicio de año.",
                                "Prestarlo completo a conocidos sin ningún acuerdo escrito.",
                                "Guardarlo en efectivo sin destino definido hasta que se acabe."
                            }, 1,
                            "El aguinaldo es un ingreso extraordinario previsible: usarlo para reducir deudas costosas o cubrir los gastos de enero (matrículas, material escolar) evita empezar el año endeudado."),
                        Q("ef_presupuesto_q09",
                            "¿Qué significa 'págate a ti mismo primero' al elaborar un presupuesto?",
                            new[]
                            {
                                "Separar el monto destinado al ahorro apenas se recibe el ingreso, antes de empezar a gastar.",
                                "Cobrarte una comisión personal por administrar el dinero del hogar.",
                                "Pagar primero todos los gustos personales y luego las cuentas del hogar.",
                                "Retirar todo el sueldo en efectivo el mismo día de pago."
                            }, 0,
                            "Ahorrar al inicio del mes convierte al ahorro en una prioridad y no en el sobrante, que muchas veces nunca llega."),
                        Q("ef_presupuesto_q10",
                            "Para un comerciante de un mercado o una feria, ¿por qué es importante separar el dinero del negocio del dinero personal?",
                            new[]
                            {
                                "Porque así se conoce la ganancia real del negocio y se evita consumir el capital de trabajo.",
                                "Porque la ley obliga a tener dos billeteras físicas distintas.",
                                "Porque el dinero del negocio no se puede depositar en un banco.",
                                "Porque mezclar ambos incrementa automáticamente las ventas."
                            }, 0,
                            "Mezclar caja del negocio y gastos del hogar impide medir la rentabilidad real y suele terminar descapitalizando el emprendimiento."),
                        Q("ef_presupuesto_q11",
                            "¿Qué es un presupuesto de flujo de caja mensual para un hogar con ingresos variables (por ejemplo, por ventas o trabajo independiente)?",
                            new[]
                            {
                                "Una proyección de cuándo entra y cuándo sale el dinero, para anticipar los meses de menor ingreso.",
                                "Un crédito automático que otorga el banco cada mes.",
                                "Un impuesto adicional que pagan los trabajadores independientes.",
                                "Un listado únicamente de las deudas pendientes."
                            }, 0,
                            "Cuando el ingreso no es fijo, proyectar entradas y salidas por semana o mes permite guardar en los meses buenos para cubrir los meses bajos."),
                        Q("ef_presupuesto_q12",
                            "Al planificar el inicio del año escolar en Bolivia, ¿cuál es la práctica presupuestaria más recomendable?",
                            new[]
                            {
                                "Ahorrar mensualmente un monto pequeño durante el año para cubrir matrículas, uniformes y material escolar.",
                                "Cubrir todo con crédito de consumo en enero, sin planificación previa.",
                                "Postergar indefinidamente el pago de la matrícula.",
                                "Reducir el gasto en alimentación del hogar para cubrirlo."
                            }, 0,
                            "Los gastos escolares son predecibles: repartirlos en cuotas de ahorro a lo largo del año evita recurrir a créditos caros en enero o febrero."),
                        Q("ef_presupuesto_q13",
                            "¿Qué indica un 'presupuesto base cero' aplicado a las finanzas del hogar?",
                            new[]
                            {
                                "Que cada mes se justifica desde cero cada gasto, en lugar de repetir automáticamente los del mes anterior.",
                                "Que el hogar debe terminar el mes con saldo cero en la cuenta.",
                                "Que no se debe destinar ningún monto al ahorro.",
                                "Que todos los gastos se pagan con crédito."
                            }, 0,
                            "El presupuesto base cero obliga a preguntarse si cada gasto sigue siendo necesario, lo que ayuda a eliminar consumos heredados por costumbre."),
                        Q("ef_presupuesto_q14",
                            "¿Cuál es una señal clara de que el nivel de endeudamiento del hogar está afectando su presupuesto?",
                            new[]
                            {
                                "Que las cuotas de deudas consuman una porción tan alta del ingreso que impidan cubrir gastos básicos o ahorrar.",
                                "Que el hogar tenga una cuenta de ahorros abierta.",
                                "Que se registre cada gasto en una libreta o aplicación.",
                                "Que el hogar cuente con un seguro de salud."
                            }, 0,
                            "Como referencia práctica, cuando el pago de cuotas supera aproximadamente un tercio del ingreso mensual, el presupuesto queda sin margen para imprevistos ni ahorro."),
                        Q("ef_presupuesto_q15",
                            "¿Por qué conviene fijar metas financieras con monto y plazo (por ejemplo, 'Bs. 6.000 en 12 meses') en lugar de metas generales?",
                            new[]
                            {
                                "Porque una meta concreta permite calcular cuánto ahorrar cada mes y medir el avance real.",
                                "Porque el banco otorga premios por escribir metas.",
                                "Porque las metas generales están prohibidas por la normativa financiera.",
                                "Porque de esa forma se evita pagar comisiones bancarias."
                            }, 0,
                            "Una meta con monto y plazo se traduce en una cuota mensual verificable; sin esos datos, el propósito de ahorrar rara vez se sostiene en el tiempo.")
                    }
                },
                new TriviaTopic
                {
                    Id = "ef_ahorro",
                    Title = "Ahorro",
                    Description = "Construye un colchón financiero y haz que tu dinero mantenga su valor en el tiempo.",
                    IconKey = "icon_savings",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: cómo empezar a ahorrar aunque tu ingreso sea variable",
                        Description = "Imagen que explica el fondo de emergencia, el interés compuesto y cómo elegir entre caja de ahorro y depósito a plazo fijo.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ef_ahorro_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ef_ahorro_q01",
                            "¿Cuál es el principal beneficio del interés compuesto al ahorrar en un banco boliviano?",
                            new[]
                            {
                                "Calcula intereses únicamente sobre el monto del depósito inicial.",
                                "Genera intereses sobre el capital inicial y también sobre los intereses acumulados previamente.",
                                "Garantiza que la inflación nunca afectará el valor real de tus ahorros.",
                                "Se aplica únicamente en préstamos de consumo a muy corto plazo."
                            }, 1,
                            "El interés compuesto suma los intereses ganados al capital inicial de forma periódica, haciendo que el interés futuro se calcule sobre un monto mayor."),
                        Q("ef_ahorro_q02",
                            "¿Cuál es el propósito principal de contar con un 'fondo de emergencia'?",
                            new[]
                            {
                                "Cubrir gastos imprevistos, como salud o pérdida de empleo, sin recurrir a deudas costosas.",
                                "Reemplazar por completo el aporte destinado a la jubilación.",
                                "Usarse exclusivamente para viajes y vacaciones familiares.",
                                "Evitar tener que abrir una cuenta de ahorros formal en un banco."
                            }, 0,
                            "Un fondo de emergencia brinda respaldo financiero ante imprevistos, evitando que la familia deba endeudarse a tasas altas para cubrir gastos urgentes."),
                        Q("ef_ahorro_q03",
                            "¿Por qué es importante considerar la inflación al planificar un ahorro de largo plazo en bolivianos?",
                            new[]
                            {
                                "Porque la inflación reduce el poder adquisitivo del dinero si el ahorro no genera un rendimiento superior a ella.",
                                "Porque la inflación siempre incrementa automáticamente el valor real de los ahorros.",
                                "Porque la inflación es un fenómeno que solo afecta las finanzas del Estado.",
                                "Porque la inflación elimina automáticamente cualquier deuda pendiente."
                            }, 0,
                            "Si el rendimiento del ahorro es menor a la inflación, el dinero pierde poder de compra con el tiempo, por lo que es clave buscar instrumentos que protejan o superen ese efecto."),
                        Q("ef_ahorro_q04",
                            "¿Qué diferencia principal existe entre una caja de ahorro y un depósito a plazo fijo (DPF) en Bolivia?",
                            new[]
                            {
                                "La caja de ahorro permite disponer del dinero cuando se quiera con menor rendimiento; el DPF inmoviliza el dinero un plazo pactado a cambio de una tasa mayor.",
                                "El DPF permite retirar el dinero en cualquier momento sin ninguna condición.",
                                "La caja de ahorro solo está disponible para empresas.",
                                "Ambos productos pagan exactamente la misma tasa de interés."
                            }, 0,
                            "El DPF premia con una tasa pasiva más alta el compromiso de no tocar el dinero durante el plazo acordado, mientras que la caja de ahorro prioriza la disponibilidad inmediata."),
                        Q("ef_ahorro_q05",
                            "Muchas familias bolivianas reciben remesas de familiares en el exterior. ¿Cuál es la recomendación financiera más prudente al recibirlas?",
                            new[]
                            {
                                "Gastar la totalidad de inmediato en bienes de consumo no esenciales.",
                                "Destinar una parte al ahorro o a una inversión productiva en lugar de consumirla por completo.",
                                "Guardar siempre el dinero en efectivo dentro de la casa, sin usar el sistema financiero.",
                                "Evitar informarse sobre el tipo de cambio o las comisiones de envío."
                            }, 1,
                            "Destinar parte de la remesa al ahorro formal o a una inversión productiva ayuda a la familia a generar estabilidad financiera a largo plazo, en vez de depender solo del consumo inmediato."),
                        Q("ef_ahorro_q06",
                            "¿Cuál es una señal de alerta común de un esquema de estafa piramidal o financiera en Bolivia?",
                            new[]
                            {
                                "Prometer rendimientos muy altos y garantizados en poco tiempo, pagando a los primeros participantes con el dinero de los nuevos.",
                                "Estar registrada y supervisada por la Autoridad de Supervisión del Sistema Financiero (ASFI).",
                                "Ofrecer tasas de interés similares a las del mercado financiero formal.",
                                "Entregar contratos y comprobantes verificables de cada operación realizada."
                            }, 0,
                            "Las estafas piramidales suelen atraer víctimas prometiendo ganancias extraordinarias y garantizadas, sosteniéndose únicamente con el dinero de nuevos participantes hasta que colapsan."),
                        Q("ef_ahorro_q07",
                            "¿Qué entidad administra actualmente los aportes para la jubilación (pensiones) de los trabajadores bolivianos?",
                            new[]
                            {
                                "La Gestora Pública de la Seguridad Social de Largo Plazo.",
                                "El Banco Central de Bolivia de forma directa.",
                                "Cada banco comercial de manera individual, según su criterio.",
                                "Ninguna entidad; el aporte para la jubilación es completamente voluntario."
                            }, 0,
                            "La Gestora Pública de la Seguridad Social de Largo Plazo es la entidad que administra los aportes jubilatorios de los trabajadores en Bolivia."),
                        Q("ef_ahorro_q08",
                            "¿De cuánto suele recomendarse que sea un fondo de emergencia familiar?",
                            new[]
                            {
                                "El equivalente a entre tres y seis meses de gastos básicos del hogar.",
                                "Exactamente el monto de un salario mínimo nacional, sin importar los gastos.",
                                "El 1% del ingreso anual, como máximo.",
                                "No existe recomendación; cualquier monto simbólico basta."
                            }, 0,
                            "La referencia habitual es cubrir entre tres y seis meses de gastos esenciales, tiempo razonable para reorganizarse ante una pérdida de ingresos."),
                        Q("ef_ahorro_q09",
                            "¿Qué ventaja tiene programar una transferencia automática hacia una cuenta de ahorro cada mes?",
                            new[]
                            {
                                "Convierte el ahorro en un hábito que no depende de la voluntad ni de lo que sobre a fin de mes.",
                                "Obliga al banco a duplicar el monto ahorrado.",
                                "Elimina el cobro de cualquier comisión bancaria.",
                                "Impide realizar cualquier otro gasto durante el mes."
                            }, 0,
                            "La automatización elimina la decisión mensual de ahorrar y reduce la tentación de gastar ese dinero antes de guardarlo."),
                        Q("ef_ahorro_q10",
                            "En Bolivia, los ahorros del público en entidades financieras autorizadas cuentan con un respaldo institucional. ¿Cuál es?",
                            new[]
                            {
                                "La supervisión de la ASFI y los mecanismos de protección al ahorrista previstos en la Ley de Servicios Financieros.",
                                "Una garantía personal firmada por el gerente de cada sucursal.",
                                "Un seguro internacional contratado por cada cliente de forma obligatoria.",
                                "Ningún respaldo: los depósitos no están regulados."
                            }, 0,
                            "Ahorrar en entidades reguladas y supervisadas por la ASFI brinda un marco de protección que no existe en esquemas informales o 'pasanakus' de alto riesgo."),
                        Q("ef_ahorro_q11",
                            "¿Qué diferencia hay entre ahorrar y invertir?",
                            new[]
                            {
                                "Ahorrar es reservar dinero con bajo riesgo y alta disponibilidad; invertir busca mayor rendimiento asumiendo más riesgo y plazo.",
                                "Son sinónimos exactos en el sistema financiero.",
                                "Ahorrar siempre genera más rendimiento que invertir.",
                                "Invertir está prohibido para personas naturales en Bolivia."
                            }, 0,
                            "El ahorro protege y da liquidez; la inversión busca crecimiento del capital aceptando fluctuaciones y plazos más largos. Ambos cumplen funciones distintas dentro de un plan financiero."),
                        Q("ef_ahorro_q12",
                            "Ahorrar en dólares frente a ahorrar en bolivianos implica considerar principalmente...",
                            new[]
                            {
                                "El riesgo cambiario y las tasas de interés diferentes que ofrece cada moneda.",
                                "Que los ahorros en moneda extranjera están prohibidos en el país.",
                                "Que la moneda extranjera nunca pierde valor.",
                                "Que ambas monedas siempre rinden exactamente igual."
                            }, 0,
                            "La elección de moneda debe considerar en qué moneda se reciben los ingresos y se pagan los gastos, además de la tasa ofrecida y el riesgo de tipo de cambio."),
                        Q("ef_ahorro_q13",
                            "¿Por qué se dice que empezar a ahorrar joven tiene una ventaja matemática?",
                            new[]
                            {
                                "Porque mientras más tiempo permanezca el dinero invertido, más actúa el interés compuesto sobre los rendimientos acumulados.",
                                "Porque las entidades financieras pagan tasas más altas solo a menores de 30 años.",
                                "Porque los jóvenes están exentos de comisiones bancarias.",
                                "Porque el ahorro juvenil se duplica automáticamente por ley."
                            }, 0,
                            "El tiempo es el factor más poderoso del interés compuesto: aportes pequeños sostenidos durante muchos años superan a aportes grandes iniciados tarde."),
                        Q("ef_ahorro_q14",
                            "Un 'pasanaku' informal entre conocidos, comparado con una cuenta de ahorro en una entidad regulada, se caracteriza por...",
                            new[]
                            {
                                "Depender de la confianza entre participantes, sin supervisión ni garantía formal si alguien incumple.",
                                "Estar supervisado por la ASFI igual que un banco.",
                                "Ofrecer siempre una tasa de interés garantizada por contrato.",
                                "Contar con seguro obligatorio de depósitos."
                            }, 0,
                            "El pasanaku puede ser útil como disciplina de ahorro colectivo, pero no ofrece protección legal ni supervisión: si un integrante incumple, no hay mecanismo formal de recuperación."),
                        Q("ef_ahorro_q15",
                            "Antes de retirar el fondo de emergencia, ¿qué criterio conviene aplicar?",
                            new[]
                            {
                                "Usarlo solo ante gastos urgentes e imprevistos, y reponerlo apenas la situación se normalice.",
                                "Usarlo cada vez que aparezca una oferta o promoción atractiva.",
                                "Retirarlo completo cada fin de año por costumbre.",
                                "Prestarlo a terceros para generar rendimiento adicional."
                            }, 0,
                            "El fondo de emergencia cumple su función solo si se reserva para lo imprevisto y se repone después de usarlo; de lo contrario deja de ser un respaldo real.")
                    }
                },
                new TriviaTopic
                {
                    Id = "ef_creditos",
                    Title = "Créditos",
                    Description = "Entiende tasas, cuotas e historial crediticio antes de firmar un préstamo.",
                    IconKey = "icon_credit",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: antes de firmar, qué revisar en un contrato de crédito",
                        Description = "Imagen que explica la tasa de interés, el costo total del crédito, el plan de pagos y los derechos del consumidor financiero en Bolivia.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ef_creditos_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ef_creditos_q01",
                            "Antes de otorgar un préstamo, un banco boliviano consulta la Central de Información Crediticia (CIRC). ¿Para qué sirve esta central?",
                            new[]
                            {
                                "Para registrar el historial de pagos y el nivel de endeudamiento de las personas.",
                                "Para funcionar como un club de ahorro voluntario entre clientes.",
                                "Para cobrar un impuesto adicional sobre cada crédito otorgado.",
                                "Para emitir un seguro de vida obligatorio junto al préstamo."
                            }, 0,
                            "La CIRC, administrada por la ASFI, centraliza el historial crediticio de las personas para que las entidades financieras evalúen el riesgo antes de prestar dinero."),
                        Q("ef_creditos_q02",
                            "En el sistema financiero boliviano, ¿cuál es la diferencia entre la 'tasa de interés activa' y la 'tasa de interés pasiva'?",
                            new[]
                            {
                                "La activa es la que cobra el banco por los préstamos y la pasiva es la que paga por los depósitos de ahorro.",
                                "Son exactamente el mismo concepto, solo con distinto nombre.",
                                "La activa solo aplica a empresas y la pasiva únicamente a personas naturales.",
                                "La tasa pasiva siempre es más alta que la tasa activa."
                            }, 0,
                            "La tasa activa es el interés que cobra la entidad financiera al prestar dinero, mientras que la tasa pasiva es el interés que paga a sus clientes por sus depósitos."),
                        Q("ef_creditos_q03",
                            "Bolivia es reconocida internacionalmente como cuna de las microfinanzas modernas. ¿Qué institución boliviana fue pionera mundial en microcrédito?",
                            new[]
                            {
                                "BancoSol.",
                                "El Banco Mercantil Santa Cruz.",
                                "El Banco de Desarrollo Productivo.",
                                "El Banco Unión."
                            }, 0,
                            "BancoSol, surgido de una ONG de microcrédito en los años 90, es reconocido mundialmente como uno de los primeros bancos comerciales especializados en microfinanzas."),
                        Q("ef_creditos_q04",
                            "¿Qué representa el 'costo total del crédito' que las entidades deben informar al cliente?",
                            new[]
                            {
                                "La suma del capital, los intereses, seguros y comisiones que el cliente pagará durante toda la vida del préstamo.",
                                "Únicamente el monto desembolsado al inicio.",
                                "Solo los intereses del primer mes.",
                                "El impuesto que cobra el Estado sobre el préstamo."
                            }, 0,
                            "Comparar créditos solo por la tasa nominal puede engañar: el costo total incluye comisiones y seguros, y es la cifra que realmente permite comparar ofertas."),
                        Q("ef_creditos_q05",
                            "¿Qué diferencia hay entre un crédito de consumo y un crédito productivo?",
                            new[]
                            {
                                "El de consumo financia gastos personales y el productivo financia actividades que generan ingresos, como capital de trabajo o maquinaria.",
                                "El productivo solo puede otorgarse a personas asalariadas.",
                                "El de consumo siempre tiene una tasa menor al productivo.",
                                "Ambos financian exactamente lo mismo."
                            }, 0,
                            "En Bolivia el crédito productivo está orientado a actividades que generan ingresos y suele tener condiciones de tasa reguladas más favorables que el crédito de consumo."),
                        Q("ef_creditos_q06",
                            "¿Qué consecuencia tiene atrasarse reiteradamente en el pago de las cuotas de un préstamo?",
                            new[]
                            {
                                "Se generan intereses penales, la calificación crediticia se deteriora y se dificulta acceder a nuevos créditos.",
                                "El saldo de la deuda se reduce automáticamente.",
                                "La entidad está obligada a condonar la deuda.",
                                "No ocurre nada mientras se pague algún día."
                            }, 0,
                            "El historial de pagos queda registrado en la central de información crediticia; la mora encarece el crédito futuro y puede cerrar el acceso al financiamiento formal."),
                        Q("ef_creditos_q07",
                            "¿Qué es el periodo de gracia en un crédito?",
                            new[]
                            {
                                "Un lapso inicial en el que no se paga capital (y a veces tampoco intereses), pactado con la entidad financiera.",
                                "Un descuento definitivo del 50% de la deuda.",
                                "El plazo para arrepentirse y anular el contrato sin costo.",
                                "El tiempo que tarda el banco en desembolsar el dinero."
                            }, 0,
                            "El periodo de gracia da aire al inicio, especialmente en créditos productivos cuyo negocio aún no genera ingresos, pero normalmente los intereses se siguen acumulando."),
                        Q("ef_creditos_q08",
                            "¿Por qué conviene comparar ofertas de crédito entre varias entidades antes de decidir?",
                            new[]
                            {
                                "Porque la tasa, el plazo, las comisiones y los seguros varían y pueden significar una diferencia importante en el monto total pagado.",
                                "Porque la ley obliga a solicitar crédito en al menos tres entidades.",
                                "Porque comparar mejora automáticamente la calificación crediticia.",
                                "Porque todas las entidades ofrecen exactamente las mismas condiciones."
                            }, 0,
                            "Las condiciones difieren entre entidades y productos; comparar el costo total en bolivianos, y no solo la cuota mensual, evita pagar de más durante años."),
                        Q("ef_creditos_q09",
                            "Un crédito con cuota fija en el sistema de amortización tradicional se caracteriza porque...",
                            new[]
                            {
                                "La cuota se mantiene constante, pero al inicio se paga proporcionalmente más interés y menos capital.",
                                "Cada cuota amortiza exactamente la misma cantidad de intereses.",
                                "El capital se paga íntegramente en la primera cuota.",
                                "La cuota crece automáticamente cada mes."
                            }, 0,
                            "En un plan de cuota fija, la composición cambia con el tiempo: los primeros pagos cubren mayormente intereses y recién luego amortizan capital de forma significativa."),
                        Q("ef_creditos_q10",
                            "¿Qué implica ser 'garante' o codeudor de un crédito de otra persona?",
                            new[]
                            {
                                "Asumir la obligación de pagar la deuda si el titular no cumple, lo que también afecta tu propia capacidad de endeudamiento.",
                                "Solo firmar un documento sin ninguna consecuencia legal.",
                                "Recibir automáticamente la mitad del monto prestado.",
                                "Obtener una comisión mensual de la entidad financiera."
                            }, 0,
                            "El garante responde con su patrimonio y su historial crediticio ante el incumplimiento del titular; no es un trámite simbólico."),
                        Q("ef_creditos_q11",
                            "¿Qué es la 'capacidad de pago' que evalúa una entidad financiera?",
                            new[]
                            {
                                "La relación entre los ingresos estables del solicitante y las cuotas que deberá pagar, considerando sus otras deudas.",
                                "El total de bienes que el solicitante posee, sin importar sus ingresos.",
                                "La cantidad de cuentas bancarias abiertas por el solicitante.",
                                "El número de años que lleva viviendo en su domicilio."
                            }, 0,
                            "La capacidad de pago mide si el flujo de ingresos alcanza para cubrir la cuota sin comprometer los gastos esenciales; es distinta de la garantía, que respalda el crédito."),
                        Q("ef_creditos_q12",
                            "Sobreendeudarse significa...",
                            new[]
                            {
                                "Tener un nivel de deudas cuyas cuotas superan la capacidad real de pago del hogar o del negocio.",
                                "Tener más de una cuenta de ahorros abierta.",
                                "Pagar las cuotas antes de la fecha de vencimiento.",
                                "Solicitar información sobre créditos en varias entidades."
                            }, 0,
                            "El sobreendeudamiento aparece cuando se toman créditos nuevos para pagar los anteriores; la salida suele requerir reestructurar deudas y reordenar el presupuesto."),
                        Q("ef_creditos_q13",
                            "¿Qué es una refinanciación o reprogramación de deuda?",
                            new[]
                            {
                                "Un acuerdo con la entidad para modificar plazo o cuotas cuando el cliente enfrenta dificultades de pago.",
                                "La condonación total e inmediata de la deuda.",
                                "El traspaso de la deuda a un familiar sin su consentimiento.",
                                "Una multa adicional aplicada por el regulador."
                            }, 0,
                            "Reprogramar a tiempo, antes de caer en mora prolongada, permite ajustar la cuota a la nueva realidad del cliente y proteger su historial crediticio."),
                        Q("ef_creditos_q14",
                            "Al usar una tarjeta de crédito, pagar únicamente el 'pago mínimo' cada mes tiene como efecto principal...",
                            new[]
                            {
                                "Extender la deuda en el tiempo y pagar mucho más en intereses acumulados.",
                                "Cancelar totalmente la deuda del mes.",
                                "Eliminar los intereses del periodo.",
                                "Aumentar automáticamente el límite de la tarjeta sin costo."
                            }, 0,
                            "El pago mínimo cubre apenas una fracción del capital: el saldo restante sigue generando intereses, convirtiendo compras pequeñas en deudas largas y caras."),
                        Q("ef_creditos_q15",
                            "¿Cuál es un derecho del consumidor financiero en Bolivia frente a una entidad regulada?",
                            new[]
                            {
                                "Recibir información clara, completa y oportuna sobre las condiciones del producto y presentar reclamos ante la entidad y la ASFI.",
                                "Exigir que se le apruebe cualquier crédito solicitado.",
                                "Dejar de pagar las cuotas si no está conforme con la atención.",
                                "Obtener siempre la tasa de interés más baja del mercado."
                            }, 0,
                            "La Ley de Servicios Financieros reconoce el derecho a información transparente y a canales de reclamo, incluido el Defensor del Consumidor Financiero y la ASFI.")
                    }
                },
                new TriviaTopic
                {
                    Id = "ef_garantias",
                    Title = "Garantías",
                    Description = "Conoce qué respalda un préstamo: hipotecas, prendas, garantes y garantías no convencionales.",
                    IconKey = "icon_guarantee",
                    Resource = new TriviaResource
                    {
                        Type = "image",
                        Title = "Infografía: tipos de garantía en el sistema financiero boliviano",
                        Description = "Imagen comparativa entre garantía hipotecaria, prendaria, personal y no convencional, con ejemplos de uso en créditos productivos y de vivienda.",
                        Url = "https://cdn.yasta.bo/trivias/recursos/ef_garantias_infografia.png"
                    },
                    Questions = new List<MockQuestion>
                    {
                        Q("ef_garantias_q01",
                            "¿Qué función cumple una garantía en un crédito?",
                            new[]
                            {
                                "Respaldar el cumplimiento de la deuda, reduciendo el riesgo de la entidad financiera si el cliente no paga.",
                                "Reemplazar la evaluación de la capacidad de pago del cliente.",
                                "Aumentar automáticamente el monto de la cuota mensual.",
                                "Eliminar la obligación de firmar un contrato."
                            }, 0,
                            "La garantía es una fuente secundaria de pago: respalda la operación, pero no sustituye la evaluación de si el cliente puede pagar con sus ingresos."),
                        Q("ef_garantias_q02",
                            "¿Qué caracteriza a una garantía hipotecaria?",
                            new[]
                            {
                                "Un bien inmueble (casa, terreno) queda registrado como respaldo del crédito ante Derechos Reales.",
                                "Se entrega un bien mueble en depósito al banco.",
                                "Una persona firma comprometiendo solo su palabra.",
                                "Se entrega el saldo de una cuenta de ahorros como respaldo."
                            }, 0,
                            "En la garantía hipotecaria el inmueble se grava a favor de la entidad mediante su inscripción en Derechos Reales, y el propietario conserva el uso del bien mientras cumple con los pagos."),
                        Q("ef_garantias_q03",
                            "¿Qué es una garantía prendaria?",
                            new[]
                            {
                                "El respaldo del crédito con un bien mueble, como maquinaria, vehículos o equipos.",
                                "El respaldo con un terreno inscrito en Derechos Reales.",
                                "Un aval firmado por una institución pública.",
                                "Un seguro de desgravamen contratado por el cliente."
                            }, 0,
                            "La prenda recae sobre bienes muebles (vehículos, maquinaria, inventarios) y es muy usada en créditos productivos de pequeños empresarios."),
                        Q("ef_garantias_q04",
                            "En una garantía personal o solidaria, ¿quién responde ante el incumplimiento?",
                            new[]
                            {
                                "El garante, con su patrimonio e historial crediticio, según lo pactado en el contrato.",
                                "Únicamente el Estado boliviano.",
                                "El regulador financiero (ASFI).",
                                "Nadie: la deuda se extingue automáticamente."
                            }, 0,
                            "Firmar como garante solidario implica que la entidad puede exigir el pago al garante en las mismas condiciones que al deudor principal."),
                        Q("ef_garantias_q05",
                            "La Ley de Servicios Financieros boliviana reconoce las 'garantías no convencionales'. ¿Qué buscan?",
                            new[]
                            {
                                "Ampliar el acceso al crédito de productores y emprendedores que no cuentan con inmuebles, aceptando por ejemplo productos almacenados, semovientes o contratos.",
                                "Reemplazar la firma del contrato de préstamo.",
                                "Eliminar la evaluación crediticia de los solicitantes.",
                                "Aplicar únicamente a créditos de grandes empresas."
                            }, 0,
                            "Las garantías no convencionales (documentos en custodia, semovientes, maquinaria, contratos de venta a futuro, entre otras) permiten bancarizar a sectores productivos sin patrimonio inmobiliario."),
                        Q("ef_garantias_q06",
                            "¿Qué es el seguro de desgravamen asociado a un crédito?",
                            new[]
                            {
                                "Un seguro que cubre el saldo de la deuda en caso de fallecimiento o invalidez total del titular, protegiendo a la familia.",
                                "Un seguro que cubre los daños del vehículo del titular.",
                                "Un impuesto adicional cobrado por el Estado.",
                                "Una comisión por desembolsar el crédito."
                            }, 0,
                            "El desgravamen evita que la deuda se traslade a los herederos ante el fallecimiento del titular; su costo forma parte del costo total del crédito."),
                        Q("ef_garantias_q07",
                            "¿Qué significa que un bien esté 'gravado' como garantía?",
                            new[]
                            {
                                "Que tiene una limitación registrada a favor de la entidad, por lo que no puede venderse libremente hasta liberar la deuda.",
                                "Que el bien pasa inmediatamente a ser propiedad del banco.",
                                "Que el bien queda exento de impuestos municipales.",
                                "Que el bien pierde todo su valor comercial."
                            }, 0,
                            "El gravamen es una anotación legal que limita la disposición del bien mientras exista la obligación; el titular sigue siendo propietario y debe tramitar el levantamiento del gravamen al terminar de pagar."),
                        Q("ef_garantias_q08",
                            "¿Qué debería hacer un cliente inmediatamente después de pagar la última cuota de un crédito hipotecario?",
                            new[]
                            {
                                "Solicitar el certificado de no adeudo y tramitar el levantamiento del gravamen en Derechos Reales.",
                                "Esperar diez años para que el gravamen caduque solo.",
                                "Vender el inmueble para confirmar que ya no tiene deuda.",
                                "No hacer nada: el registro se actualiza automáticamente sin trámite."
                            }, 0,
                            "El gravamen no desaparece solo: hay que gestionar la cancelación registral para que el inmueble quede libre y pueda venderse o volver a ofrecerse en garantía."),
                        Q("ef_garantias_q09",
                            "En un crédito de vivienda de interés social en Bolivia, la garantía habitual es...",
                            new[]
                            {
                                "La hipoteca sobre la misma vivienda que se está adquiriendo o construyendo.",
                                "Una prenda sobre el vehículo del solicitante.",
                                "El aval del municipio donde vive el solicitante.",
                                "El depósito del 100% del valor de la vivienda en una cuenta."
                            }, 0,
                            "En estos créditos la propia vivienda financiada respalda la operación, lo que permite plazos largos y tasas reguladas más accesibles."),
                        Q("ef_garantias_q10",
                            "¿Por qué una entidad financiera realiza un avalúo del bien ofrecido en garantía?",
                            new[]
                            {
                                "Para determinar su valor comercial actual y definir hasta qué monto puede respaldar el crédito.",
                                "Para cobrar un impuesto sobre el bien.",
                                "Para transferir la propiedad del bien a la entidad.",
                                "Para asegurar el bien contra incendios de forma obligatoria."
                            }, 0,
                            "El avalúo establece el valor de referencia; las entidades prestan un porcentaje de ese valor, dejando margen frente a variaciones del mercado."),
                        Q("ef_garantias_q11",
                            "Un grupo de mujeres emprendedoras accede a un crédito mediante 'banca comunal' o garantía mancomunada. ¿Qué implica?",
                            new[]
                            {
                                "Que las integrantes se respaldan mutuamente ante la entidad, sustituyendo la exigencia de garantías reales.",
                                "Que solo una integrante asume toda la deuda del grupo.",
                                "Que el crédito no genera intereses.",
                                "Que la entidad no evalúa la capacidad de pago de ninguna integrante."
                            }, 0,
                            "La garantía solidaria grupal fue clave en el desarrollo de las microfinanzas bolivianas: la confianza y el compromiso mutuo del grupo reemplazan al inmueble como respaldo."),
                        Q("ef_garantias_q12",
                            "¿Qué es un fondo de garantía como los creados en Bolivia para el sector productivo y de vivienda social?",
                            new[]
                            {
                                "Un fondo que cubre parcialmente el riesgo del crédito, permitiendo que personas sin garantía suficiente accedan al financiamiento.",
                                "Un impuesto destinado a obras públicas.",
                                "Una cuenta de ahorro obligatoria para todos los clientes.",
                                "Un subsidio directo que se entrega en efectivo al prestatario."
                            }, 0,
                            "Estos fondos, constituidos con parte de las utilidades de las entidades financieras, respaldan parcialmente el crédito y amplían el acceso a quienes no cuentan con garantías tradicionales."),
                        Q("ef_garantias_q13",
                            "¿Qué ocurre si un crédito con garantía hipotecaria entra en mora prolongada y no se logra un acuerdo de pago?",
                            new[]
                            {
                                "La entidad puede iniciar un proceso judicial de ejecución de la garantía para recuperar lo adeudado.",
                                "La deuda se condona automáticamente después de un año.",
                                "El bien se transfiere de inmediato sin ningún proceso legal.",
                                "El cliente conserva el bien sin ninguna consecuencia."
                            }, 0,
                            "La ejecución de garantías es un proceso legal y de último recurso; por eso conviene renegociar o reprogramar la deuda apenas aparecen dificultades de pago."),
                        Q("ef_garantias_q14",
                            "¿Qué documento es indispensable revisar antes de ofrecer un inmueble como garantía?",
                            new[]
                            {
                                "El folio real de Derechos Reales, para verificar la titularidad y que el bien no tenga otros gravámenes.",
                                "El recibo de luz del mes anterior únicamente.",
                                "La factura de compra de los muebles del inmueble.",
                                "El carnet de identidad de los vecinos."
                            }, 0,
                            "El folio real muestra quién es el propietario registrado y si existen hipotecas o anotaciones preventivas previas que impidan usar el bien como garantía."),
                        Q("ef_garantias_q15",
                            "Para un pequeño productor agrícola sin título de propiedad, ¿qué alternativa de garantía contempla el sistema financiero boliviano?",
                            new[]
                            {
                                "Garantías no convencionales, como maquinaria, semovientes, contratos de venta a futuro o productos almacenados.",
                                "Ninguna: sin inmueble no existe acceso posible al crédito formal.",
                                "Únicamente la garantía de un funcionario público.",
                                "Solo el depósito de la totalidad del monto solicitado."
                            }, 0,
                            "El marco normativo boliviano reconoce garantías no convencionales precisamente para incluir a productores rurales y emprendedores que no cuentan con bienes inmuebles registrados.")
                    }
                }
            }
        };
    }
}
