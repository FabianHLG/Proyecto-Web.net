namespace Proyecto_1.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int RutaId { get; set; }
        public string AsientoSeleccionado { get; set; }
        public int PrecioTotal { get; set; }
        public DateTime FechaReserva { get; set; }
        public int UsuarioId { get; set; }
        public string EstadoPago { get; set; }

        public virtual Ruta Ruta { get; set; }
    }
}
