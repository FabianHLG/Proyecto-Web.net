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

            // Obtener la promoción activa para la ruta de la reserva
            var promocion = _appDbContext.Promociones
                .Where(p => p.RutaId == reserva.Ruta.Id
                            && p.FechaFin >= DateTime.Today)
                .FirstOrDefault();

            if (reserva.EstadoPago == "En Proceso")
            {
                // Convertir el string en una lista de enteros
                var asientosOcupadosInt = reserva.AsientoSeleccionado
                    .Split(',')
                    .Select(asiento => int.Parse(asiento.Trim()))
                    .ToList();

                int PrecioOriginal = reserva.Ruta.Precio * asientosOcupadosInt.Count;

                ViewBag.PrecioOriginal = PrecioOriginal;
                ViewBag.PorcentajeDescuento = promocion.Descuento;
                ViewBag.PromocionActiva = promocion != null ? promocion.Descripcion : null;

                reserva.EstadoPago = "No Pagado";
                _appDbContext.Reservas.Update(reserva);
                _appDbContext.SaveChanges();
            }
            else
            {
                // Calcular el precio total, aplicando el descuento si existe una promoción activa
                decimal precioFinal = reserva.PrecioTotal;

                ViewBag.PrecioOriginal = reserva.PrecioTotal;

                if (promocion != null)
                {
                    precioFinal = (precioFinal * promocion.Descuento);

                    precioFinal = reserva.PrecioTotal - precioFinal;

                    reserva.PrecioTotal = Convert.ToInt32(precioFinal);
                    _appDbContext.Reservas.Update(reserva);
                    _appDbContext.SaveChanges();
                }
                ViewBag.PorcentajeDescuento = promocion.Descuento;
                ViewBag.PromocionActiva = promocion != null ? promocion.Descripcion : null;
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

            HttpContext.Session.SetString("MetodoPago", metodoPago);

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

        [HttpPost]
        public IActionResult VolverPago(int reservaId)
        {
            // Simula la lógica del proceso de pago
            var reserva = _appDbContext.Reservas.FirstOrDefault(r => r.Id == reservaId);
            if (reserva == null)
            {
                return NotFound();
            }

            // Actualiza el estado de la reserva para reflejar que está pagada
            reserva.EstadoPago = "En Proceso";
            _appDbContext.Reservas.Update(reserva);
            _appDbContext.SaveChanges();

            // Redirige a una vista de Procesar el pago
            return RedirectToAction("ProcesarPago", new { reservaId = reserva.Id });
        }
    }
}
