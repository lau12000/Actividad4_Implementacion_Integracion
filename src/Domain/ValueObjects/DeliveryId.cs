
namespace Logistics.Domain.ValueObjects
{
    /// <summary>
    /// Representa el identificador único de una entrega.
    /// </summary>
    public sealed class DeliveryId
    {
        public Guid Value { get; }

        public DeliveryId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(
                    "El identificador de la entrega no puede estar vacío."
                );
            }

            Value = value;
        }

        public static DeliveryId New()
        {
            return new DeliveryId(Guid.NewGuid());
        }

        public override bool Equals(object? obj)
        {
            return obj is DeliveryId other && Value == other.Value;
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
