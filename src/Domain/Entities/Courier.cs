
using Logistics.Domain.ValueObjects;

namespace Logistics.Domain.Entities
{
    /// <summary>
    /// Representa un mensajero de la plataforma logística.
    /// </summary>
    public class Courier
    {
        public CourierId Id { get; private set; }

        public string Name { get; private set; }

        public bool IsAvailable { get; private set; }

        public Courier(CourierId id, string name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "El nombre del mensajero es obligatorio."
                );
            }

            Name = name;
            IsAvailable = true;
        }

        /// <summary>
        /// Marca al mensajero como disponible.
        /// </summary>
        public void SetAvailable()
        {
            IsAvailable = true;
        }

        /// <summary>
        /// Marca al mensajero como no disponible.
        /// </summary>
        public void SetUnavailable()
        {
            IsAvailable = false;
        }
    }
}
