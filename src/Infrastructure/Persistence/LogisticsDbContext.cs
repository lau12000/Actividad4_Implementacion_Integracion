
using Logistics.Domain.Entities;
using Logistics.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Logistics.Infrastructure.Persistence
{
    /// <summary>
    /// Contexto principal de Entity Framework Core.
    /// Administra la persistencia de entregas y mensajeros.
    /// </summary>
    public class LogisticsDbContext : DbContext
    {
        public LogisticsDbContext(DbContextOptions<LogisticsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Delivery> Deliveries => Set<Delivery>();

        public DbSet<Courier> Couriers => Set<Courier>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Conversión de identificadores de entrega a Guid.
            var deliveryIdConverter =
                new ValueConverter<DeliveryId, Guid>(
                    id => id.Value,
                    value => new DeliveryId(value)
                );

            // Conversión de identificadores de mensajero a Guid.
            var courierIdConverter =
                new ValueConverter<CourierId, Guid>(
                    id => id.Value,
                    value => new CourierId(value)
                );

            // Configuración de la entidad Delivery.
            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.ToTable("Deliveries");

                entity.HasKey(delivery => delivery.Id);

                entity.Property(delivery => delivery.Id)
                    .HasConversion(deliveryIdConverter)
                    .ValueGeneratedNever();

                entity.Property(delivery => delivery.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(delivery => delivery.AssignedCourierId)
                    .HasConversion<Guid?>(
                        id => id == null ? (Guid?)null : id.Value,
                        value => value.HasValue ? new CourierId(value.Value) : null
                    )
                    .IsRequired(false);

                entity.Property(delivery => delivery.AssignedAt)
                    .IsRequired(false);
            });

            // Configuración de la entidad Courier.
            modelBuilder.Entity<Courier>(entity =>
            {
                entity.ToTable("Couriers");

                entity.HasKey(courier => courier.Id);

                entity.Property(courier => courier.Id)
                    .HasConversion(courierIdConverter)
                    .ValueGeneratedNever();

                entity.Property(courier => courier.Name)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(courier => courier.IsAvailable)
                    .IsRequired();
            });
        }
    }
}
