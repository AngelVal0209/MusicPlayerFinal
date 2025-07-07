using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.ViewM;
using System.Security.Claims;

namespace MusicPlayer.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDBContext _AppDbContext;

        public UsuarioController(AppDBContext appDBContext)
        {
            _AppDbContext = appDBContext;
        }

        [HttpGet]
        public async Task<IActionResult> EditarPerfil()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login", "Login");
            }

            var usuario = await _AppDbContext.Usuarios.FindAsync(int.Parse(usuarioId));
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Cargar datos actuales en el modelo para mostrar en el formulario
            var modelo = new EditarPerfilUVM
            {
                NombreUsuario = usuario.NombreUsuario,
                CorreoElectronico = usuario.CorreoElectronico
            };

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(EditarPerfilUVM modelo)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return RedirectToAction("Login", "Login");
            }

            var usuario = await _AppDbContext.Usuarios.FindAsync(int.Parse(usuarioId));
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            bool cambios = false;

            // Cambiar nombre de usuario si es distinto y no vacío
            if (!string.IsNullOrWhiteSpace(modelo.NombreUsuario) && modelo.NombreUsuario != usuario.NombreUsuario)
            {
                usuario.NombreUsuario = modelo.NombreUsuario;
                cambios = true;
            }

            // Cambiar correo si es distinto y no vacío
            if (!string.IsNullOrWhiteSpace(modelo.CorreoElectronico) && modelo.CorreoElectronico != usuario.CorreoElectronico)
            {
                usuario.CorreoElectronico = modelo.CorreoElectronico;
                cambios = true;
            }

            // Cambiar contraseña solo si la nueva es válida y la vieja es correcta
            if (!string.IsNullOrWhiteSpace(modelo.NuevaContrasena))
            {
                if (string.IsNullOrWhiteSpace(modelo.ViejaContraseña))
                {
                    ViewData["Mensaje"] = "Debes ingresar tu contraseña actual para cambiarla.";
                    return View(modelo);
                }

                if (modelo.ViejaContraseña != usuario.Contrasena)
                {
                    ViewData["Mensaje"] = "La contraseña actual es incorrecta.";
                    return View(modelo);
                }

                if (modelo.NuevaContrasena == usuario.Contrasena)
                {
                    ViewData["Mensaje"] = "La nueva contraseña no puede ser igual a la anterior.";
                    return View(modelo);
                }

                usuario.Contrasena = modelo.NuevaContrasena;
                cambios = true;
            }

            if (!cambios)
            {
                ViewData["Mensaje"] = "No se detectaron cambios para guardar.";
                return View(modelo);
            }

            try
            {
                await _AppDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = $"Error guardando cambios: {ex.Message}";
                return View(modelo);
            }

            TempData["SuccessMessage"] = "Se actualizó la información correctamente.";
            return RedirectToAction("EditarPerfil");
        }
    }
}
