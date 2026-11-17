
namespace Logistics.Domain.ValueObjects
{
    /// <summary>
    /// Representa la dirección de destino de un pedido.
    /// </summary>
    public sealed class DeliveryAddress
    {
        public string Street { get; }

        public string Neighborhood { get; }

        public string City { get; }

        public string Reference { get; }

        public DeliveryAddress(
            string street,
            string neighborhood,
            string city,
            string reference)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("La dirección es obligatoria.");

            if (string.IsNullOrWhiteSpace(neighborhood))
                throw new ArgumentException("El barrio es obligatorio.");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("La ciudad es obligatoria.");

            Street = street.Trim();
            Neighborhood = neighborhood.Trim();
            City = city.Trim();
            Reference = reference?.Trim() ?? string.Empty;
        }

        public override string ToString()
        {
            return $"{Street}, {Neighborhood}, {City}. {Reference}";
        }
    }
}
