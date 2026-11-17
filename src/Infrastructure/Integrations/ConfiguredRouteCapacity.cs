
using Logistics.Application.Ports;
using Logistics.Domain.ValueObjects;

namespace Logistics.Infrastructure.Integrations
{
    /// <summary>
    /// Adaptador de capacidad de rutas configurable.
    /// Permite habilitar o bloquear asignaciones según la capacidad.
    /// </summary>
    public class ConfiguredRouteCapacity : IRouteCapacity
    {
        private readonly bool _hasAvailableCapacity;

        public ConfiguredRouteCapacity(bool hasAvailableCapacity = true)
        {
            _hasAvailableCapacity = hasAvailableCapacity;
        }

        public Task<bool> HasCapacityForDeliveryAsync(
            DeliveryId deliveryId,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(_hasAvailableCapacity);
        }
    }
}
