using Logistics.Domain.Entities;
using Logistics.Domain.ValueObjects;

namespace Logistics.Application.Ports
{
    /// <summary>
    /// Define las operaciones necesarias para consultar y guardar entregas.
    /// No depende de una base de datos específica.
    /// </summary>
    public interface IDeliveryRepository
    {
        Task<Delivery?> GetByIdAsync(
            DeliveryId deliveryId,
            CancellationToken cancellationToken = default
        );

        Task SaveAsync(
            Delivery delivery,
            CancellationToken cancellationToken = default
        );
    }
}