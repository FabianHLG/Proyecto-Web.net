using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_1.Data;
using Proyecto_1.Models;

namespace Proyecto_1.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public UsuarioController(AppDbContext context)
        {
            _appDbContext = context;
        }

        // Acción para la vista de registro
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el email ya existe en la base de datos
                var existingUser = _appDbContext.Usuario.SingleOrDefault(u => u.Email == usuario.Email);

                if (existingUser != null)
                {
                    // Si el correo ya está registrado, se agrega un mensaje de error
                    ModelState.AddModelError("Email", "El correo electrónico ya está registrado. Por favor, usa otro correo.");
                    return View(usuario);
                }

                usuario.FechaRegistro = DateTime.Now;
                usuario.Activo = true; // Se establece el usuario como activo automáticamente

                _appDbContext.Usuario.Add(usuario);
                _appDbContext.SaveChanges();

                return RedirectToAction("Login", "Usuario");
            }

            return View(usuario);
        }


        // Acción para la vista de inicio de sesión
        public IActionResult Login()
        {
            return View();
        }

        // Procesa el inicio de sesión
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Busca al usuario en la base de datos
            var user = _appDbContext.Usuario.SingleOrDefault(u => u.Email == email && u.Contraseña == password);

            if (user != null)
            {
                // Autenticación exitosa: guarda el Id en la sesión
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("UserNombre", user.Nombre);

                // Redirige a la página principal
                return RedirectToAction("Index", "Búsqueda");
            }

            // Manejo de error en caso de credenciales incorrectas
            ModelState.AddModelError("", "Correo o contraseña incorrectos");
            return View();
        }

        // Acción para la vista de perfil
        public IActionResult Perfil()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = _appDbContext.Usuario.Find(int.Parse(userId));
            return View(user);
        }

        // Procesa la edición del perfil de usuario
        [HttpPost]
        public IActionResult EditPerfil(Usuario updatedUser)
        {
            if (ModelState.IsValid)
            {
                // Encuentra al usuario en la base de datos
                var user = _appDbContext.Usuario.SingleOrDefault(u => u.Id == updatedUser.Id);

                if (user != null)
                {
                    // Actualiza las propiedades del usuario con los nuevos valores
                    user.Nombre = updatedUser.Nombre;
                    user.Email = updatedUser.Email;

                    // Marca el usuario como actualizado
                    _appDbContext.Usuario.Update(user);

                    // Guarda los cambios en la base de datos
                    _appDbContext.SaveChanges();

                    // Actualiza el nombre del usuario en la sesión
                    HttpContext.Session.SetString("UserNombre", user.Nombre);

                    return RedirectToAction("Perfil");
                }
            }
            return View("Perfil", updatedUser);
        }

    }
}
