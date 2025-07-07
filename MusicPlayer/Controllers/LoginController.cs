using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Data;
using MusicPlayer.Models;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.ViewM;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MusicPlayer.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDBContext _AppDbContext;
        public LoginController(AppDBContext appDBContext) { 
        _AppDbContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroUVM modelo)
        {
            // verificamos contraseña
            if (modelo.Contrasena != modelo.ConfirmarContraseña)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                return View(modelo);
            }

            // verificacmos modelos
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }
            // Verificamos correo
            bool correoEnUso = await _AppDbContext.Usuarios
                .AnyAsync(u => u.CorreoElectronico == modelo.CorreoElectronico);

            if (correoEnUso)
            {
                ViewData["Mensaje"] = "El correo electrónico ya está en uso.";
                return View(modelo);
            }
     
            Usuario usuario = new Usuario
            {
                NombreUsuario = modelo.NombreUsuario,
                CorreoElectronico = modelo.CorreoElectronico,
                Contrasena = modelo.Contrasena, 
                FechaNacimiento = modelo.FechaNacimiento,
                Genero = modelo.Genero
            };

            // Guardar usuario en base de datos
            await _AppDbContext.Usuarios.AddAsync(usuario);
            await _AppDbContext.SaveChangesAsync();

            // Comprobar si el usuario fue guardado correctamente
            if (usuario.UsuarioID != 0)
            {
                
                TempData["SuccessMessage"] = "Te registraste correctamente. ¡Bienvenido!";
                return RedirectToAction("Login", "Login");
            }
   
            ViewData["Mensaje"] = "Hubo un error al registrar el usuario.";
            return View(modelo);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUVM modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario_encontrado = await _AppDbContext.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.CorreoElectronico == modelo.CorreoElectronico &&
                    u.Contrasena == modelo.Contrasena);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "Correo o contraseña incorrectos.";
                return View(modelo);
            }

       
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario),
        new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.UsuarioID.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false, // recordar
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Inicio", "PlaylistAleatoria");
        }


        [HttpGet]
        public IActionResult RecuperarContraseña()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RecuperarContraseña(RecuperarContraseñaUVM modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = await _AppDbContext.Usuarios
                .Where(u => u.NombreUsuario == modelo.NombreUsuario && u.CorreoElectronico == modelo.CorreoElectronico && u.FechaNacimiento == modelo.FechaNacimiento)
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias para los datos proporcionados.";
                return View(modelo);
            }

            if (modelo.NuevaContrasena != modelo.ConfirmarNuevaContrasena)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden.";
                return View(modelo);
            }

            if (string.IsNullOrWhiteSpace(modelo.NuevaContrasena))
            {
                ViewData["Mensaje"] = "La nueva contraseña no puede estar vacía.";
                return View(modelo);
            }

            usuario.Contrasena = modelo.NuevaContrasena;
            _AppDbContext.Entry(usuario).Property(u => u.Contrasena).IsModified = true;

            try
            {
                await _AppDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear ex.Message para depurar
                ViewData["Mensaje"] = "Ocurrió un error al actualizar la contraseña.";
                return View(modelo);
            }

            ViewData["Success"] = true;
            ViewData["MensajeExito"] = "Tu contraseña ha sido restablecida correctamente. Serás redirigido en 5 segundos.";
            return View();
        }

    }
}
