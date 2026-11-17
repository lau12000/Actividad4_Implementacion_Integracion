
# Riesgos, seguridad y observabilidad - Actividad 4

## 1. Objetivo

Identificar los principales riesgos técnicos y operativos de la solución implementada, establecer medidas de mitigación y definir mecanismos de observabilidad que permitan conocer el comportamiento de la aplicación.

## 2. Riesgos identificados

| Riesgo | Impacto | Mitigación |
|---|---|---|
| Asignación duplicada de una entrega | Puede generar inconsistencias en la operación logística | Validar el estado de la entrega desde el dominio y rechazar operaciones no permitidas |
| Mensajero no disponible | La asignación no podría realizarse correctamente | Consultar la disponibilidad antes de ejecutar la asignación |
| Falta de capacidad en la ruta | Se podría asignar una entrega que no pueda ser atendida | Verificar la capacidad mediante el contrato IRouteCapacity |
| Error de persistencia | La asignación podría no quedar almacenada | Utilizar Entity Framework Core y pruebas de integración con SQLite |
| Fallo al registrar una notificación | La información puede quedar guardada sin que se complete la notificación | Registrar el evento en logs y considerar un patrón Outbox para una futura implementación |
| Acceso no autorizado | Un usuario podría ejecutar operaciones sin permisos | Incorporar autenticación y autorización antes de un despliegue productivo |
| Pérdida o corrupción de la base de datos | Se podría perder información de las entregas | Implementar copias de seguridad y procedimientos de recuperación |

## 3. Medidas de seguridad

Durante la implementación se tuvieron en cuenta las siguientes medidas:

- Validación de los identificadores recibidos por la API.
- Aplicación de reglas de negocio en las entidades del dominio.
- Uso de contratos para separar las responsabilidades de cada capa.
- Manejo de excepciones para responder con códigos HTTP apropiados.
- Uso de Entity Framework Core para realizar operaciones de persistencia.
- Registro de información operativa sin almacenar datos sensibles innecesarios.

### Aspectos pendientes

La versión implementada todavía requiere:

- Autenticación de usuarios.
- Autorización por roles o permisos.
- Configuración de políticas de seguridad para producción.
- Protección y administración segura de la cadena de conexión.
- Estrategia de copias de seguridad.

Estas medidas deben implementarse antes de utilizar el sistema en un entorno productivo.

## 4. Observabilidad

La aplicación utiliza el sistema de logging de ASP.NET Core mediante ILogger.

El adaptador LoggingAssignmentNotifier registra información relacionada con la asignación de entregas.

Los principales eventos que se pueden observar son:

| Evento | Información observada |
|---|---|
| Inicio de la aplicación | Inicio y configuración del servicio |
| Consulta de entregas | Solicitudes recibidas por la API |
| Consulta de mensajeros | Solicitudes de consulta |
| Asignación exitosa | Identificador de entrega, mensajero y fecha de asignación |
| Error de asignación | Respuesta HTTP correspondiente al error |
| Error de persistencia | Excepciones registradas durante las operaciones |

## 5. Indicadores de verificación

Para comprobar el funcionamiento de la solución se utilizaron los siguientes indicadores:

- Estado de respuesta HTTP.
- Cantidad de asignaciones exitosas.
- Cantidad de asignaciones rechazadas.
- Estado actual de las entregas.
- Disponibilidad de los mensajeros.
- Resultado de las pruebas automatizadas.
- Estado de la compilación.

## 6. Manejo de errores

La API contempla respuestas específicas para los principales escenarios:

| Situación | Respuesta |
|---|---|
| Asignación exitosa | HTTP 200 |
| Entrega inexistente | HTTP 404 |
| Operación no permitida por reglas del negocio | HTTP 409 |
| Datos de entrada inválidos | HTTP 400 |

En las pruebas realizadas se comprobó especialmente el rechazo de una asignación duplicada mediante HTTP 409.

## 7. Limitaciones operativas

Se identificaron los siguientes aspectos que requieren evolución:

- El adaptador de capacidad de ruta utiliza una configuración fija y no consulta un sistema real de planificación de rutas.
- Las notificaciones se registran mediante logs, sin una cola de mensajes.
- No se ha implementado reintento automático ni una cola de mensajes fallidos (Dead Letter Queue), debido a que la integración actual es local y síncrona.
- No se ha implementado monitoreo centralizado ni alertas automáticas.
- No se ha implementado un patrón Outbox para coordinar de forma confiable la persistencia y las notificaciones.

## 8. Conclusión

La implementación cuenta con validaciones de negocio, manejo de errores y registros de eventos que permiten verificar el funcionamiento del flujo de asignación.

Se identificaron riesgos y medidas de mitigación para mejorar la seguridad, disponibilidad y confiabilidad de la solución en futuras versiones.
