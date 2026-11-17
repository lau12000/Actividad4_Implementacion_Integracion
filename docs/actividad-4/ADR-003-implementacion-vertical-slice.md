
# ADR-003: Implementación de la solución vertical de asignación de mensajeros

## 1. Información general

- **Proyecto:** Sistema de gestión logística.
- **Actividad:** Actividad 4 - Implementación e integración.
- **Estado:** Aprobado e implementado.
- **Decisión:** Implementar un flujo vertical para la asignación de mensajeros a entregas.
- **Base de datos:** SQLite.
- **Tecnología:** .NET 10, ASP.NET Core y Entity Framework Core 10.

## 2. Contexto

En la actividad anterior se definió una arquitectura de monolito modular, con separación de responsabilidades entre dominio, aplicación, infraestructura y entrada al sistema.

Para esta etapa se necesitaba implementar un flujo funcional que conectara las diferentes capas y permitiera comprobar que las reglas del negocio se cumplen durante una operación real.

Se seleccionó el proceso de asignación de mensajeros porque permite integrar las reglas del dominio, los casos de uso, la persistencia de información y la respuesta de la API.

## 3. Decisión adoptada

Se implementó un flujo vertical de asignación de mensajeros compuesto por las siguientes capas:

1. **Entrada:** ASP.NET Core Web API recibe la solicitud HTTP desde `DeliveriesController`.
2. **Aplicación:** `AssignCourierUseCase` coordina la operación y utiliza los contratos definidos en los puertos.
3. **Dominio:** las entidades `Delivery` y `Courier` aplican las reglas de negocio.
4. **Persistencia:** Entity Framework Core y SQLite almacenan los cambios mediante `SqliteDeliveryRepository`.
5. **Integraciones:** se verifica la disponibilidad del mensajero, la capacidad de la ruta y se registra la notificación de asignación.

## 4. Componentes implementados

| Componente | Responsabilidad |
|---|---|
| DeliveriesController | Recibir solicitudes HTTP de entregas |
| AssignCourierUseCase | Coordinar el caso de uso |
| Delivery | Controlar los estados de una entrega |
| Courier | Representar al mensajero |
| IDeliveryRepository | Contrato para consultar y guardar entregas |
| ICourierAvailability | Consultar disponibilidad |
| IRouteCapacity | Consultar capacidad de ruta |
| IAssignmentNotifier | Notificar la asignación |
| SqliteDeliveryRepository | Persistir entregas en SQLite |
| SqliteCourierAvailability | Consultar mensajeros en SQLite |
| ConfiguredRouteCapacity | Adaptador de capacidad configurada |
| LoggingAssignmentNotifier | Registrar notificaciones mediante logs |

## 5. Reglas de negocio verificadas

- Una entrega nueva comienza en estado `Pending`.
- Solo se puede asignar un mensajero a una entrega pendiente.
- El mensajero debe estar disponible.
- La operación debe comprobar la capacidad de la ruta.
- Una entrega asignada cambia al estado `Assigned`.
- La asignación registra el identificador del mensajero y la fecha correspondiente.
- No se permite asignar nuevamente una entrega que ya fue asignada.

## 6. Persistencia y migraciones

Se utilizó SQLite como mecanismo de persistencia y Entity Framework Core como herramienta de acceso a datos.

Se creó la migración inicial `InitialCreate`, que permite generar las estructuras de la base de datos de manera reproducible.

La aplicación utiliza `Database.MigrateAsync()` para aplicar las migraciones pendientes al iniciar.

La base de datos de esta actividad se identifica como `logistics-activity4.db`.

## 7. Pruebas realizadas

Se implementaron pruebas unitarias y de integración.

Los resultados obtenidos fueron:

- 12 pruebas ejecutadas.
- 12 pruebas correctas.
- 0 pruebas fallidas.
- 0 pruebas omitidas.

También se verificó el funcionamiento de la API mediante solicitudes HTTP.

La asignación correcta devolvió una respuesta exitosa y el intento de asignar nuevamente una entrega produjo un código HTTP 409 (Conflict).

## 8. Consecuencias y compromisos

### Beneficios

- Separación clara de responsabilidades.
- Reglas del negocio centralizadas en el dominio.
- Persistencia desacoplada mediante contratos.
- Posibilidad de probar el caso de uso sin depender de una base de datos externa.
- Integración funcional entre API, aplicación, dominio e infraestructura.
- Migraciones reproducibles.

### Limitaciones identificadas

- La capacidad de las rutas utiliza actualmente un adaptador de configuración fija y no un cálculo real de rutas.
- Las notificaciones se registran mediante logs y no cuentan todavía con un mecanismo de entrega asíncrona.
- No se ha implementado un patrón Outbox para garantizar la entrega de notificaciones cuando ocurre un fallo después de guardar la asignación.
- La solución requiere incorporar autenticación y autorización antes de utilizarse en un entorno productivo.

## 9. Conclusión

La implementación permitió llevar la arquitectura definida en la actividad anterior a un flujo funcional y verificable.

La solución conecta la entrada HTTP con el caso de uso, las reglas del dominio y la persistencia en SQLite, manteniendo la separación de responsabilidades y permitiendo comprobar tanto los escenarios exitosos como los errores de negocio.
