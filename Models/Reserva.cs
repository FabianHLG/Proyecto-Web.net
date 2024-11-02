namespace Proyecto_1.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int RutaId { get; set; }
        public string AsientoSeleccionado { get; set; }
        public bool Pagado { get; set; }
    }
}
