
using Logistics.Application.DTOs;
using Logistics.Application.Ports;
using Logistics.Application.UseCases;
using Logistics.Domain.Entities;
using Logistics.Domain.ValueObjects;
using Logistics.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Logistics.IntegrationTests;

public class AssignCourierIntegrationTests
{
    [Fact]
    public async Task AssignCourier_ShouldPersistAssignmentInSqlite()
    {
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LogisticsDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var dbContext = new LogisticsDbContext(options);

        await dbContext.Database.EnsureCreatedAsync();

        var delivery = new Delivery(DeliveryId.New());
        var courier = new Courier(CourierId.New(), "Mensajero de prueba");

        await dbContext.Deliveries.AddAsync(delivery);
        await dbContext.Couriers.AddAsync(courier);
        await dbContext.SaveChangesAsync();

        var repository = new SqliteDeliveryRepository(dbContext);
        var availability = new SqliteCourierAvailability(dbContext);

        var useCase = new AssignCourierUseCase(
            repository,
            availability,
            new FakeRouteCapacity(),
            new FakeAssignmentNotifier()
        );

        var command = new AssignCourierCommand(
            delivery.Id,
            courier.Id
        );

        await useCase.ExecuteAsync(command);

        var savedDelivery = await repository.GetByIdAsync(delivery.Id);

        Assert.NotNull(savedDelivery);
        Assert.Equal(courier.Id, savedDelivery.AssignedCourierId);
        Assert.NotNull(savedDelivery.AssignedAt);
        Assert.Equal(
            Logistics.Domain.Enums.DeliveryStatus.Assigned,
            savedDelivery.Status
        );
    }

    [Fact]
    public async Task AssignCourier_ShouldRejectSecondAssignment()
    {
        await using var connection =
            new SqliteConnection("Data Source=:memory:");

        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<LogisticsDbContext>()
            .UseSqlite(connection)
            .Options;

        await using var dbContext = new LogisticsDbContext(options);

        await dbContext.Database.EnsureCreatedAsync();

        var delivery = new Delivery(DeliveryId.New());
        var firstCourier = new Courier(CourierId.New(), "Carlos");
        var secondCourier = new Courier(CourierId.New(), "Maria");

        await dbContext.Deliveries.AddAsync(delivery);
        await dbContext.Couriers.AddRangeAsync(firstCourier, secondCourier);
        await dbContext.SaveChangesAsync();

        var repository = new SqliteDeliveryRepository(dbContext);
        var availability = new SqliteCourierAvailability(dbContext);

        var useCase = new AssignCourierUseCase(
            repository,
            availability,
            new FakeRouteCapacity(),
            new FakeAssignmentNotifier()
        );

        await useCase.ExecuteAsync(
            new AssignCourierCommand(delivery.Id, firstCourier.Id)
        );

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(
                new AssignCourierCommand(delivery.Id, secondCourier.Id)
            )
        );

        var savedDelivery = await repository.GetByIdAsync(delivery.Id);

        Assert.NotNull(savedDelivery);
        Assert.Equal(firstCourier.Id, savedDelivery.AssignedCourierId);
        Assert.Equal(
            Logistics.Domain.Enums.DeliveryStatus.Assigned,
            savedDelivery.Status
        );
    }

    private sealed class FakeRouteCapacity : IRouteCapacity
    {
        public Task<bool> HasCapacityForDeliveryAsync(
            DeliveryId deliveryId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class FakeAssignmentNotifier : IAssignmentNotifier
    {
        public Task NotifyAssignmentAsync(
            Delivery delivery,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
