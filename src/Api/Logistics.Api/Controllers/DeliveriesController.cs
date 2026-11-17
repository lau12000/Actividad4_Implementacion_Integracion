
using Logistics.Application.DTOs;
using Logistics.Application.UseCases;
using Logistics.Domain.Entities;
using Logistics.Domain.Enums;
using Logistics.Domain.ValueObjects;
using Logistics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveriesController : ControllerBase
    {
        private readonly LogisticsDbContext _context;
        private readonly AssignCourierUseCase _assignCourierUseCase;

        public DeliveriesController(
            LogisticsDbContext context,
            AssignCourierUseCase assignCourierUseCase)
        {
            _context = context;
            _assignCourierUseCase = assignCourierUseCase;
        }

        // Consultar todas las entregas.
        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var deliveries = await _context.Deliveries
                .AsNoTracking()
                .Select(delivery => new
                {
                    Id = delivery.Id.Value,
                    Status = delivery.Status.ToString(),
                    CourierId = delivery.AssignedCourierId == null
                        ? (Guid?)null
                        : delivery.AssignedCourierId.Value,
                    delivery.AssignedAt
                })
                .ToListAsync(cancellationToken);

            return Ok(deliveries);
        }

        // Consultar una entrega por su identificador.
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var delivery = await _context.Deliveries
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    item => item.Id == new DeliveryId(id),
                    cancellationToken
                );

            if (delivery == null)
                return NotFound(new { message = "Entrega no encontrada." });

            return Ok(new
            {
                Id = delivery.Id.Value,
                Status = delivery.Status.ToString(),
                CourierId = delivery.AssignedCourierId == null
                    ? (Guid?)null
                    : delivery.AssignedCourierId.Value,
                delivery.AssignedAt
            });
        }

        // Asignar un mensajero a una entrega.
        [HttpPost("{id:guid}/assign")]
        public async Task<IActionResult> AssignCourier(
            Guid id,
            [FromBody] AssignCourierRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = new AssignCourierCommand(
                    new DeliveryId(id),
                    new CourierId(request.CourierId)
                );

                await _assignCourierUseCase.ExecuteAsync(
                    command,
                    cancellationToken
                );

                return Ok(new
                {
                    message = "Mensajero asignado correctamente.",
                    deliveryId = id,
                    courierId = request.CourierId
                });
            }
            catch (KeyNotFoundException exception)
            {
                return NotFound(new { message = exception.Message });
            }
            catch (InvalidOperationException exception)
            {
                return Conflict(new { message = exception.Message });
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }
    }

    public class AssignCourierRequest
    {
        public Guid CourierId { get; set; }
    }
}

