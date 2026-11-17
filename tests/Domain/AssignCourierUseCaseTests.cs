
using Logistics.Application.Ports;
using Logistics.Application.DTOs;
using Logistics.Application.UseCases;
using Logistics.Domain.Entities;
using Logistics.Domain.ValueObjects;

namespace Logistics.Tests;

public class AssignCourierUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldAssignAvailableCourier()
    {
        var delivery = new Delivery(DeliveryId.New());
        var courierId = CourierId.New();

        var repository = new FakeDeliveryRepository(delivery);
        var availability = new FakeCourierAvailability(true);
        var routeCapacity = new FakeRouteCapacity(true);
        var notifier = new FakeAssignmentNotifier();

        var useCase = new AssignCourierUseCase(
            repository, availability, routeCapacity, notifier);

        await useCase.ExecuteAsync(
            new AssignCourierCommand(delivery.Id, courierId));

        Assert.Equal(courierId, delivery.AssignedCourierId);
        Assert.True(repository.WasSaved);
        Assert.True(notifier.WasNotified);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRejectUnavailableCourier()
    {
        var delivery = new Delivery(DeliveryId.New());

        var repository = new FakeDeliveryRepository(delivery);
        var availability = new FakeCourierAvailability(false);
        var routeCapacity = new FakeRouteCapacity(true);
        var notifier = new FakeAssignmentNotifier();

        var useCase = new AssignCourierUseCase(
            repository, availability, routeCapacity, notifier);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(
                new AssignCourierCommand(delivery.Id, CourierId.New())));

        Assert.False(repository.WasSaved);
        Assert.False(notifier.WasNotified);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRejectDeliveryWithoutRouteCapacity()
    {
        var delivery = new Delivery(DeliveryId.New());

        var repository = new FakeDeliveryRepository(delivery);
        var availability = new FakeCourierAvailability(true);
        var routeCapacity = new FakeRouteCapacity(false);
        var notifier = new FakeAssignmentNotifier();

        var useCase = new AssignCourierUseCase(
            repository, availability, routeCapacity, notifier);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(
                new AssignCourierCommand(delivery.Id, CourierId.New())));

        Assert.False(repository.WasSaved);
        Assert.False(notifier.WasNotified);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRejectMissingDelivery()
    {
        var repository = new FakeDeliveryRepository(null);
        var availability = new FakeCourierAvailability(true);
        var routeCapacity = new FakeRouteCapacity(true);
        var notifier = new FakeAssignmentNotifier();

        var useCase = new AssignCourierUseCase(
            repository, availability, routeCapacity, notifier);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => useCase.ExecuteAsync(
                new AssignCourierCommand(DeliveryId.New(), CourierId.New())));

        Assert.False(repository.WasSaved);
        Assert.False(notifier.WasNotified);
    }

    private sealed class FakeDeliveryRepository : IDeliveryRepository
    {
        private readonly Delivery? _delivery;

        public bool WasSaved { get; private set; }

        public FakeDeliveryRepository(Delivery? delivery)
        {
            _delivery = delivery;
        }

        public Task<Delivery?> GetByIdAsync(
            DeliveryId deliveryId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                _delivery?.Id == deliveryId ? _delivery : null);
        }

        public Task SaveAsync(
            Delivery delivery,
            CancellationToken cancellationToken = default)
        {
            WasSaved = true;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeCourierAvailability : ICourierAvailability
    {
        private readonly bool _isAvailable;

        public FakeCourierAvailability(bool isAvailable)
        {
            _isAvailable = isAvailable;
        }

        public Task<bool> IsAvailableAsync(
            CourierId courierId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_isAvailable);
        }
    }

    private sealed class FakeRouteCapacity : IRouteCapacity
    {
        private readonly bool _hasCapacity;

        public FakeRouteCapacity(bool hasCapacity)
        {
            _hasCapacity = hasCapacity;
        }

        public Task<bool> HasCapacityForDeliveryAsync(
            DeliveryId deliveryId,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_hasCapacity);
        }
    }

    private sealed class FakeAssignmentNotifier : IAssignmentNotifier
    {
        public bool WasNotified { get; private set; }

        public Task NotifyAssignmentAsync(
            Delivery delivery,
            CancellationToken cancellationToken = default)
        {
            WasNotified = true;
            return Task.CompletedTask;
        }
    }
}
