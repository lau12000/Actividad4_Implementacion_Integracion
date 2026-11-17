# Flujo de negocio: Asignación de repartidor a una entrega

## 1. Descripción general

El flujo de asignación de repartidor tiene como propósito permitir que un operador logístico asigne un repartidor disponible a una entrega pendiente.

Este proceso forma parte de la Plataforma de Gestión Logística de Última Milla y busca organizar la distribución de las entregas, evitar asignaciones duplicadas y mantener actualizado el estado de cada operación.

## 2. Objetivo del flujo

Asignar un repartidor a una entrega que se encuentre disponible para despacho, verificando previamente que la entrega exista, que no tenga otro repartidor asignado y que el repartidor seleccionado tenga disponibilidad.

## 3. Actores involucrados

| Actor | Responsabilidad |
|---|---|
| Operador logístico | Inicia la solicitud de asignación de repartidor. |
| Plataforma logística | Coordina las validaciones y ejecuta el caso de uso. |
| Repartidor | Persona encargada de realizar la entrega. |
| Servicio de disponibilidad | Proporciona información sobre la disponibilidad del repartidor. |
| Repositorio de entregas | Permite consultar y guardar la información de las entregas. |

## 4. Condiciones iniciales

Para ejecutar el flujo deben cumplirse las siguientes condiciones:

- La entrega debe estar registrada en la plataforma.
- La entrega debe encontrarse en un estado que permita asignar un repartidor.
- La entrega no debe tener un repartidor activo asignado.
- El repartidor debe existir y estar disponible.
- El operador debe contar con permisos para realizar la asignación.

## 5. Desarrollo del flujo principal

1. El operador logístico selecciona una entrega pendiente.
2. La plataforma recibe el identificador de la entrega y del repartidor.
3. El caso de uso `AssignCourierUseCase` solicita la información de la entrega al puerto `IDeliveryRepository`.
4. El sistema verifica que la entrega exista.
5. El sistema comprueba que la entrega no tenga un repartidor activo asignado.
6. El caso de uso consulta la disponibilidad del repartidor mediante el puerto `ICourierAvailability`.
7. Si el repartidor está disponible, se ejecuta el método `AssignCourier(courierId)` de la entidad `Delivery`.
8. La entidad verifica las reglas del negocio y realiza la asignación.
9. El caso de uso solicita guardar la entrega actualizada mediante `IDeliveryRepository`.
10. Finalmente, se registra o publica el resultado de la operación mediante el puerto correspondiente.

## 6. Flujos alternativos y excepciones

### Entrega inexistente

Si no se encuentra la entrega solicitada, el sistema informa que no existe y detiene el proceso.

### Entrega ya asignada

Si la entrega ya tiene un repartidor activo, se rechaza la nueva asignación para evitar duplicidad.

### Repartidor no disponible

Si el repartidor no está disponible, el sistema rechaza la solicitud y conserva la información original de la entrega.

### Error al guardar

Si ocurre un problema durante el almacenamiento, la operación debe informar el error y no presentar la asignación como confirmada.

## 7. Resultado esperado

Cuando el proceso termina correctamente:

- La entrega queda asociada al repartidor seleccionado.
- La información de la entrega queda actualizada.
- Se registra el resultado de la asignación.
- El operador recibe una confirmación.

Si alguna validación falla, la entrega conserva su asignación anterior y se informa el motivo del rechazo.

## 8. Reglas relacionadas

- Una entrega no puede tener dos repartidores activos asignados simultáneamente.
- Solo se pueden asignar entregas que estén en un estado permitido.
- El repartidor debe estar disponible antes de realizar la asignación.
- Las reglas de asignación deben protegerse dentro del dominio.
- La aplicación no debe depender directamente de una base de datos o de una API externa.

## 9. Alcance de esta actividad

En esta actividad se documenta y diseña el flujo de asignación de repartidor, identificando sus actores, condiciones, reglas y responsabilidades.

La implementación completa del caso de uso, la persistencia y las integraciones reales se desarrollarán en la Actividad 4.