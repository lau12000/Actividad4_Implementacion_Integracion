
namespace Logistics.Domain.ValueObjects
{
    /// <summary>
    /// Representa el identificador único de un mensajero.
    /// </summary>
    public sealed class CourierId
    {
        public Guid Value { get; }

        public CourierId(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException(
                    "El identificador del mensajero no puede estar vacío."
                );
            }

            Value = value;
        }

        public static CourierId New()
        {
            return new CourierId(Guid.NewGuid());
        }

        public override bool Equals(object? obj)
        {
            return obj is CourierId other && Value == other.Value;
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
