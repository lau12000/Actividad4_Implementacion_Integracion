# Plataforma de Gestión Logística de Última Milla

## Actividad 3: Diseño y dominio

**Programa:** Análisis y Desarrollo de Software - SENA  
**Proyecto:** Plataforma de Gestión Logística de Última Milla  
**Etapa:** Planeación - Diseño y dominio

## 1. Descripción del proyecto

La Plataforma de Gestión Logística de Última Milla busca organizar y facilitar los procesos relacionados con la administración de pedidos, entregas, mensajeros y rutas.

El propósito de esta actividad es definir el dominio del negocio, identificar sus entidades principales, establecer las reglas que deben cumplirse y diseñar la arquitectura que permitirá desarrollar la solución de manera organizada.

Para este ejercicio se seleccionó como proceso principal la asignación de un mensajero a una entrega.

## 2. Objetivo de la Actividad 3

Diseñar el modelo de dominio y la arquitectura de la solución, identificando las responsabilidades de cada componente y estableciendo las reglas del negocio antes de avanzar a la implementación.

## 3. Proceso de negocio seleccionado

**Asignación de mensajero a una entrega.**

El proceso comienza cuando un operador logístico solicita asignar un mensajero a una entrega pendiente.

La plataforma debe verificar que la entrega exista, que se encuentre en un estado permitido, que el mensajero esté disponible y que la ruta tenga capacidad cuando corresponda.

Si se cumplen las condiciones, se realiza la asignación y se actualiza el estado de la entrega.

## 4. Modelo de dominio

Las principales entidades identificadas son:

| Entidad | Responsabilidad |
|---|---|
| Delivery | Representa una entrega y controla su asignación. |
| Courier | Representa al mensajero encargado de realizar la entrega. |
| Order | Representa el pedido asociado a una entrega. |
| Assignment | Registra la relación de asignación entre entrega y mensajero. |

También se identifican objetos de valor como `DeliveryId`, `CourierId`, `OrderId`, `DeliveryAddress` y `DeliveryStatus`.

## 5. Arquitectura propuesta

Se propone una arquitectura de monolito modular con separación en tres capas:

- **Domain:** contiene las entidades, objetos de valor y reglas del negocio.
- **Application:** contiene los casos de uso y coordina las operaciones.
- **Infrastructure:** contiene los adaptadores técnicos para comunicarse con bases de datos y servicios externos.

Se utiliza el enfoque de puertos y adaptadores para reducir el acoplamiento entre la lógica del negocio y los detalles técnicos.

## 6. Estructura del repositorio

```text
Actividad3_Dominio_Diseno/
│
├── README.md
├── Architecture.md
│
├── docs/
│   └── actividad-3/
│       ├── diagramas/
│       │   └── diagramas-arquitectura.md
│       ├── flujo-negocio.md
│       ├── glosario-dominio.md
│       ├── modelo-dominio.md
│       ├── invariantes.md
│       ├── escenarios-aceptacion.md
│       ├── contextos-delimitados.md
│       ├── casos-uso-puertos-adaptadores.md
│       └── ADR-002-diseno-dominio.md
│
├── src/
│   ├── Domain/
│   ├── Application/
│   └── Infrastructure/
│
└── tests/
    ├── Domain/
    └── Integration/
```

## 7. Documentación de la actividad

Los documentos se encuentran organizados en `docs/actividad-3/`.

| Documento | Descripción |
|---|---|
| flujo-negocio.md | Describe el proceso de asignación. |
| glosario-dominio.md | Define los términos del negocio. |
| modelo-dominio.md | Identifica entidades, objetos de valor y estados. |
| invariantes.md | Establece las reglas que deben cumplirse. |
| escenarios-aceptacion.md | Describe los comportamientos esperados mediante escenarios. |
| contextos-delimitados.md | Define los límites y responsabilidades de los contextos. |
| casos-uso-puertos-adaptadores.md | Explica el caso de uso, los puertos y los adaptadores. |
| ADR-002-diseno-dominio.md | Registra las decisiones de diseño. |
| diagramas/diagramas-arquitectura.md | Reúne los diagramas del dominio y la arquitectura. |

El documento `Architecture.md`, ubicado en la raíz, describe la arquitectura general de la solución.

## 8. Reglas principales del negocio

- Una entrega debe existir antes de ser asignada.
- Una entrega solo puede tener un mensajero activo.
- El mensajero seleccionado debe estar disponible.
- Una entrega entregada no puede ser asignada nuevamente.
- Una entrega cancelada no puede recibir una asignación.
- La capacidad de la ruta debe validarse cuando corresponda.

## 9. Alcance de esta entrega

Esta actividad se concentra en el diseño del dominio y la arquitectura de la solución.

Se documentan las entidades, los estados, las reglas del negocio, los escenarios de aceptación, los contextos delimitados y los contratos entre componentes.

No se implementan todavía la persistencia real de datos ni las integraciones externas. Estos elementos se desarrollarán en las siguientes etapas del proyecto.

## 10. Tecnologías y herramientas previstas

- Visual Studio Code.
- Git y GitHub para el control de versiones.
- C# como lenguaje previsto para la implementación.
- Mermaid para la representación de diagramas.
- Markdown para la documentación técnica.

## 11. Repositorio

El código y la documentación de esta actividad se encuentran en:

[Repositorio de GitHub - Actividad 3](https://github.com/lau12000/Actividad3_Dominio_Diseno-)

## 12. Sustentación en video

**Enlace del video:** Pendiente de publicar.

En la sustentación se explicará el proceso de asignación de mensajeros, el modelo de dominio, las reglas del negocio, los contextos delimitados y la arquitectura propuesta.

## 13. Conclusión

El diseño realizado permite establecer una base clara para el desarrollo de la Plataforma de Gestión Logística de Última Milla.

La identificación de las entidades, reglas e invariantes ayuda a prevenir errores en los procesos principales, mientras que la separación por capas y el uso de puertos y adaptadores facilitan el mantenimiento y la evolución de la solución.