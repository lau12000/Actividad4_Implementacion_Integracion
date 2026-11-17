
using Logistics.Domain.ValueObjects;

namespace Logistics.Domain.Entities
{
    /// <summary>
    /// Representa el registro de asignación de un mensajero a una entrega.
    /// </summary>
    public sealed class Assignment
    {
        public DeliveryId DeliveryId { get; }

        public CourierId CourierId { get; }

        public DateTime AssignedAt { get; }

        public Assignment(
            DeliveryId deliveryId,
            CourierId courierId,
            DateTime assignedAt)
        {
            DeliveryId = deliveryId
                ?? throw new ArgumentNullException(nameof(deliveryId));

            CourierId = courierId
                ?? throw new ArgumentNullException(nameof(courierId));

            if (assignedAt == default)
            {
                throw new ArgumentException(
                    "La fecha de asignación es obligatoria."
                );
            }

            AssignedAt = assignedAt;
        }

        public static Assignment Create(
            DeliveryId deliveryId,
            CourierId courierId)
        {
            return new Assignment(
                deliveryId,
                courierId,
                DateTime.UtcNow
            );
        }
    }
}
