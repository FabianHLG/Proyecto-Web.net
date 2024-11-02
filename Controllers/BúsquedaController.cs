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
            return View();
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
            return View(ruta);
        }
    }
}
