# Escenarios de aceptación – Asignación de repartidores

## 1. Objetivo

Definir los escenarios de aceptación del proceso de asignación de repartidores mediante el formato Dado-Cuando-Entonces.

Estos escenarios permiten describir el comportamiento esperado de la plataforma cuando se cumplen las reglas del negocio y cuando se presentan situaciones que impiden realizar una asignación.

## 2. Escenarios de aceptación

### EA-01. Asignación exitosa de un repartidor

**Dado:** que existe una entrega en estado Pending, no tiene un repartidor activo asignado y el repartidor seleccionado está disponible.

**Cuando:** el operador logístico solicita asignar ese repartidor a la entrega.

**Entonces:** el sistema debe validar las condiciones, asociar el repartidor a la entrega, actualizar su estado a Assigned y guardar el resultado de la operación.

### EA-02. Entrega inexistente

**Dado:** que el identificador de entrega no corresponde a ninguna entrega registrada.

**Cuando:** el operador solicita realizar la asignación.

**Entonces:** el sistema debe rechazar la operación e informar que la entrega no existe.

### EA-03. Entrega que ya tiene un repartidor

**Dado:** que existe una entrega que ya tiene un repartidor activo asignado.

**Cuando:** el operador intenta asignar un segundo repartidor.

**Entonces:** el sistema debe rechazar la solicitud y conservar la asignación existente.

### EA-04. Repartidor no disponible

**Dado:** que existe una entrega pendiente y el repartidor seleccionado no está disponible.

**Cuando:** el operador solicita asignarlo a la entrega.

**Entonces:** el sistema debe rechazar la operación y mantener la información original de la entrega.

### EA-05. Entrega completada

**Dado:** que existe una entrega en estado Delivered.

**Cuando:** el operador intenta asignarle otro repartidor.

**Entonces:** el sistema debe rechazar la operación porque la entrega ya fue completada.

### EA-06. Entrega cancelada

**Dado:** que existe una entrega en estado Cancelled.

**Cuando:** el operador intenta asignar un repartidor.

**Entonces:** el sistema debe rechazar la solicitud y conservar el estado de cancelación.

### EA-07. Ruta sin capacidad disponible

**Dado:** que la ruta asociada al proceso de asignación ha alcanzado su capacidad máxima definida.

**Cuando:** el operador solicita asignar un repartidor a una entrega que depende de esa ruta.

**Entonces:** el sistema debe rechazar la operación e informar que la ruta no tiene capacidad disponible.

### EA-08. Error durante el almacenamiento

**Dado:** que la entrega y el repartidor cumplen las condiciones del negocio.

**Cuando:** ocurre un error al guardar la asignación.

**Entonces:** el sistema debe informar que la operación no pudo completarse y no debe comunicar una asignación como confirmada.

## 3. Relación entre escenarios e invariantes

| Escenario | Invariante relacionada | Resultado esperado |
|---|---|---|
| EA-01 | INV-01, INV-03, INV-04 | Asignación exitosa. |
| EA-02 | INV-02 | Operación rechazada. |
| EA-03 | INV-01 | Se conserva la asignación existente. |
| EA-04 | INV-03 | Operación rechazada. |
| EA-05 | INV-06 | No se modifica la entrega completada. |
| EA-06 | INV-07 | No se modifica la entrega cancelada. |
| EA-07 | INV-08 | Operación rechazada. |
| EA-08 | Reglas de confirmación de asignación | No se confirma una operación fallida. |

## 4. Criterios generales de aceptación

Para considerar correcto el proceso de asignación se deben cumplir las siguientes condiciones:

- La entrega debe existir.
- El estado de la entrega debe permitir la asignación.
- No debe existir otro repartidor activo asignado.
- El repartidor debe estar disponible.
- La ruta debe tener capacidad, cuando aplique esta restricción.
- La asignación debe quedar registrada correctamente.
- Los errores deben comunicarse sin confirmar operaciones fallidas.

## 5. Importancia de los escenarios

Los escenarios permiten convertir las reglas del negocio en comportamientos verificables.

En la siguiente etapa del proyecto podrán utilizarse como base para diseñar y ejecutar pruebas unitarias del dominio y pruebas de integración del flujo completo.

## 6. Conclusión

Los escenarios de aceptación permiten comprobar que la asignación de repartidores se realice de acuerdo con las reglas establecidas.

También facilitan la comunicación entre los responsables del negocio y el equipo de desarrollo, porque describen de forma clara qué debe suceder ante cada situación.