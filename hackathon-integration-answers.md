### Respuestas a Consultas de Integración - Módulo Trivias (gRPC)

Estimado equipo, a continuación les confirmamos las especificaciones de integración y comportamiento del servicio de Trivias.

> [!NOTE]
> **Convenio de Comunicación:** Siguiendo el estándar de la plataforma, el canal gRPC siempre responderá con un código de estado `OK` (0). El éxito o los errores de negocio específicos se controlarán a través del campo **`status_code`** y el campo **`message`** en el payload `BaseResponse` devuelto. El campo `errors` existe en el contrato del payload pero actualmente no es utilizado por ningún método — no dependan de él para detectar errores.

---

#### 1. Catálogo de Códigos de Error (`status_code` en BaseResponse)
Si una petición falla por lógica de negocio, se retornará un `status_code` diferente a `"SUC000"` con su respectivo mensaje sugerido para mostrar al usuario. Cuando la operación es exitosa (`"SUC000"`), la respuesta **no incluye `message`**, solo el objeto `data` correspondiente.

| `status_code` | Descripción de Negocio / Escenario | Mensaje Sugerido para la UI |
| :--- | :--- | :--- |
| **`SUC000`** | Operación exitosa | *(sin mensaje — solo se retorna `data`)* |
| **`ERR001`** | La sesión de trivia ya ha finalizado | "La sesión ya ha finalizado. Por favor reclame sus recompensas." |
| **`ERR002`** | Sesión de trivia no encontrada | "La sesión de trivia no fue encontrada." |
| **`ERR003`** | `question_id` incorrecto o fuera de orden | "La pregunta enviada no corresponde al orden actual de tu sesión de trivia." |
| **`ERR004`** | La sesión de trivia aún no ha sido completada | "La sesión de trivia aún no ha sido completada. Se completaron {X} de {Y} preguntas." |
| **`ERR005`** | Sesión de trivia expirada por inactividad | "La sesión de trivia ha expirado por inactividad." |
| **`ERR006`** | Sin intentos semanales disponibles (`weekly_attempts_left = 0`) | "Has agotado tus intentos semanales permitidos para jugar trivias." |
| **`ERR007`** | Departamento seleccionado está bloqueado | "El departamento seleccionado está bloqueado. Debes completar primero tu departamento de apertura." |
| **`ERR008`** | `selected_option_id` inválido para la pregunta | "La opción seleccionada es inválida o no corresponde a la pregunta." |
| **`ERR009`** | No autenticado: token de autenticación no proporcionado en la petición | "No autenticado. Token de autenticación no proporcionado." |
| **`ERR010`** | El `id_user_profile` del token no corresponde a ningún usuario registrado | "No se encontró un usuario registrado con id_user_profile '{X}'." |
| **`ERR011`** | El claim `id_user_profile` no se encuentra en el token | "No autenticado. El claim 'id_user_profile' no se encuentra en el token." |
| **`ERR012`** | No se pudo recuperar la pregunta actual de la sesión (dato interno inconsistente) | "No se pudo recuperar la pregunta de la sesión de trivia." |
| **`ERR013`** | `department_code` inválido o inexistente | "El código de departamento '{X}' no es válido." |
| **`ERR014`** | El department_code enviado no corresponde al de la sesión activa especificada | "El department_code '{X}' no corresponde al departamento de la sesión de trivia especificada ('{Y}')." |
| **`ERR015`** | La sesión de trivia no pertenece al usuario autenticado | "La sesión de trivia especificada no pertenece al usuario autenticado." |

---

#### 2. Header de Autenticación y Token Vencido
* **Header de Autenticación:** Se lee mediante metadatos de gRPC bajo la clave: `Authorization: Bearer <jwt>`.
* **Claims simulados del JWT:** Por el momento, el decodificador lee del payload del JWT los campos `"id_user_profile"` (identificador único) y `"document_number"` (ej. `"1234567-LP"`).
* **Alcance de la validación:** El servicio de Trivias únicamente valida (a) que el token esté presente (`ERR009` si falta) y (b) que el claim `id_user_profile` exista y sea legible dentro del payload (`ERR011` si falta o el token no se puede decodificar). **No valida** expiración, firma ni formato exacto del JWT — esas verificaciones son responsabilidad del gateway, que se ejecuta antes de llegar a este servicio. Por lo tanto, el equipo móvil no debe esperar un `status_code` de este servicio para tokens expirados o mal formados; ese caso lo bloqueará el gateway antes de la petición.

---

#### 3. Método `GetProfile`
* **Estados Oficiales del Departamento:** Se retornará estrictamente uno de los siguientes textos: `"locked"`, `"unlocked"`, `"in_progress"` o `"completed"`.
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

* **Formato de `next_reset_date`:** Se devolverá la fecha en formato `dd/MM/yyyy` (ej. `13/07/2026`), sin hora ni offset.

---

#### 4. Método `GetCurrentQuestion`
* **Códigos de Categorías:** Se manejan 4 categorías mezcladas dinámicamente en el bloque de 10 preguntas:
  1. `educacion_financiera` (Educación Financiera)
  2. `medio_ambiente` (Medio Ambiente)
  3. `equidad_genero` (Equidad de Género)
  4. `derechos_mujer` (Derechos de la Mujer)
* **Atributos de Pantalla:** Se incluyeron los campos `category_label` (ej: `"Derechos de la Mujer"`) y `category_icon_key` (ej: `"icon_women"`) dentro del objeto `TriviaQuestionPb` para facilitar el renderizado de iconos en el frontend.
* **Continuación de Trivia:** Sí, la estructura `GetCurrentQuestionRequestPb` ahora acepta el parámetro `"trivia_session_id"`. Si lo envían, el servicio les devolverá la siguiente pregunta correspondiente de esa misma sesión.

---

#### 5. Método `SubmitAnswer`
* **XP Obtenida:** Se agregó el campo `xp_earned` en el objeto de datos de la respuesta. Devolverá `50` (si la respuesta fue correcta) o `0` (si fue incorrecta).

---

#### 6. Método `GetRewards`
* **Respuestas Simplificadas:** Para evitar llamadas redundantes a `GetProfile`, el payload de retorno `GetRewardsResponsePb` ahora incluye:
  * `weekly_attempts_left` (intentos restantes del usuario).
  * `department_status` (que pasará a ser `"completed"`).
  * `unlocked_department_codes` (lista repetida de departamentos que se acaban de desbloquear, ej: si completaron su sucursal de apertura).
* **Cálculo de XP:**
  * Cada respuesta correcta otorga `50` XP.
  * Completar las 10 preguntas otorga un bono fijo de `100` XP.
  * El total devuelto en `RewardsEarnedPb/xp_bonus` es: `(Respuestas correctas * 50) + 100`.
* **Consumo del Intento:** El intento se descuenta inmediatamente al iniciar la trivia (primera llamada a `GetCurrentQuestion` sin `trivia_session_id`). Si el usuario vuelve a consultar y tiene `weekly_attempts_left = 0`, el servicio retornará `status_code = "ERR006"`.
* **Abandono:** Si el usuario abandona a media trivia, el intento ya fue descontado (se consume al inicio). Como la sesión nunca llega a completarse llamando a `GetRewards`, el departamento volverá a estar disponible con estado `"unlocked"` (o `"in_progress"` si intentaran retomar), pero requerirá consumir un nuevo intento disponible si desean iniciar de cero.
