# Plataforma de Gestión Logística de Última Milla

## Actividad 4: Implementación e integración

**Programa:** Análisis y Desarrollo de Software - SENA  
**Proyecto:** Plataforma de Gestión Logística de Última Milla  
**Etapa:** Planeación e implementación

## 1. Descripción del proyecto

La Plataforma de Gestión Logística de Última Milla busca organizar y facilitar los procesos relacionados con la administración de entregas, mensajeros y rutas.

En esta actividad se implementó un flujo funcional para la asignación de un mensajero a una entrega, conectando las reglas del dominio con los casos de uso, la persistencia de datos y una API REST.

## 2. Objetivo de la Actividad 4

Implementar y validar el proceso de asignación de mensajeros mediante una solución ejecutable, aplicando separación por capas, persistencia de datos, pruebas automatizadas y mecanismos de verificación de calidad.

## 3. Proceso implementado

**Asignación de un mensajero a una entrega.**

El sistema permite:

1. Consultar las entregas registradas.
2. Consultar los mensajeros disponibles.
3. Verificar que la entrega exista y esté pendiente.
4. Validar la disponibilidad del mensajero.
5. Comprobar la capacidad configurada para la ruta.
6. Asignar el mensajero a la entrega.
7. Guardar los cambios en la base de datos.
8. Registrar la notificación de la asignación.

Si se intenta realizar una asignación que no cumple las reglas del negocio, el sistema rechaza la operación.

## 4. Arquitectura implementada

La solución utiliza una arquitectura por capas con separación de responsabilidades:

- **Domain:** contiene las entidades, objetos de valor, estados y reglas del negocio.
- **Application:** contiene los casos de uso y los contratos o puertos que necesita la aplicación.
- **Infrastructure:** implementa los adaptadores de persistencia, disponibilidad, capacidad de ruta y notificaciones.
- **Api:** expone los endpoints HTTP y conecta las solicitudes con los casos de uso.

Se mantiene el enfoque de puertos y adaptadores para reducir el acoplamiento entre las reglas del negocio y los detalles técnicos.

## 5. Tecnologías utilizadas

- C#.
- .NET 10.
- ASP.NET Core.
- Entity Framework Core.
- SQLite.
- xUnit para pruebas automatizadas.
- Git y GitHub.
- GitHub Actions.
- Visual Studio Code.

## 6. Estructura del repositorio

```text
Actividad4_Implementacion_Integracion/
│
├── .github/
│   └── workflows/
│       └── dotnet.yml
│
├── docs/
│   ├── actividad-3/
│   └── actividad-4/
│       ├── ADR-003-implementacion-vertical-slice.md
│       ├── pruebas-y-evidencias.md
│       ├── riesgos-y-observabilidad.md
│       ├── resultados-y-pendientes.md
│       └── guion-video.md
│
├── src/
│   ├── Api/
│   │   └── Logistics.Api/
│   ├── Application/
│   ├── Domain/
│   └── Infrastructure/
│
├── tests/
│   ├── Domain/
│   └── Integration/
│
├── Logistics.slnx
├── Architecture.md
└── README.md
```

## 7. Persistencia de datos

Se implementó SQLite como mecanismo de persistencia y Entity Framework Core para el mapeo entre los objetos del dominio y las tablas de la base de datos.

La estructura de la base de datos se administra mediante migraciones de Entity Framework Core.

Migración inicial:

`InitialCreate`

Archivo de conexión configurado:

`Data Source=logistics-activity4.db`

La base de datos local se excluye del repositorio mediante `.gitignore`.

## 8. Endpoints de la API

La API permite consultar mensajeros y entregas, además de realizar asignaciones.

| Método | Endpoint | Función |
|---|---|---|
| GET | `/api/couriers` | Consultar mensajeros. |
| GET | `/api/couriers/{id}` | Consultar un mensajero. |
| GET | `/api/deliveries` | Consultar entregas. |
| GET | `/api/deliveries/{id}` | Consultar una entrega. |
| POST | `/api/deliveries/{id}/assign` | Asignar un mensajero. |

Dirección local de ejecución:

`http://localhost:5069`

## 9. Ejecución del proyecto

