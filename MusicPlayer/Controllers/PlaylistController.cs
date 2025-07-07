using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.ViewM;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MusicPlayer.Controllers
{
    [Authorize]
    public class PlaylistController : Controller
    {
        private readonly AppDBContext _AppDbContext;

        public PlaylistController(AppDBContext appDBContext)
        {
            _AppDbContext = appDBContext;
        }

        private int? ObtenerUsuarioID()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return null;

            return int.Parse(userIdStr);
        }

        [HttpGet]
        public IActionResult CrearPlaylist()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearPlaylist(PlaylistUVM modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var nuevaPlaylist = new Playlist
            {
                Nombre = modelo.Nombre,
                FechaCreacion = DateTime.Now,
                UsuarioID = userId.Value
            };

            _AppDbContext.Playlists.Add(nuevaPlaylist);
            await _AppDbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "¡Playlist creada exitosamente!";
            return RedirectToAction("MisPlaylists");
        }

        [HttpGet]
        public async Task<IActionResult> MisPlaylists(int page = 1, int pageSize = 10)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var query = _AppDbContext.Playlists
                .Where(p => p.UsuarioID == userId)
                .Include(p => p.PlaylistCanciones);

            int totalPlaylists = await query.CountAsync();

            var playlists = await query
                .OrderByDescending(p => p.FechaCreacion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["TotalPlaylists"] = totalPlaylists;
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;

            return View(playlists);
        }

        [HttpGet]
        public async Task<IActionResult> EditarPlaylist(int id)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var playlist = await _AppDbContext.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistID == id && p.UsuarioID == userId);

            if (playlist == null) return NotFound();

            var modelo = new PlaylistUVM
            {
                PlaylistID = playlist.PlaylistID,
                Nombre = playlist.Nombre
            };

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPlaylist(PlaylistUVM modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var playlist = await _AppDbContext.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistID == modelo.PlaylistID && p.UsuarioID == userId);

            if (playlist == null) return NotFound();

            playlist.Nombre = modelo.Nombre;
            await _AppDbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "¡Playlist actualizada exitosamente!";
            return RedirectToAction("MisPlaylists");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarPlaylist(int id)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var playlist = await _AppDbContext.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistID == id && p.UsuarioID == userId);

            if (playlist == null) return NotFound();

            _AppDbContext.Playlists.Remove(playlist);
            await _AppDbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "¡Playlist eliminada exitosamente!";
            return RedirectToAction("MisPlaylists");
        }

        [HttpGet]
        public async Task<IActionResult> VerPlaylist(int id)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var playlist = await _AppDbContext.Playlists
                .Include(p => p.PlaylistCanciones)
                    .ThenInclude(pc => pc.Cancion)
                .FirstOrDefaultAsync(p => p.PlaylistID == id && p.UsuarioID == userId);

            if (playlist == null) return NotFound();

            return View(playlist);
        }

        [HttpGet]
        public async Task<IActionResult> AgregarCancion(int playlistId)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var playlist = await _AppDbContext.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistID == playlistId && p.UsuarioID == userId);

            if (playlist == null) return NotFound();

            var cancionesDisponibles = await _AppDbContext.Canciones
                .Where(c => !_AppDbContext.PlaylistCanciones
                    .Any(pc => pc.PlaylistID == playlistId && pc.CancionID == c.CancionID))
                .ToListAsync();

            ViewData["Playlist"] = playlist;
            return View(cancionesDisponibles);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarCancion(int playlistId, int cancionId)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var playlist = await _AppDbContext.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistID == playlistId && p.UsuarioID == userId);

            if (playlist == null) return NotFound();

            bool yaAgregada = await _AppDbContext.PlaylistCanciones
                .AnyAsync(pc => pc.PlaylistID == playlistId && pc.CancionID == cancionId);

            if (!yaAgregada)
            {
                var nuevaRelacion = new PlaylistCancion
                {
                    PlaylistID = playlistId,
                    CancionID = cancionId,
                    FechaAgregado = DateTime.Now
                };
                _AppDbContext.PlaylistCanciones.Add(nuevaRelacion);
                await _AppDbContext.SaveChangesAsync();
            }

            return RedirectToAction("VerPlaylist", new { id = playlistId });
        }

        [HttpPost]
        public async Task<IActionResult> QuitarCancion(int playlistId, int cancionId)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var relacion = await _AppDbContext.PlaylistCanciones
                .Include(pc => pc.Playlist)
                .FirstOrDefaultAsync(pc => pc.PlaylistID == playlistId
                    && pc.CancionID == cancionId
                    && pc.Playlist.UsuarioID == userId);

            if (relacion == null) return NotFound();

            _AppDbContext.PlaylistCanciones.Remove(relacion);
            await _AppDbContext.SaveChangesAsync();

            return RedirectToAction("VerPlaylist", new { id = playlistId });
        }

        [HttpPost]
        public async Task<IActionResult> SeleccionarPlaylist(int cancionId)
        {
            var userId = ObtenerUsuarioID();
            if (userId == null) return RedirectToAction("Login", "Login");

            var playlists = await _AppDbContext.Playlists
                                .Where(p => p.UsuarioID == userId)
                                .ToListAsync();

            var model = new SeleccionarPlaylistVM
            {
                CancionId = cancionId,
                Playlists = playlists
            };

            return View(model);
        }


    }
}
