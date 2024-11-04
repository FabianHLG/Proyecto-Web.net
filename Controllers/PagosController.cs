using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;

namespace Proyecto_1.Controllers
{
    public class PagosController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public PagosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult ProcesarPago(int reservaId)
        {
            var reserva = _appDbContext.Reservas.Include(r => r.Ruta).SingleOrDefault(r => r.Id == reservaId);

            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);

        }

        [HttpPost]
        public IActionResult ConfirmarPago(int reservaId, string metodoPago)
        {
            // Simula la lógica del proceso de pago
            var reserva = _appDbContext.Reservas.FirstOrDefault(r => r.Id == reservaId);
            if (reserva == null)
            {
                return NotFound();
            }

            // Actualiza el estado de la reserva para reflejar que está pagada
            reserva.EstadoPago = "Pagado";
            _appDbContext.Reservas.Update(reserva);
            _appDbContext.SaveChanges();

            // Redirige a una vista de confirmación de pago
            return RedirectToAction("ConfirmacionPago", new { reservaId = reserva.Id });
        }

        public IActionResult ConfirmacionPago(int reservaId)
        {
            var reserva = _appDbContext.Reservas.Include(r => r.Ruta).FirstOrDefault(r => r.Id == reservaId);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }
    }
}
