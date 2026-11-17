
using Logistics.Domain.Enums;
using Logistics.Domain.ValueObjects;

namespace Logistics.Domain.Entities
{
    /// <summary>
    /// Representa una entrega dentro de la plataforma logística.
    /// Contiene las reglas del negocio relacionadas con su asignación.
    /// </summary>
    public class Delivery
    {
        public DeliveryId Id { get; private set; }

        public DeliveryStatus Status { get; private set; }

        public CourierId? AssignedCourierId { get; private set; }

        public DateTime? AssignedAt { get; private set; }

        public Delivery(DeliveryId id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));

            Status = DeliveryStatus.Pending;
            AssignedCourierId = null;
            AssignedAt = null;
        }

        /// <summary>
        /// Asigna un mensajero a la entrega.
        /// </summary>
        public void AssignCourier(CourierId courierId)
        {
            if (courierId == null)
            {
                throw new ArgumentNullException(nameof(courierId));
            }

            if (Status != DeliveryStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Solo se pueden asignar mensajeros a entregas pendientes."
                );
            }

            if (AssignedCourierId != null)
            {
                throw new InvalidOperationException(
                    "La entrega ya tiene un mensajero asignado."
                );
            }

            AssignedCourierId = courierId;
            AssignedAt = DateTime.UtcNow;
            Status = DeliveryStatus.Assigned;
        }

        /// <summary>
        /// Cambia el estado de una entrega que ya fue asignada.
        /// </summary>
        public void StartTransit()
        {
            if (Status != DeliveryStatus.Assigned)
            {
                throw new InvalidOperationException(
                    "Solo una entrega asignada puede iniciar su recorrido."
                );
            }

            Status = DeliveryStatus.InTransit;
        }

        /// <summary>
        /// Confirma que la entrega fue completada.
        /// </summary>
        public void CompleteDelivery()
        {
            if (Status != DeliveryStatus.InTransit)
            {
                throw new InvalidOperationException(
                    "La entrega debe estar en tránsito para ser completada."
                );
            }

            Status = DeliveryStatus.Delivered;
        }

        /// <summary>
        /// Cancela una entrega que todavía no ha sido completada.
        /// </summary>
        public void Cancel()
        {
            if (Status == DeliveryStatus.Delivered ||
                Status == DeliveryStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "No se puede cancelar una entrega completada o cancelada."
                );
            }

            Status = DeliveryStatus.Cancelled;
        }
    }
}
