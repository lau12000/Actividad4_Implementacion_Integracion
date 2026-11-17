using Logistics.Domain.ValueObjects;

namespace Logistics.Application.Ports
{
    /// <summary>
    /// Define el contrato para consultar la disponibilidad de un mensajero.
    /// </summary>
    public interface ICourierAvailability
    {
        Task<bool> IsAvailableAsync(
            CourierId courierId,
            CancellationToken cancellationToken = default
        );
    }
}