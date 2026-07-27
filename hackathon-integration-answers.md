### Respuestas a Consultas de Integración - Módulo Trivias (gRPC)

Estimado equipo, a continuación les confirmamos las especificaciones de integración y comportamiento del servicio de Trivias.

> [!IMPORTANT]
> **Cambio de flujo:** el módulo dejó de ser "un banco único de preguntas por departamento". Ahora el recorrido es
> **departamento → categorías (+ desafío diario) → temas → recurso de aprendizaje → trivia del tema**.
> Se agregaron tres métodos (`GetCategories`, `GetTopics`, `GetTopicResource`) y `GetCurrentQuestion` pasa a recibir `topic_id`.

> [!NOTE]
> **Convenio de Comunicación:** Siguiendo el estándar de la plataforma, el canal gRPC siempre responderá con un código de estado `OK` (0). El éxito o los errores de negocio específicos se controlarán a través del campo **`status_code`** y el campo **`message`** en el payload `BaseResponse` devuelto. El campo `errors` existe en el contrato del payload pero actualmente no es utilizado por ningún método — no dependan de él para detectar errores.

---

#### 1. Flujo de pantallas

| # | Pantalla | Método | Entrada principal |
| :-- | :--- | :--- | :--- |
| 1 | Categorías + desafío diario | `GetCategories` | `department_code` |
| 2 | Temas de la categoría *(se omite si se entró por el desafío diario)* | `GetTopics` | `department_code`, `category_code` |
| 3 | Recurso de aprendizaje (uno solo) | `GetTopicResource` | `topic_id` |
| 4 | Pregunta actual de la trivia | `GetCurrentQuestion` | `department_code`, `topic_id` |
| 5 | Envío de respuesta | `SubmitAnswer` | `trivia_session_id`, `question_id`, `selected_option_id` |
| 6 | Recompensas / cierre | `GetRewards` | `trivia_session_id` |

`GetProfile` se mantiene sin cambios de flujo (perfil, cuota semanal y progreso por departamento).

**Puntos clave del flujo**
* El **desafío diario es un tema puntual** (ya no una categoría). Al tocarlo, la app **salta la pantalla 2** y va directo a `GetTopicResource` con el `topic_id` que viene en `daily_challenge`; de ahí en adelante el recorrido es idéntico al flujo normal (`GetCurrentQuestion` → `SubmitAnswer` → `GetRewards`).
* `GetTopicResource` **no crea sesión ni consume intentos**: es solo la pantalla de aprendizaje previa.
* El intento semanal se descuenta **al crear la sesión**, es decir en la primera llamada a `GetCurrentQuestion` con `topic_id` y sin `trivia_session_id`. Reanudar una sesión existente del mismo tema no vuelve a descontar.

---

#### 2. Catálogo de contenido

**4 categorías × 4 temas = 16 temas.** Cada tema tiene un banco de **15 preguntas** del cual la sesión toma **10 al azar**, por lo que repetir un tema no repite el mismo set ni el mismo orden.

| `category_code` | Label | `icon_key` | Temas (`topic_id`) |
| :--- | :--- | :--- | :--- |
| `educacion_financiera` | Educación Financiera | `icon_finance` | `ef_presupuesto`, `ef_ahorro`, `ef_creditos`, `ef_garantias` |
| `medio_ambiente` | Medio Ambiente | `icon_environment` | `ma_reciclaje`, `ma_agua`, `ma_biodiversidad`, `ma_energia` |
| `equidad_genero` | Equidad de Género | `icon_gender` | `eg_trabajo`, `eg_cuidado`, `eg_liderazgo`, `eg_educacion` |
| `derechos_mujer` | Derechos de la Mujer | `icon_women` | `dm_marco_legal`, `dm_violencia`, `dm_salud`, `dm_patrimonio` |

* **Estados de categoría y de tema:** `"not_started"`, `"in_progress"`, `"completed"`.
* **Estados de departamento:** `"locked"`, `"unlocked"`, `"in_progress"`, `"completed"`.
* Los ids de opción se derivan del id de pregunta: `ef_presupuesto_q01_a` … `_d`.

---

#### 3. Método `GetCategories`

