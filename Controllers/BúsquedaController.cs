using Microsoft.AspNetCore.Mvc;
using Proyecto_1.Data;

namespace Proyecto_1.Controllers
{
    public class BúsquedaController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BúsquedaController(AppDbContext context)
        {
            _appDbContext = context;
        }

        public IActionResult Index()
        {
            var rutasDisponibles = _appDbContext.Rutas.Where(r => r.Horario.Date >= DateTime.Today).ToList();

            return View(rutasDisponibles);
        }

        [HttpGet]
        public IActionResult Busqueda(string origen, string destino, DateTime fecha)
        {
            var rutas = _appDbContext.Rutas
                .Where(r => r.Origen == origen && r.Destino == destino && r.Horario.Date == fecha.Date)
                .ToList();

            return View(rutas);
        }

        public IActionResult Detalles(int id)
        {
            var ruta = _appDbContext.Rutas.Find(id);

            // Obtener los asientos ocupados de la base de datos de reservas

            var asientosOcupados = _appDbContext.Reservas
                .Where(r => r.RutaId == id)
                .Select(r => r.AsientoSeleccionado)
                .ToList();

            // Convertir la lista de strings a una lista de enteros
            var asientosOcupadosInt = asientosOcupados
                .SelectMany(asiento => asiento.Split(','))
                .Select(asiento => int.Parse(asiento.Trim()))
                .ToList();

            ViewBag.AsientosOcupados = asientosOcupadosInt;

            return View(ruta);
        }
    }
}
