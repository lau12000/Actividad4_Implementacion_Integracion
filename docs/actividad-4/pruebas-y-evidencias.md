
# Pruebas y evidencias - Actividad 4

## 1. Objetivo

Comprobar el funcionamiento de la solución vertical de asignación de mensajeros, verificando las reglas del dominio, la ejecución del caso de uso, la persistencia de información y el manejo de errores.

## 2. Entorno de pruebas

- **Lenguaje:** C#.
- **Plataforma:** .NET 10.
- **Framework de pruebas:** xUnit.
- **Persistencia:** SQLite.
- **Acceso a datos:** Entity Framework Core 10.
- **Proyecto de pruebas unitarias:** `tests/Domain/Logistics.Tests.csproj`.
- **Proyecto de pruebas de integración:** `tests/Integration/Logistics.IntegrationTests.csproj`.

## 3. Pruebas unitarias

Se realizaron pruebas sobre las entidades del dominio y el caso de uso de asignación.

Las verificaciones incluyen:

- Una entrega nueva comienza en estado Pending.
- Una entrega puede recibir un mensajero cuando cumple las condiciones.
- No se permite asignar dos veces una entrega.
- No se puede iniciar el tránsito de una entrega pendiente.
- Una entrega puede completarse cuando se encuentra en tránsito.
- No se permite cancelar una entrega que ya fue entregada.
- Se rechaza la asignación cuando el mensajero no está disponible.
- Se rechaza la asignación cuando no existe capacidad en la ruta.
- Se genera un error cuando la entrega solicitada no existe.
- Se comprueba que el repositorio y el notificador participan en el caso de uso exitoso.

## 4. Pruebas de integración

Se implementaron pruebas utilizando SQLite en memoria para comprobar la interacción entre el caso de uso, los adaptadores de persistencia y las entidades del dominio.

### Prueba 1. Persistencia de una asignación

Se crea una entrega y un mensajero de prueba. Después se ejecuta el caso de uso y se consulta nuevamente la entrega desde SQLite.

Se verifica que:

- La entrega existe.
- El identificador del mensajero quedó almacenado.
- La fecha de asignación fue registrada.
- El estado de la entrega cambió a Assigned.

**Resultado:** Correcto.

### Prueba 2. Rechazo de una segunda asignación

Se crea una entrega y dos mensajeros. Primero se asigna el primer mensajero y posteriormente se intenta asignar el segundo.

Se verifica que:

- El segundo intento genera una excepción InvalidOperationException.
- El mensajero originalmente asignado permanece asociado a la entrega.
- El estado continúa siendo Assigned.

**Resultado:** Correcto.

## 5. Pruebas realizadas sobre la API

Además de las pruebas automatizadas, se realizaron solicitudes HTTP utilizando PowerShell.

### Caso exitoso

Se envió una solicitud POST al endpoint:

`POST /api/deliveries/{id}/assign`

La solicitud incluyó el identificador del mensajero.

La API respondió:

`Mensajero asignado correctamente.`

Posteriormente, mediante una solicitud GET a `/api/deliveries`, se verificó que la entrega quedó en estado Assigned y conservó el identificador del mensajero.

**Resultado:** Correcto.

### Caso de error

Se intentó asignar un segundo mensajero a una entrega que ya estaba asignada.

La API respondió con:

`HTTP 409 - Conflict`

Esto demuestra que la operación rechaza una asignación duplicada.

**Resultado:** Correcto.

## 6. Resultado general de las pruebas automatizadas

Última ejecución registrada mediante:

`dotnet test Logistics.slnx`

| Indicador | Resultado |
|---|---:|
| Pruebas ejecutadas | 12 |
| Pruebas correctas | 12 |
| Pruebas con errores | 0 |
| Pruebas omitidas | 0 |

**Estado general:** Todas las pruebas automatizadas finalizaron correctamente.

## 7. Evidencias de ejecución

Las siguientes capturas se encuentran almacenadas en `docs/actividad-4/evidencias/`:

| Archivo | Evidencia |
|---|---|
| `01-compilacion-exitosa.png` | Compilación correcta de la solución. |
| `02-pruebas-automatizadas.png` | Ejecución de las 12 pruebas automatizadas. |
| `03-consulta-mensajeros.png` | Consulta de mensajeros registrados. |
| `05-asignacion-exitosa.png` | Asignación exitosa de un mensajero. |
| `06-entregas-actualizadas.png` | Consulta de entregas después de la asignación. |
| `07-error-asignacion-duplicada-409.png` | Rechazo de una asignación duplicada mediante HTTP 409. |
| `08-archivos-migracion.png` | Archivos generados por la migración InitialCreate. |
| `09-aplicacion-migracion.png` | Verificación de aplicación de la migración. |
| `10-listado-migraciones.png` | Listado de migraciones registradas. |

**Nota:** No se conserva una captura independiente de la consulta inicial de entregas. El resultado de esa consulta quedó registrado durante la ejecución y se utilizó para realizar la asignación exitosa.

Las evidencias corresponden a ejecuciones realizadas sobre el proyecto local.

Las capturas deben corresponder a las ejecuciones reales del proyecto.

## 8. Conclusión

Las pruebas permitieron verificar el funcionamiento de las reglas del dominio, la coordinación del caso de uso y la persistencia de las asignaciones en SQLite.

También se comprobó el manejo de errores cuando se intenta realizar una operación que no cumple las reglas del negocio.

Los resultados obtenidos respaldan el funcionamiento de la solución vertical implementada para la Actividad 4.
