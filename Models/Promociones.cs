namespace Proyecto_1.Models
{
    public class Promociones
    {
        public int Id { get; set; }
        public int RutaId { get; set; }
        public decimal Descuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Descripcion { get; set; }
    }
}
