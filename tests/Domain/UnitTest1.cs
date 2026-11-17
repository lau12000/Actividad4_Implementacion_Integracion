
using Logistics.Domain.Entities;
using Logistics.Domain.Enums;
using Logistics.Domain.ValueObjects;

namespace Logistics.Tests;

public class DeliveryTests
{
    [Fact]
    public void NewDelivery_ShouldStartAsPending()
    {
        var delivery = new Delivery(DeliveryId.New());

        Assert.Equal(DeliveryStatus.Pending, delivery.Status);
        Assert.Null(delivery.AssignedCourierId);
    }

    [Fact]
    public void AssignCourier_ShouldAssignCourierToPendingDelivery()
    {
        var delivery = new Delivery(DeliveryId.New());
        var courierId = CourierId.New();

        delivery.AssignCourier(courierId);

        Assert.Equal(DeliveryStatus.Assigned, delivery.Status);
        Assert.Equal(courierId, delivery.AssignedCourierId);
        Assert.NotNull(delivery.AssignedAt);
    }

    [Fact]
    public void AssignCourier_ShouldRejectSecondAssignment()
    {
        var delivery = new Delivery(DeliveryId.New());

        delivery.AssignCourier(CourierId.New());

        Assert.Throws<InvalidOperationException>(
            () => delivery.AssignCourier(CourierId.New()));
    }

    [Fact]
    public void StartTransit_ShouldRejectPendingDelivery()
    {
        var delivery = new Delivery(DeliveryId.New());

        Assert.Throws<InvalidOperationException>(
            () => delivery.StartTransit());
    }

    [Fact]
    public void CompleteDelivery_ShouldChangeStatusToDelivered()
    {
        var delivery = new Delivery(DeliveryId.New());

        delivery.AssignCourier(CourierId.New());
        delivery.StartTransit();
        delivery.CompleteDelivery();

        Assert.Equal(DeliveryStatus.Delivered, delivery.Status);
    }

    [Fact]
    public void Cancel_ShouldRejectDeliveredDelivery()
    {
        var delivery = new Delivery(DeliveryId.New());

        delivery.AssignCourier(CourierId.New());
        delivery.StartTransit();
        delivery.CompleteDelivery();

        Assert.Throws<InvalidOperationException>(
            () => delivery.Cancel());
    }
}
