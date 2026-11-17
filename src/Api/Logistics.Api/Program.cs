
using Logistics.Application.Ports;
using Logistics.Application.UseCases;
using Logistics.Domain.Entities;
using Logistics.Domain.ValueObjects;
using Logistics.Infrastructure.Integrations;
using Logistics.Infrastructure.Notifications;
using Logistics.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. REGISTRO DE SERVICIOS
// ==========================================

// Registrar controladores.
builder.Services.AddControllers();

// Configurar la conexión con SQLite.
var connectionString = builder.Configuration
    .GetConnectionString("LogisticsDatabase")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión LogisticsDatabase."
    );

builder.Services.AddDbContext<LogisticsDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Registrar el caso de uso de asignación.
builder.Services.AddScoped<AssignCourierUseCase>();

// Registrar los adaptadores de persistencia.
builder.Services.AddScoped<
    IDeliveryRepository,
    SqliteDeliveryRepository
>();

builder.Services.AddScoped<
    ICourierAvailability,
    SqliteCourierAvailability
>();

// Registrar los adaptadores de integración.
builder.Services.AddScoped<
    IRouteCapacity,
    ConfiguredRouteCapacity
>();

builder.Services.AddScoped<
    IAssignmentNotifier,
    LoggingAssignmentNotifier
>();

// Configurar OpenAPI.
builder.Services.AddOpenApi();

var app = builder.Build();

// ==========================================
// 2. CREACIÓN Y CONFIGURACIÓN DE LA BASE DE DATOS
// ==========================================

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<LogisticsDbContext>();

    // Crear la base de datos si todavía no existe.
    await dbContext.Database.MigrateAsync();

    // ==========================================
    // 3. INSERTAR MENSAJEROS DE PRUEBA
    // ==========================================

    if (!await dbContext.Couriers.AnyAsync())
    {
        var courier1 = new Courier(
            CourierId.New(),
            "Carlos Ramírez"
        );

        var courier2 = new Courier(
            CourierId.New(),
            "María González"
        );

        await dbContext.Couriers.AddRangeAsync(
            courier1,
            courier2
        );

        await dbContext.SaveChangesAsync();
    }

    // ==========================================
    // 4. INSERTAR ENTREGAS DE PRUEBA
    // ==========================================

    if (!await dbContext.Deliveries.AnyAsync())
    {
        var delivery1 = new Delivery(
            DeliveryId.New()
        );

        var delivery2 = new Delivery(
            DeliveryId.New()
        );

        await dbContext.Deliveries.AddRangeAsync(
            delivery1,
            delivery2
        );

        await dbContext.SaveChangesAsync();
    }
}

// ==========================================
// 5. CONFIGURACIÓN DE LOS ENDPOINTS
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ==========================================
// 6. INICIAR LA APLICACIÓN
// ==========================================

app.Run();