* **Entrada:** `department_code`. Si se envía vacío, se asume la sucursal de apertura del usuario.
* **Salida:** `department_code`, `department_name`, `department_status`, la lista de `categories` y el objeto `daily_challenge`.
* Cada `CategoryPb` incluye `total_topics`, `completed_topics` y `status`, calculados **para ese departamento**: el mismo tema puede estar completado en La Paz y sin empezar en Santa Cruz.
* `daily_challenge` trae `date` (`dd/MM/yyyy`), **`topic_id`**, `topic_title`, `category_code`, `category_label`, `icon_key`, `title`, `description`, `total_questions`, `status`, `best_score`, `times_completed` y `resource_type`. Con eso la card del reto se pinta completa (incluido el progreso del usuario en ese tema **dentro del departamento consultado**) y el tap navega directo a `GetTopicResource(topic_id)`.
* ⚠️ **`daily_challenge.status` se refiere solo a HOY**, a diferencia de `TopicPb.status`, que es el histórico del tema:
  * `"completed"` → el usuario reclamó recompensas de este tema **hoy** (hora Bolivia). Es el valor con el que se pinta "ya cumpliste el reto de hoy".
  * `"in_progress"` → tiene una sesión abierta de este tema (incluye el caso de haber respondido las 10 y no haber llamado aún a `GetRewards`).
  * `"not_started"` → todo lo demás. **Importante:** si el usuario completó este mismo tema en otra fecha, hoy vuelve a llegar `"not_started"`, porque el reto de hoy sigue pendiente.
* En cambio `best_score` y `times_completed` **sí son históricos** del tema (sirven para "tu mejor puntaje aquí fue 8/10"). El reto diario se puede rejugar: consume un intento semanal como cualquier otra sesión.
* El tema destacado **rota de forma determinística según la fecha** (hora Bolivia, UTC-4) y es el mismo para todos los usuarios ese día. No requiere persistencia: si la app lo calcula por su cuenta, coincidirá con el backend. La categoría cambia cada día, de modo que dos días seguidos nunca caen en la misma; el ciclo completo recorre los 16 temas.
* El desafío diario **no otorga recompensas ni intentos aparte**: es un atajo al mismo tema, con las mismas reglas de cuota semanal.
* Si el departamento está bloqueado se retorna `ERR007`; si no existe, `ERR013`.

---

#### 4. Método `GetTopics`

* **Entrada:** `department_code` (opcional) y `category_code` (requerido).
* **Salida:** datos de la categoría + `topics`.
* Cada `TopicPb` incluye: `id`, `title`, `description`, `icon_key`, `total_questions` (siempre 10), `status`, `best_score`, `times_completed`, `resource_type` (`"pdf"` | `"image"` | `"video"`) — para que la card del tema muestre el ícono del recurso sin llamar a `GetTopicResource` — y `is_daily_challenge`, para pintar el badge de "desafío diario" en el tema que hoy es el reto.
* La bandera `is_daily_challenge` va en **cada `TopicPb`**, no a nivel de la respuesta: el reto diario es un tema, no una categoría entera.
* `category_code` inválido → `ERR016`.

---

#### 5. Método `GetTopicResource`

* **Entrada:** `topic_id`. **No requiere `department_code`** y no genera sesión ni consume intentos.
* **Salida:** `topic_id`, `topic_title`, `category_code`, `category_label`, `total_questions`, `is_daily_challenge` y **un único** `resource`. Como el reto diario entra directo a esta pantalla, `is_daily_challenge` permite mostrar el badge sin haber pasado por `GetTopics`.
* `TopicResourcePb` tiene exactamente **4 campos**: `type` (`"pdf"` | `"image"` | `"video"`), `title`, `description` y `url`. No hay thumbnail, duración ni número de páginas.
* Todos los recursos cargados hoy en el mock son de tipo `"image"`.
* Las URLs del mock apuntan a `https://cdn.yasta.bo/trivias/recursos/...` y son **ficticias**: el dominio no existe y no sirve contenido. Sirven para validar el contrato y el parseo, **no para renderizar** la imagen.
* `topic_id` inválido → `ERR017`. Tema sin recurso configurado → `ERR019`.

---

#### 6. Método `GetCurrentQuestion`

