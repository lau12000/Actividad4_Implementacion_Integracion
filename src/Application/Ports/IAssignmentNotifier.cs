using Logistics.Domain.Entities;

namespace Logistics.Application.Ports
{
    /// <summary>
    /// Define el contrato para notificar la asignación de una entrega.
    /// </summary>
    public interface IAssignmentNotifier
    {
        Task NotifyAssignmentAsync(
            Delivery delivery,
            CancellationToken cancellationToken = default
        );
    }
}