
using Logistics.Application.DTOs;
using Logistics.Application.Ports;

namespace Logistics.Application.UseCases
{
    /// <summary>
    /// Coordina el proceso de asignación de un mensajero a una entrega.
    /// </summary>
    public sealed class AssignCourierUseCase
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ICourierAvailability _courierAvailability;
        private readonly IRouteCapacity _routeCapacity;
        private readonly IAssignmentNotifier _assignmentNotifier;

        public AssignCourierUseCase(
            IDeliveryRepository deliveryRepository,
            ICourierAvailability courierAvailability,
            IRouteCapacity routeCapacity,
            IAssignmentNotifier assignmentNotifier)
        {
            _deliveryRepository = deliveryRepository
                ?? throw new ArgumentNullException(nameof(deliveryRepository));

            _courierAvailability = courierAvailability
                ?? throw new ArgumentNullException(nameof(courierAvailability));

            _routeCapacity = routeCapacity
                ?? throw new ArgumentNullException(nameof(routeCapacity));

            _assignmentNotifier = assignmentNotifier
                ?? throw new ArgumentNullException(nameof(assignmentNotifier));
        }

        public async Task ExecuteAsync(
            AssignCourierCommand command,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(command);

            // 1. Buscar la entrega.
            var delivery = await _deliveryRepository.GetByIdAsync(
                command.DeliveryId,
                cancellationToken);

            if (delivery is null)
                throw new KeyNotFoundException("La entrega solicitada no existe.");

            // 2. Verificar disponibilidad del mensajero.
            var isAvailable = await _courierAvailability.IsAvailableAsync(
                command.CourierId,
                cancellationToken);

            if (!isAvailable)
                throw new InvalidOperationException(
                    "El mensajero no está disponible.");

            // 3. Verificar capacidad de la ruta.
            var hasCapacity = await _routeCapacity.HasCapacityForDeliveryAsync(
                command.DeliveryId,
                cancellationToken);

            if (!hasCapacity)
                throw new InvalidOperationException(
                    "La ruta no tiene capacidad disponible.");

            // 4. Aplicar las reglas de asignación del dominio.
            delivery.AssignCourier(command.CourierId);

            // 5. Guardar los cambios.
            await _deliveryRepository.SaveAsync(
                delivery,
                cancellationToken);

            // 6. Notificar la asignación.
            await _assignmentNotifier.NotifyAssignmentAsync(
                delivery,
                cancellationToken);
        }
    }
}
