
namespace Logistics.Domain.Enums
{
    /// <summary>
    /// Representa los estados posibles de una entrega.
    /// </summary>
    public enum DeliveryStatus
    {
        Pending,
        Assigned,
        InTransit,
        Delivered,
        Failed,
        Cancelled
    }
}
