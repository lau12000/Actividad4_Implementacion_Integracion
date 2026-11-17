
using Logistics.Application.Ports;
using Logistics.Domain.Entities;
using Logistics.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Infrastructure.Persistence
{
    /// <summary>
    /// Implementación de IDeliveryRepository utilizando SQLite.
    /// </summary>
    public class SqliteDeliveryRepository : IDeliveryRepository
    {
        private readonly LogisticsDbContext _context;

        public SqliteDeliveryRepository(LogisticsDbContext context)
        {
            _context = context;
        }

        public async Task<Delivery?> GetByIdAsync(
            DeliveryId deliveryId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Deliveries
                .FirstOrDefaultAsync(
                    delivery => delivery.Id == deliveryId,
                    cancellationToken
                );
        }

        public async Task SaveAsync(
            Delivery delivery,
            CancellationToken cancellationToken = default)
        {
            var existingDelivery = await _context.Deliveries
                .FirstOrDefaultAsync(
                    item => item.Id == delivery.Id,
                    cancellationToken
                );

            if (existingDelivery == null)
            {
                await _context.Deliveries.AddAsync(
                    delivery,
                    cancellationToken
                );
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
