
namespace Logistics.Domain.ValueObjects
{
    /// <summary>
    /// Representa el identificador único de un pedido.
    /// </summary>
    public sealed class OrderId
    {
        public Guid Value { get; }

        public OrderId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(
                    "El identificador del pedido no puede estar vacío."
                );
            }

            Value = value;
        }

        public static OrderId New()
        {
            return new OrderId(Guid.NewGuid());
        }

        public override bool Equals(object? obj)
        {
            return obj is OrderId other && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