* **Entrada:** `department_code`, `topic_id` y `trivia_session_id` (opcional).
  * Para **iniciar** una trivia: enviar `topic_id` sin `trivia_session_id`. Si falta `topic_id` → `ERR020`.
  * Para **continuar**: enviar `trivia_session_id`. `topic_id` es opcional; si se envía y no coincide con el de la sesión → `ERR018`.
* **Salida:** además de la pregunta, se devuelven `topic_id`, `topic_title`, `category_code` y `category_label` para poder renderizar el encabezado de la trivia sin guardar estado local.
* `TriviaQuestionPb` mantiene `category`, `category_label` y `category_icon_key`, y suma `topic_id` y `topic_title`.
* `total_questions` es siempre **10** (el tamaño de la sesión), no el tamaño del banco del tema.
* La sesión activa se resuelve por **usuario + departamento + tema**: se pueden tener sesiones abiertas en temas distintos en paralelo.

---

#### 7. Método `SubmitAnswer`

* Sin cambios en la entrada. En la salida se agregaron, para evitar que la app lleve el conteo por su cuenta: `current_question_number`, `total_questions` y `correct_answers_so_far`.
* **XP:** `xp_earned` devuelve `50` (respuesta correcta) o `0` (incorrecta).

---

#### 8. Método `GetRewards`

* **Cálculo de XP y monedas** (sin cambios respecto de la versión anterior):
  * Cada respuesta correcta otorga `50` XP.
  * Completar las 10 preguntas otorga un bono fijo de `100` XP.
  * `rewards_earned.xp_bonus` = `(respuestas correctas * 50) + 100`; `rewards_earned.yasta_coins` = `respuestas correctas * 10`.
* **Campos agregados:** `topic_id`, `topic_title`, `category_code`, `category_label`, `topic_status` (queda en `"completed"`), `best_score` (mejor puntaje histórico del usuario en ese tema y departamento), `completed_topics_in_department` y `total_topics_in_department`.
* Se mantienen `weekly_attempts_left`, `department_status` y `unlocked_department_codes`.

**Regla de completado y desbloqueo de departamentos**

* Un **tema** queda `"completed"` al reclamar recompensas de una sesión de ese tema. Volver a jugarlo es posible: consume un intento y solo actualiza `best_score` y `times_completed`.
* Un **departamento** pasa a `"completed"` cuando el usuario completó **al menos un tema de cada una de las 4 categorías** en ese departamento (4 sesiones). Se eligió esta regla porque exigir los 16 temas haría inalcanzable el desbloqueo con la cuota de 3 intentos semanales.
* Al completar la **sucursal de apertura**, se desbloquean los demás departamentos y sus códigos llegan en `unlocked_department_codes` (solo los que se desbloquearon en esa llamada).
* En `DepartmentProgressPb` se agregaron `completed_topics` y `total_topics` (16). `completed_questions` / `total_questions` ahora se expresan en preguntas de sesión: `completed_topics * 10` sobre `16 * 10 = 160`.

> [!WARNING]
> Con 3 intentos semanales, completar un departamento requiere 4 sesiones, es decir **dos semanas de cuota**. Si necesitan validar el desbloqueo en una sola corrida de pruebas, avísennos y subimos `WeeklyAttemptsMax` en el mock (`TriviaMockDatabase.WeeklyAttemptsMax`).

---

#### 9. Catálogo de Códigos de Error (`status_code` en BaseResponse)

Si una petición falla por lógica de negocio, se retornará un `status_code` diferente a `"SUC000"` con su respectivo mensaje sugerido para mostrar al usuario. Cuando la operación es exitosa (`"SUC000"`), la respuesta **no incluye `message`**, solo el objeto `data` correspondiente.

