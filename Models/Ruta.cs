namespace Proyecto_1.Models
{
    public class Ruta
    {
        public int Id { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public DateTime Horario { get; set; }
        public int Precio { get; set; }
        public int AsientosDisponibles { get; set; }
    }
}
