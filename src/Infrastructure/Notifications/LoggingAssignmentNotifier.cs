
using Logistics.Application.Ports;
using Logistics.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Logistics.Infrastructure.Notifications
{
    /// <summary>
    /// Registra las notificaciones de asignación mediante ILogger.
    /// </summary>
    public class LoggingAssignmentNotifier : IAssignmentNotifier
    {
        private readonly ILogger<LoggingAssignmentNotifier> _logger;

        public LoggingAssignmentNotifier(
            ILogger<LoggingAssignmentNotifier> logger)
        {
            _logger = logger;
        }

        public Task NotifyAssignmentAsync(
            Delivery delivery,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _logger.LogInformation(
                "Entrega {DeliveryId} asignada al mensajero {CourierId} en {AssignedAt}",
                delivery.Id,
                delivery.AssignedCourierId,
                delivery.AssignedAt
            );

            return Task.CompletedTask;
        }
    }
}