| `status_code` | Descripción de Negocio / Escenario | Mensaje Sugerido para la UI |
| :--- | :--- | :--- |
| **`SUC000`** | Operación exitosa | *(sin mensaje — solo se retorna `data`)* |
| **`ERR001`** | La sesión de trivia ya ha finalizado | "La sesión ya ha finalizado. Por favor reclame sus recompensas." |
| **`ERR002`** | Sesión de trivia no encontrada | "La sesión de trivia no fue encontrada." |
| **`ERR003`** | `question_id` incorrecto o fuera de orden | "La pregunta enviada no corresponde al orden actual de tu sesión de trivia." |
| **`ERR004`** | La sesión de trivia aún no ha sido completada | "La sesión de trivia aún no ha sido completada. Debes responder todas las preguntas antes de reclamar tus recompensas." |
| **`ERR005`** | Sesión de trivia expirada por inactividad (30 min) | "La sesión de trivia ha expirado por inactividad." |
| **`ERR006`** | Sin intentos semanales disponibles (`weekly_attempts_left = 0`) | "Has agotado tus intentos semanales permitidos para jugar trivias." |
| **`ERR007`** | Departamento seleccionado está bloqueado | "El departamento seleccionado está bloqueado. Debes completar primero tu departamento de apertura." |
| **`ERR008`** | `selected_option_id` inválido para la pregunta | "La opción seleccionada es inválida o no corresponde a la pregunta." |
| **`ERR009`** | No autenticado: token no proporcionado | "No autenticado. Token de autenticación no proporcionado." |
| **`ERR010`** | El `id_user_profile` del token no corresponde a ningún usuario registrado | "No se encontró un usuario registrado con el id_user_profile del token." |
| **`ERR011`** | El claim `id_user_profile` no se encuentra en el token | "No autenticado. El claim 'id_user_profile' no se encuentra en el token." |
| **`ERR012`** | No se pudo recuperar la pregunta actual de la sesión | "No se pudo recuperar la pregunta de la sesión de trivia." |
| **`ERR013`** | `department_code` inválido o inexistente | "El código de departamento no es válido." |
| **`ERR014`** | El `department_code` no corresponde al de la sesión especificada | "El department_code no corresponde al departamento de la sesión de trivia especificada." |
| **`ERR015`** | La sesión de trivia no pertenece al usuario autenticado | "La sesión de trivia especificada no pertenece al usuario autenticado." |
| **`ERR016`** | `category_code` inválido o inexistente | "La categoría no es válida." |
| **`ERR017`** | `topic_id` inválido o inexistente | "El tema no es válido." |
| **`ERR018`** | El `topic_id` no corresponde al tema de la sesión especificada | "El topic_id no corresponde al tema de la sesión de trivia especificada." |
| **`ERR019`** | El tema no tiene recurso de aprendizaje configurado | "No se encontró un recurso de aprendizaje para el tema." |
| **`ERR020`** | Falta `topic_id` al iniciar una nueva sesión | "Debe especificar un topic_id para iniciar una nueva sesión de trivia." |

---

#### 10. Header de Autenticación y Token Vencido

* **Header de Autenticación:** Se lee mediante metadatos de gRPC bajo la clave: `Authorization: Bearer <jwt>`.
* **Claims simulados del JWT:** El decodificador lee del payload los campos `"id_user_profile"` (identificador único) y `"document_number"`.
* **Usuarios registrados en el mock:** `id_user_profile` = `"1"` (John) y `"2"` (Fiora). Cualquier otro valor retorna `ERR010`.
* **Alcance de la validación:** El servicio únicamente valida (a) que el token esté presente (`ERR009`) y (b) que el claim `id_user_profile` exista y sea legible (`ERR011`). **No valida** expiración, firma ni formato exacto del JWT — esas verificaciones son responsabilidad del gateway.

---

#### 11. Método `GetProfile`

* **Lista Oficial de Departamentos (Código y Nombre):**

| Departamento | Código |
| :--- | :--- |
| Beni | BE |
| Chuquisaca | CH |
| Cochabamba | CB |
| La Paz | LP |
| Oruro | OR |
| Pando | PD |
| Potosí | PT |
| Santa Cruz | SC |
| Tarija | TJ |

* **Formato de `next_reset_date`:** `dd/MM/yyyy` (ej. `13/07/2026`), sin hora ni offset. La cuota se repone los lunes 00:00 hora Bolivia.
* **Abandono de una trivia:** el intento ya fue descontado al iniciar. La sesión abandonada expira a los 30 minutos de inactividad; el tema queda en `"in_progress"` y volver a jugarlo consumirá un nuevo intento.
