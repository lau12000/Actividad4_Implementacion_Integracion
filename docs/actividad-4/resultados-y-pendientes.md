# Resultados y pendientes – Actividad 4

## 1. Resultados obtenidos

Durante la implementación de la Actividad 4 se construyó e integró un flujo funcional para la asignación de mensajeros a entregas dentro del proyecto Logistics.

Los principales resultados fueron:

- Se implementó el caso de uso `AssignCourierUseCase`.
- Se conectaron las capas de API, aplicación, dominio e infraestructura.
- Se implementaron contratos para consultar y guardar entregas, verificar disponibilidad de mensajeros, validar capacidad de rutas y notificar asignaciones.
- Se integró Entity Framework Core con SQLite para la persistencia de información.
- Se creó y aplicó una migración inicial para generar la estructura de la base de datos.
- Se verificó que una asignación válida se guarde correctamente.
- Se comprobó que una entrega no pueda asignarse nuevamente cuando ya tiene un mensajero asignado.

## 2. Verificación técnica

Los resultados de las verificaciones realizadas fueron:

| Verificación | Resultado |
|---|---|
| Compilación de la solución | Correcta |
| Pruebas automatizadas | 12 pruebas aprobadas |
| Pruebas con SQLite | Correctas |
| Persistencia de asignación | Verificada |
| Rechazo de segunda asignación | Verificado con HTTP 409 |
| Migración de base de datos | Creada y aplicada |

Comando utilizado para compilar:

```powershell
dotnet build Logistics.slnx
```

Comando utilizado para ejecutar las pruebas:

```powershell
dotnet test Logistics.slnx
```

Comandos utilizados para administrar las migraciones:

```powershell
dotnet ef migrations list --project src/Infrastructure/Logistics.Infrastructure.csproj --startup-project src/Api/Logistics.Api/Logistics.Api.csproj
```

```powershell
dotnet ef database update --project src/Infrastructure/Logistics.Infrastructure.csproj --startup-project src/Api/Logistics.Api/Logistics.Api.csproj
```

## 3. Riesgos y limitaciones pendientes

Aunque el flujo principal funciona, se identificaron aspectos que deben mejorarse antes de considerar la solución lista para producción:

1. La validación de capacidad de las rutas utiliza actualmente un adaptador de configuración fija y no un cálculo real de capacidad.
2. La notificación se ejecuta después de guardar la asignación. Si falla, la asignación puede quedar almacenada aunque la API informe un error.
3. No se implementó todavía un mecanismo Outbox para garantizar la entrega de notificaciones.
4. No se configuró una cola de mensajería con reintentos y cola de mensajes fallidos.
5. Se requiere fortalecer la autenticación y autorización antes de exponer la API en un entorno real.
6. Se deben establecer copias de seguridad y monitoreo centralizado para producción.

## 4. Próximos pasos

- Ejecutar las verificaciones automatizadas desde GitHub Actions.
- Revisar que el repositorio no incluya archivos temporales, bases de datos locales ni carpetas de compilación.
- Actualizar el README con las instrucciones de instalación, ejecución y pruebas.
- Registrar los cambios de implementación en el historial de Git.
- Grabar el video de sustentación mostrando el flujo implementado y sus pruebas.
- Incorporar el enlace del video en la documentación del proyecto.

## 5. Conclusión

La Actividad 4 permitió pasar del diseño arquitectónico a una implementación funcional. Se construyó un flujo vertical que conecta la entrada de la API con la lógica de aplicación, las reglas del dominio y la persistencia en SQLite.

Las pruebas realizadas permitieron comprobar tanto el funcionamiento de una asignación válida como el rechazo de una segunda asignación sobre una entrega que ya se encuentra asignada.

La solución cuenta con una base funcional y verificable, además de una identificación de los riesgos y mejoras necesarias para continuar su evolución.

## Verificación automática con GitHub Actions

Se configuró un flujo de integración continua mediante GitHub Actions
en el archivo `.github/workflows/dotnet.yml`.

El flujo ejecuta automáticamente:
- Restauración de dependencias.
- Compilación de la solución.
- Ejecución de pruebas automatizadas.

Resultado de la ejecución:
- Estado: Correcto.
- Rama: main.
- Commit: dabaa48.
- Duración: 42 segundos.

La ejecución finalizó satisfactoriamente, sin errores reportados.