
using Logistics.Domain.ValueObjects;

namespace Logistics.Domain.Entities
{
    /// <summary>
    /// Representa un pedido realizado por un cliente.
    /// </summary>
    public class Order
    {
        public OrderId Id { get; private set; }

        public string CustomerName { get; private set; }

        public DeliveryAddress DeliveryAddress { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public Order(
            OrderId id,
            string customerName,
            DeliveryAddress deliveryAddress)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("El nombre del cliente es obligatorio.");

            CustomerName = customerName;

            DeliveryAddress = deliveryAddress
                ?? throw new ArgumentNullException(nameof(deliveryAddress));

            CreatedAt = DateTime.UtcNow;
        }
    }
}
