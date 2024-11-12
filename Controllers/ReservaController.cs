using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;
using Proyecto_1.Models;

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

            // Obtener los asientos ocupados de la base de datos de reservas

            var asientosOcupados = _appDbContext.Reservas
                .Where(r => r.RutaId == rutaId)
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

        public IActionResult Book(int rutaId, List<int> asientos)
        {
            // Verificar si el usuario ha iniciado sesión
            var userId = HttpContext.Session.GetString("UserId");

            if (userId == null)
            {
                // Redirige al usuario al inicio de sesión si no está autenticado
                return RedirectToAction("Login", "Usuario");
            }

            // Lógica de reserva
            var ruta = _appDbContext.Rutas.Find(rutaId);
            if (ruta == null || asientos == null || asientos.Count == 0)
            {
                TempData["ErrorData"] = "UPS!! ALGO ESTA VACIO";
                return RedirectToAction("Index", "Búsqueda");
            }

            // Crear un objeto de reserva para almacenar los detalles en la base de datos
            var reserva = new Reserva
            {
                RutaId = rutaId,
                AsientoSeleccionado = string.Join(",", asientos),
                PrecioTotal = asientos.Count * ruta.Precio, // Calcula el precio total
                FechaReserva = DateTime.Now,
                UsuarioId = int.Parse(userId), // ID del usuario en sesión
                EstadoPago = "No Pagado", // Inicialmente, el estado de pago es "No Pagado"
            };
            // Encuentra la ruta en la base de datos

            if (ruta != null)
            {
                // Actualiza las propiedades del usuario con los nuevos valores
                ruta.Origen = ruta.Origen;
                ruta.Destino = ruta.Destino;
                ruta.Precio = ruta.Precio;
                ruta.AsientosDisponibles = ruta.AsientosDisponibles - asientos.Count;

                // Guarda los cambios en la base de datos
                _appDbContext.SaveChanges();
            }

            _appDbContext.Reservas.Add(reserva);
            _appDbContext.SaveChanges();

            // Redirigir al controlador de Pagos para procesar el pago
            return RedirectToAction("ProcesarPago", "Pagos", new { reservaId = reserva.Id });
        }
    }
}
