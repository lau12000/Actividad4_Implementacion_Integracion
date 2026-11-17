# Diagramas de dominio y arquitectura

**Proyecto:** Plataforma de Gestión Logística de Última Milla  
**Actividad:** Actividad 3 - Diseño y dominio

Este documento reúne los diagramas que representan el modelo de dominio, el proceso de asignación de mensajeros, los contextos delimitados y la arquitectura propuesta para la plataforma.

## 1. Diagrama del modelo de dominio

El modelo representa las entidades principales que participan en el proceso de gestión de entregas.

```mermaid
classDiagram
    class Order {
        +OrderId id
        +string customerName
        +string deliveryAddress
    }

    class Delivery {
        +DeliveryId id
        +DeliveryStatus status
        +CourierId assignedCourierId
        +AssignCourier(courierId)
        +ChangeStatus(status)
    }

    class Courier {
        +CourierId id
        +string name
        +bool available
    }

    class Assignment {
        +DeliveryId deliveryId
        +CourierId courierId
        +datetime assignedAt
    }

    Order "1" --> "1" Delivery : genera
    Delivery "1" --> "0..*" Assignment : registra
    Courier "1" --> "0..*" Assignment : participa
```

## 2. Diagrama de estados de una entrega

El siguiente diagrama representa los posibles estados de una entrega y sus transiciones principales.

```mermaid
stateDiagram-v2
    [*] --> Pending
    Pending --> Assigned: Asignar mensajero
    Assigned --> InTransit: Iniciar recorrido
    InTransit --> Delivered: Confirmar entrega
    InTransit --> Failed: Reportar novedad
    Pending --> Cancelled: Cancelar
    Assigned --> Cancelled: Cancelar
    Failed --> Pending: Reprogramar
    Delivered --> [*]
    Cancelled --> [*]
```

## 3. Diagrama del flujo de negocio

Representa el proceso que realiza el operador para asignar un mensajero.

```mermaid
flowchart TD
    A["Inicio"]
    B["Operador solicita asignación"]
    C["Consultar entrega"]
    D{"¿Existe la entrega?"}
    E["Consultar disponibilidad"]
    F{"¿Mensajero disponible?"}
    G{"¿Entrega en estado permitido?"}
    H["Validar capacidad de ruta"]
    I{"¿Hay capacidad?"}
    J["Asignar mensajero"]
    K["Guardar resultado"]
    L["Informar asignación exitosa"]
    M["Informar motivo del rechazo"]
    N["Fin"]

    A --> B
    B --> C
    C --> D
    D -- "No" --> M
    D -- "Sí" --> E
    E --> F
    F -- "No" --> M
    F -- "Sí" --> G
    G -- "No" --> M
    G -- "Sí" --> H
    H --> I
    I -- "No" --> M
    I -- "Sí" --> J
    J --> K
    K --> L
    L --> N
    M --> N
```

## 4. Diagrama de contextos delimitados

Se muestran los límites de responsabilidad de los principales contextos del negocio.

```mermaid
flowchart TD
    OM["Order Management<br/>Gestión de pedidos"]
    DM["Delivery Management<br/>Gestión de entregas"]
    CM["Courier Management<br/>Gestión de mensajeros"]
    RM["Route Management<br/>Gestión de rutas"]

    OM -->|"Información del pedido"| DM
    DM -->|"Consulta disponibilidad"| CM
    DM -->|"Consulta capacidad"| RM
```

El contexto Delivery Management coordina el proceso de asignación y consulta la información que necesita de los demás contextos.

## 5. Diagrama de arquitectura por capas

Representa la separación entre dominio, aplicación e infraestructura.

```mermaid
flowchart TD
    UI["Interfaz de usuario o API"]
    APP["Application<br/>Casos de uso"]
    DOMAIN["Domain<br/>Entidades y reglas"]
    PORTS["Puertos de salida"]
    INFRA["Infrastructure<br/>Adaptadores"]
    DB["Persistencia futura"]
    EXT["Servicios externos futuros"]

    UI --> APP
    APP --> DOMAIN
    APP --> PORTS
    INFRA -.->|"Implementa"| PORTS
    INFRA --> DB
    INFRA --> EXT
```

## 6. Diagrama de puertos y adaptadores

Representa cómo el caso de uso se comunica con las capacidades externas mediante contratos.

```mermaid
flowchart LR
    USER["Operador"]
    INPUT["DeliveryController"]
    USECASE["AssignCourierUseCase"]

    REPO["IDeliveryRepository"]
    AVAIL["ICourierAvailability"]
    ROUTE["IRouteCapacity"]
    NOTIFY["IAssignmentNotifier"]

    ADAPTERS["Adaptadores de infraestructura"]

    USER --> INPUT
    INPUT --> USECASE
    USECASE --> REPO
    USECASE --> AVAIL
    USECASE --> ROUTE
    USECASE --> NOTIFY

    ADAPTERS -.-> REPO
    ADAPTERS -.-> AVAIL
    ADAPTERS -.-> ROUTE
    ADAPTERS -.-> NOTIFY
```

## 7. Diagrama de secuencia de asignación

Describe el intercambio de mensajes durante la ejecución del caso de uso.

```mermaid
sequenceDiagram
    actor Operador
    participant Controller as DeliveryController
    participant App as AssignCourierUseCase
    participant Repo as IDeliveryRepository
    participant Availability as ICourierAvailability
    participant Domain as Delivery

    Operador->>Controller: Solicita asignación
    Controller->>App: Ejecutar solicitud
    App->>Repo: Consultar entrega
    Repo-->>App: Entrega encontrada
    App->>Availability: Consultar mensajero
    Availability-->>App: Disponibilidad confirmada
    App->>Domain: AssignCourier(courierId)
    Domain-->>App: Resultado de validación
    App->>Repo: Guardar cambios
    Repo-->>App: Confirmación
    App-->>Controller: Resultado
    Controller-->>Operador: Asignación exitosa
```

## 8. Diagrama de organización del proyecto

```mermaid
flowchart TD
    ROOT["Actividad3_Dominio_Diseno"]
    DOCS["docs"]
    SRC["src"]
    TESTS["tests"]

    DOMAIN["Domain"]
    APPLICATION["Application"]
    INFRA["Infrastructure"]

    ROOT --> DOCS
    ROOT --> SRC
    ROOT --> TESTS

    SRC --> DOMAIN
    SRC --> APPLICATION
    SRC --> INFRA

    DOMAIN --> ENT["Entities / ValueObjects / Enums"]
    APPLICATION --> UC["UseCases / Ports / DTOs"]
    INFRA --> AD["Persistence / Integrations / Notifications"]
```

## 9. Conclusión

Los diagramas permiten visualizar cómo se organiza el dominio de la plataforma y cómo se relacionan sus componentes.

El diseño mantiene las reglas del negocio dentro del dominio, utiliza casos de uso para coordinar las operaciones y propone puertos y adaptadores para mantener separadas las responsabilidades técnicas.

Los diagramas representan la arquitectura propuesta para la Actividad 3. Las integraciones y los mecanismos de persistencia se desarrollarán en etapas posteriores.