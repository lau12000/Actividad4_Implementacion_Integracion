# Invariantes y reglas del dominio

## 1. Objetivo

Definir las reglas del negocio que deben cumplirse durante el proceso de asignación de repartidores a las entregas de la Plataforma de Gestión Logística de Última Milla.

Estas reglas permiten proteger la integridad de la información y evitar situaciones incorrectas durante la operación.

## 2. Invariantes del dominio

### INV-01. Una entrega no puede tener dos repartidores activos

Una entrega solo puede tener un repartidor activo asignado en un mismo momento.

**Justificación:** Evita duplicidad de asignaciones y conflictos durante el proceso de entrega.

### INV-02. No se puede asignar un repartidor a una entrega inexistente

El sistema debe verificar que la entrega exista antes de realizar cualquier asignación.

**Justificación:** Evita registrar asignaciones sin una entrega válida.

### INV-03. El repartidor debe estar disponible

Una asignación solo puede realizarse si el repartidor seleccionado cumple las condiciones de disponibilidad definidas por el negocio.

**Justificación:** Evita asignar entregas a repartidores que no pueden atenderlas.

### INV-04. La entrega debe encontrarse en un estado permitido

Solo se pueden asignar repartidores a entregas cuyo estado permita iniciar el proceso de asignación.

Para el alcance inicial, el estado permitido es `Pending`.

**Justificación:** Evita asignaciones sobre entregas que ya están en tránsito, completadas, fallidas o canceladas.

### INV-05. La asignación debe tener identificadores válidos

Toda asignación debe relacionar una entrega existente con un repartidor identificado.

**Justificación:** Mantiene la integridad de las relaciones entre las entidades del dominio.

### INV-06. Una entrega finalizada no puede recibir una nueva asignación

Una entrega que se encuentre en estado `Delivered` no puede recibir otra asignación.

**Justificación:** Evita modificar una operación que ya fue completada.

### INV-07. Una entrega cancelada no puede recibir una asignación

Una entrega que se encuentre en estado `Cancelled` no puede ser asignada a un repartidor.

**Justificación:** Evita ejecutar operaciones sobre entregas que ya fueron canceladas.

### INV-08. La asignación debe respetar la capacidad de la ruta

No se debe asignar una entrega cuando la ruta correspondiente haya alcanzado la capacidad definida por el negocio.

**Justificación:** Evita sobrecargar las rutas y afectar el cumplimiento de las entregas.

La capacidad máxima y los criterios para determinar la saturación deben ser definidos por los responsables del negocio.

## 3. Reglas de negocio complementarias

Además de las invariantes anteriores, se establecen las siguientes reglas:

- La asignación debe ser solicitada por un operador logístico autorizado.
- La disponibilidad del repartidor debe consultarse mediante un contrato de aplicación.
- La entidad Delivery debe proteger las reglas que afectan directamente su estado y asignación.
- La aplicación debe coordinar el proceso sin depender de implementaciones concretas de infraestructura.
- Una asignación solo debe confirmarse cuando las validaciones y el almacenamiento hayan finalizado correctamente.
- Los errores deben comunicarse sin presentar una asignación fallida como exitosa.

## 4. Responsabilidad de las capas

| Regla o validación | Capa responsable |
|---|---|
| Evitar dos repartidores activos en una entrega | Dominio |
| Verificar que la entrega permita asignación | Dominio |
| Controlar las transiciones de estado | Dominio |
| Consultar la existencia de la entrega | Aplicación mediante un puerto |
| Consultar disponibilidad del repartidor | Aplicación mediante un puerto |
| Coordinar la asignación | Aplicación |
| Consultar y guardar información persistente | Infraestructura mediante un adaptador |
| Consultar servicios externos | Infraestructura mediante adaptadores |

## 5. Comportamiento ante incumplimientos

Cuando una regla no se cumple, el sistema debe rechazar la operación y comunicar el motivo.

| Situación | Resultado esperado |
|---|---|
| La entrega no existe | Rechazar la asignación. |
| La entrega ya tiene un repartidor activo | Rechazar la nueva asignación. |
| El repartidor no está disponible | Rechazar la asignación. |
| La entrega está completada | Rechazar la asignación. |
| La entrega está cancelada | Rechazar la asignación. |
| La ruta alcanzó su capacidad | Rechazar la asignación. |

En todos estos casos, el sistema debe evitar confirmar cambios que no cumplan las reglas del negocio.

## 6. Relación con la entidad Delivery

La entidad `Delivery` es responsable de proteger las reglas que corresponden directamente a una entrega.

El método `AssignCourier(courierId)` debe verificar que no exista una asignación activa y que el estado actual permita realizar la operación.

Las validaciones que requieren consultar información externa, como la disponibilidad del repartidor o la capacidad de la ruta, deben coordinarse desde el caso de uso mediante puertos.

De esta manera, el dominio conserva sus reglas sin depender de una base de datos o de una API externa.

## 7. Conclusión

Las invariantes permiten proteger la lógica principal del negocio y garantizar que las asignaciones se realicen bajo condiciones válidas.

La separación de responsabilidades entre dominio, aplicación e infraestructura permite mantener las reglas independientes de las tecnologías utilizadas para almacenar o consultar información.