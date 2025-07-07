using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.ViewM;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Claims;

namespace MusicPlayer.Controllers
{
    [Authorize]
    public class CancionController : Controller
    {
        private readonly AppDBContext _context;

        public CancionController(AppDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Buscar(string? query)
        {
            var canciones = _context.Canciones.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                canciones = canciones.Where(c =>
                    c.Titulo.Contains(query) || c.Artista.Contains(query));
            }

            var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                return Forbid();
            }

            var playlists = await _context.Playlists
                .Where(p => p.UsuarioID == userId)
                .ToListAsync();

            var modelo = new CancionBusquedaVM
            {
                Query = query,
                Resultados = await canciones.ToListAsync(),
                PlaylistsUsuario = playlists
            };

            return View(modelo);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarCancion(int playlistId, int cancionId, string? query)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdString, out int userId))
            {
                return Forbid();
            }

            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistID == playlistId && p.UsuarioID == userId);

            if (playlist == null)
            {
                TempData["Error"] = "No tienes permiso para modificar esta playlist.";
                return RedirectToAction(nameof(Buscar), new { query });
            }

            var cancion = await _context.Canciones.FindAsync(cancionId);
            if (cancion == null)
            {
                TempData["Error"] = "La canción no existe.";
                return RedirectToAction(nameof(Buscar), new { query });
            }

            bool yaAgregada = await _context.PlaylistCanciones
                .AnyAsync(pc => pc.PlaylistID == playlistId && pc.CancionID == cancionId);

            if (yaAgregada)
            {
                TempData["Error"] = "La canción ya está en la playlist.";
                return RedirectToAction(nameof(Buscar), new { query });
            }

            var playlistCancion = new PlaylistCancion
            {
                PlaylistID = playlistId,
                CancionID = cancionId,
                FechaAgregado = DateTime.Now
            };

            _context.PlaylistCanciones.Add(playlistCancion);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Canción '{cancion.Titulo}' agregada a playlist '{playlist.Nombre}'.";

            return RedirectToAction(nameof(Buscar), new { query });
        }
    }
}