### Requisitos previos

- .NET SDK 10.
- Git.
- Visual Studio Code o una terminal compatible.

### Clonar el repositorio

```bash
git clone https://github.com/lau12000/Actividad4_Implementacion_Integracion.git
cd Actividad4_Implementacion_Integracion
```

### Restaurar dependencias

```bash
dotnet restore Logistics.slnx
```

### Compilar la solución

```bash
dotnet build Logistics.slnx
```

### Ejecutar las pruebas

```bash
dotnet test Logistics.slnx
```

### Ejecutar la API

```bash
dotnet run --project src/Api/Logistics.Api/Logistics.Api.csproj
```

La API se ejecuta localmente en:

`http://localhost:5069`

La aplicación aplica las migraciones configuradas al iniciar.

## 10. Pruebas y validación

Se implementaron pruebas unitarias para las reglas del dominio y los casos de uso, además de pruebas de integración con SQLite en memoria.

Resultados de la última ejecución local:

- Total de pruebas: 12.
- Pruebas aprobadas: 12.
- Pruebas fallidas: 0.
- Pruebas omitidas: 0.

También se verificó el comportamiento de la API:

- Asignación exitosa de un mensajero.
- Consulta del estado actualizado de la entrega.
- Rechazo de una segunda asignación mediante respuesta HTTP 409.

## 11. Verificación automática de calidad

Se configuró GitHub Actions mediante el archivo:

`.github/workflows/dotnet.yml`

El flujo ejecuta automáticamente:

1. Restauración de dependencias.
2. Compilación de la solución.
3. Ejecución de las pruebas automatizadas.

Se registraron dos ejecuciones exitosas:

| Commit | Resultado |
|---|---|
| `dabaa48` | Correcto |
| `11c05b9` | Correcto |

Esto permite comprobar que los cambios publicados mantienen la compilación y las pruebas en estado satisfactorio.

## 12. Riesgos y limitaciones

Se documentaron riesgos relacionados con la asignación duplicada, disponibilidad de mensajeros, capacidad de rutas, persistencia, notificaciones y seguridad de acceso.

Entre las limitaciones actuales se encuentran:

- La capacidad de ruta utiliza una configuración fija y no un cálculo real de ocupación.
- Las notificaciones se realizan mediante registro en logs.
- No se implementó una cola de procesamiento asíncrono.
- No se implementó el patrón Outbox ni reintentos automáticos de notificación.
- Se requiere ampliar los mecanismos de autenticación y autorización antes de un despliegue productivo.

Las medidas de mitigación, señales de observabilidad y riesgos pendientes están descritos en la documentación de la actividad.

## 13. Documentación de la actividad

Los documentos técnicos se encuentran en `docs/actividad-4/`:

| Documento | Descripción |
|---|---|
| `ADR-003-implementacion-vertical-slice.md` | Registra las decisiones de implementación. |
| `pruebas-y-evidencias.md` | Describe las pruebas y sus resultados. |
| `riesgos-y-observabilidad.md` | Identifica riesgos, mitigaciones y señales de observabilidad. |
| `resultados-y-pendientes.md` | Registra resultados y aspectos pendientes. |
| `guion-video.md` | Contiene el guion de sustentación. |

También se conserva la documentación de diseño y dominio de la Actividad 3.

## 14. Repositorio

El código fuente y la documentación están disponibles en:

https://github.com/lau12000/Actividad4_Implementacion_Integracion

## 15. Sustentación en video

**Enlace del video:** Pendiente de publicar.

La sustentación explicará el problema, la arquitectura, el flujo implementado, la persistencia, las pruebas, los riesgos, la observabilidad y las decisiones técnicas.

## 16. Conclusión

La implementación permitió llevar el diseño de dominio realizado en la Actividad 3 a una solución ejecutable.

Se desarrolló el proceso de asignación de mensajeros, conectando las capas de la aplicación con persistencia SQLite y una API REST. Además, se incorporaron pruebas automatizadas y un flujo de integración continua con GitHub Actions.

La solución cuenta con una base funcional y documentada que permite continuar su evolución, incorporando posteriormente integraciones externas, controles de seguridad y mejoras operativas.