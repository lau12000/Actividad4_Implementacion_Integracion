using Logistics.Domain.ValueObjects;

namespace Logistics.Application.Ports
{
    /// <summary>
    /// Define el contrato para consultar si existe capacidad
    /// disponible en la ruta asociada a una entrega.
    /// </summary>
    public interface IRouteCapacity
    {
        Task<bool> HasCapacityForDeliveryAsync(
            DeliveryId deliveryId,
            CancellationToken cancellationToken = default
        );
    }
}