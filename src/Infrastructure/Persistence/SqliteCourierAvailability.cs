
using Logistics.Application.Ports;
using Logistics.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Persistence
{
    /// <summary>
    /// Consulta en SQLite si un mensajero está disponible.
    /// </summary>
    public class SqliteCourierAvailability : ICourierAvailability
    {
        private readonly LogisticsDbContext _context;

        public SqliteCourierAvailability(LogisticsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAvailableAsync(
            CourierId courierId,
            CancellationToken cancellationToken = default)
        {
            var courier = await _context.Couriers
                .FirstOrDefaultAsync(
                    item => item.Id == courierId,
                    cancellationToken
                );

            return courier != null && courier.IsAvailable;
        }
    }
}
