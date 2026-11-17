# ADR-002: Diseño del dominio y arquitectura de la solución

## 1. Información general

- **Identificador:** ADR-002.
- **Título:** Diseño del dominio y arquitectura para la asignación de mensajeros.
- **Estado:** Aceptado.
- **Proyecto:** Plataforma de Gestión Logística de Última Milla.
- **Actividad:** Actividad 3 - Diseño y dominio.

## 2. Contexto

La Plataforma de Gestión Logística de Última Milla necesita organizar sus operaciones para administrar pedidos, entregas, mensajeros y rutas.

Uno de los procesos principales del negocio es la asignación de un mensajero a una entrega. Para realizar este proceso correctamente, es necesario establecer reglas que eviten asignaciones incorrectas y mantengan la información organizada.

Entre las condiciones identificadas se encuentran:

- Una entrega debe existir antes de ser asignada.
- Una entrega pendiente no debe tener otro mensajero activo.
- El mensajero seleccionado debe estar disponible.
- Una entrega entregada o cancelada no puede recibir una nueva asignación.
- La ruta debe contar con capacidad disponible cuando corresponda.

También se requiere separar las reglas del negocio de los detalles técnicos, como la persistencia de información y las integraciones con servicios externos.

## 3. Decisión de arquitectura

Se decide mantener una arquitectura de **monolito modular**, organizada en tres capas principales:

1. Domain (Dominio).
2. Application (Aplicación).
3. Infrastructure (Infraestructura).

Esta organización permite mantener las responsabilidades separadas sin introducir inicialmente la complejidad de una arquitectura distribuida en microservicios.

Se utilizará el enfoque de puertos y adaptadores para establecer los contratos de comunicación entre la aplicación y los servicios externos.

## 4. Diseño del dominio

La entidad principal del proceso es `Delivery`, porque representa la entrega que debe ser administrada y asignada.

Se identifican las siguientes entidades:

- Delivery: representa una entrega.
- Courier: representa un mensajero.
- Order: representa un pedido.
- Assignment: representa el registro de una asignación.

También se proponen objetos de valor:

- DeliveryId.
- CourierId.
- OrderId.
- DeliveryAddress.
- DeliveryStatus.
- AssignmentDate.

La entidad `Delivery` será responsable de proteger las reglas relacionadas con su estado y asignación.

## 5. Reglas del negocio

Se establecen las siguientes reglas:

1. Una entrega debe existir para poder ser asignada.
2. Una entrega solo puede tener un mensajero activo.
3. El mensajero debe estar disponible.
4. La entrega debe encontrarse en un estado permitido para la asignación.
5. Una entrega entregada no puede volver a asignarse.
6. Una entrega cancelada no puede ser asignada.
7. La asignación debe respetar la capacidad de la ruta cuando corresponda.

Estas reglas se documentan en `invariantes.md` y se relacionan con los escenarios de aceptación de `escenarios-aceptacion.md`.

## 6. Organización por capas

### Domain

Contiene las entidades, los objetos de valor, los estados y las reglas principales del negocio.

No debe depender de bases de datos, servicios externos ni frameworks de infraestructura.

### Application

Contiene los casos de uso que coordinan las operaciones del sistema.

Para esta actividad se identifica el caso de uso `AssignCourierUseCase`, encargado de coordinar la asignación de un mensajero a una entrega.

También define los puertos que necesita para consultar y guardar información.

### Infrastructure

Contiene los adaptadores encargados de implementar los puertos definidos por la aplicación.

En una etapa posterior podrá incluir adaptadores para bases de datos, servicios de disponibilidad, rutas y notificaciones.

En esta actividad se documenta su diseño, sin exigir integraciones externas reales.

## 7. Puertos y adaptadores

Se definen los siguientes puertos:

- `IDeliveryRepository`: consulta y guarda entregas.
- `ICourierAvailability`: consulta la disponibilidad del mensajero.
- `IRouteCapacity`: consulta la capacidad de una ruta cuando aplique.
- `IAssignmentNotifier`: permite informar que una asignación fue realizada.

Los adaptadores propuestos implementarán estos contratos sin modificar las reglas centrales del dominio.

## 8. Alternativas consideradas

### Alternativa 1: Arquitectura monolítica sin separación por capas

Se consideró desarrollar toda la lógica dentro de una misma estructura, pero se descartó porque dificultaría separar las reglas del negocio de los detalles técnicos.

### Alternativa 2: Arquitectura de microservicios

Se consideró dividir los procesos en servicios independientes, pero se descartó para esta etapa porque aumentaría la complejidad de desarrollo, comunicación y despliegue.

### Alternativa 3: Monolito modular con puertos y adaptadores

Se seleccionó esta alternativa porque permite mantener una estructura organizada, separar responsabilidades y facilitar futuros cambios tecnológicos.

## 9. Consecuencias de la decisión

### Consecuencias positivas

- Mayor separación de responsabilidades.
- Reglas del negocio centralizadas.
- Menor dependencia de tecnologías externas.
- Facilidad para realizar pruebas del dominio.
- Posibilidad de cambiar adaptadores sin modificar las reglas principales.
- Preparación para ampliar la solución en futuras etapas.

### Consecuencias y riesgos

- Se requiere mantener correctamente los límites entre las capas.
- Es necesario definir contratos claros entre los componentes.
- La asignación simultánea de una misma entrega requiere mecanismos de control de concurrencia.
- Las integraciones reales deberán desarrollarse y probarse en una etapa posterior.

## 10. Relación con los entregables

Esta decisión se refleja en los siguientes documentos:

| Documento | Contenido relacionado |
|---|---|
| flujo-negocio.md | Proceso de asignación de mensajero. |
| glosario-dominio.md | Lenguaje común del negocio. |
| modelo-dominio.md | Entidades, objetos de valor y estados. |
| invariantes.md | Reglas que deben cumplirse. |
| escenarios-aceptacion.md | Comportamientos esperados. |
| contextos-delimitados.md | Separación de responsabilidades del negocio. |
| casos-uso-puertos-adaptadores.md | Casos de uso y contratos de comunicación. |
| Architecture.md | Organización general de la arquitectura. |

## 11. Estado de implementación

La decisión de arquitectura queda aceptada para orientar el diseño de la solución.

En esta actividad se documentan el dominio, los contratos, los límites y las responsabilidades de los componentes. La implementación de persistencia, integraciones externas y mecanismos técnicos de concurrencia se realizará en etapas posteriores.

## 12. Conclusión

El diseño propuesto permite establecer una base organizada para la Plataforma de Gestión Logística de Última Milla.

La separación entre dominio, aplicación e infraestructura facilita el mantenimiento y la evolución del sistema. Además, la definición de reglas, invariantes y escenarios de aceptación permite comprender cómo debe comportarse el proceso de asignación antes de implementar sus integraciones técnicas.