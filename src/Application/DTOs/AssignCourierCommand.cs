using Logistics.Domain.ValueObjects;

namespace Logistics.Application.DTOs
{
    /// <summary>
    /// Contiene los datos necesarios para solicitar la asignación
    /// de un mensajero a una entrega.
    /// </summary>
    public sealed class AssignCourierCommand
    {
        public DeliveryId DeliveryId { get; }
        public CourierId CourierId { get; }

        public AssignCourierCommand(
            DeliveryId deliveryId,
            CourierId courierId)
        {
            DeliveryId = deliveryId
                ?? throw new ArgumentNullException(nameof(deliveryId));

            CourierId = courierId
                ?? throw new ArgumentNullException(nameof(courierId));
        }
    }
}