using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;

namespace Proyecto_1.Controllers
{
    public class ReservaController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ReservaController(AppDbContext context)
        {
            _appDbContext = context;
        }

        public IActionResult Index(int rutaId)
        {
            var ruta = _appDbContext.Rutas.FirstOrDefault(r => r.Id == rutaId);

            if (ruta == null)
            {
                return NotFound();
            }

            return View(ruta);
        }

    }
}
