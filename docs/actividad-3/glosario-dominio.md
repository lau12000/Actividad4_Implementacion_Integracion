# Glosario del dominio – Plataforma de Gestión Logística de Última Milla

## 1. Objetivo

Definir los términos principales utilizados en el proceso de asignación de repartidores a las entregas. El glosario establece un lenguaje común para evitar confusiones entre los usuarios, el equipo de desarrollo y las personas responsables de la operación logística.

## 2. Glosario de términos

| Término | Definición |
|---|---|
| Entrega | Proceso mediante el cual un pedido es transportado hasta la dirección indicada por el cliente. |
| Repartidor | Persona encargada de transportar y entregar los pedidos a los clientes. |
| Asignación | Acción de asociar un repartidor disponible con una entrega pendiente. |
| Operador logístico | Usuario responsable de coordinar las actividades de despacho y asignación de entregas. |
| Disponibilidad | Condición que indica si un repartidor puede recibir una nueva asignación. |
| Pedido | Solicitud realizada por un cliente que contiene uno o varios productos. |
| Ruta | Recorrido establecido para realizar una o varias entregas. |
| Dirección de entrega | Ubicación donde debe recibirse el pedido. |
| Estado de entrega | Situación actual de una entrega dentro del proceso logístico. |
| Incidencia | Situación inesperada que afecta el desarrollo normal de una entrega. |
| Entrega pendiente | Entrega registrada que todavía no ha sido asignada o completada. |
| Repartidor activo | Repartidor que mantiene una asignación vigente dentro del proceso logístico. |
| Dominio | Conjunto de conceptos, reglas y procesos que representan el negocio. |
| Entidad | Objeto del dominio que tiene identidad propia y un ciclo de vida. |
| Objeto de valor | Elemento definido por sus atributos y que no posee identidad independiente. |
| Invariante | Regla del negocio que debe cumplirse siempre para proteger la integridad del dominio. |
| Puerto | Contrato o interfaz que define una necesidad de comunicación de la aplicación con otro componente. |
| Adaptador | Implementación concreta que conecta un puerto con una tecnología o servicio externo. |
| Caso de uso | Representación de una acción específica que el sistema debe realizar para cumplir una necesidad del negocio. |

## 3. Términos que requieren aclaración

Durante el análisis se identificaron algunos términos que podrían tener diferentes interpretaciones y que deben confirmarse con los responsables del negocio.

| Término | Posible ambigüedad | Aclaración necesaria |
|---|---|---|
| Disponible | Puede significar que el repartidor no tiene entregas o que todavía tiene capacidad para recibir más. | Definir si un repartidor puede tener varias entregas activas y cuál es su capacidad máxima. |
| Entrega activa | Puede referirse a una entrega asignada, despachada o que todavía no ha sido finalizada. | Definir exactamente qué estados se consideran activos. |
| Ruta saturada | No está claro si depende del número de pedidos, distancia, tiempo o capacidad del vehículo. | Establecer los criterios para determinar cuándo una ruta alcanza su capacidad. |
| Asignación activa | Puede interpretarse como una asignación pendiente, en tránsito o todavía no cerrada. | Definir en qué momento una asignación deja de estar activa. |
| Entrega completada | Puede significar que el repartidor llegó a la dirección o que el cliente recibió el pedido. | Definir qué evidencia confirma la finalización de la entrega. |

## 4. Lenguaje común del proceso

Para mantener la coherencia del modelo se utilizarán las siguientes expresiones:

- **Asignar repartidor:** asociar un repartidor disponible a una entrega que cumple las condiciones del negocio.
- **Rechazar asignación:** detener el proceso cuando alguna regla del negocio no se cumple.
- **Confirmar asignación:** registrar que la entrega quedó asociada correctamente al repartidor.
- **Consultar disponibilidad:** verificar mediante un contrato si el repartidor puede recibir una nueva entrega.
- **Consultar entrega:** recuperar la información de una entrega utilizando su identificador.

## 5. Relación con el modelo de dominio

Los términos definidos en este glosario servirán como base para identificar las entidades, objetos de valor, estados, reglas e invariantes del modelo de dominio.

Los conceptos principales para el flujo seleccionado son Entrega