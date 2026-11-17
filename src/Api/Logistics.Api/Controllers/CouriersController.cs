
using Logistics.Domain.ValueObjects;
using Logistics.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouriersController : ControllerBase
    {
        private readonly LogisticsDbContext _context;

        public CouriersController(LogisticsDbContext context)
        {
            _context = context;
        }

        // Consultar todos los mensajeros.
        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var couriers = await _context.Couriers
                .AsNoTracking()
                .Select(courier => new
                {
                    Id = courier.Id.Value,
                    courier.Name,
                    courier.IsAvailable
                })
                .ToListAsync(cancellationToken);

            return Ok(couriers);
        }

        // Consultar un mensajero por su identificador.
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var courier = await _context.Couriers
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    item => item.Id == new CourierId(id),
                    cancellationToken
                );

            if (courier == null)
                return NotFound(new
                {
                    message = "Mensajero no encontrado."
                });

            return Ok(new
            {
                Id = courier.Id.Value,
                courier.Name,
                courier.IsAvailable
            });
        }
    }
}
