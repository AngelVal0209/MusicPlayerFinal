using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.Data;
using MusicPlayer.Models;
using MusicPlayer.ViewM;

namespace MusicPlayer.Controllers
{
    public class PlaylistAleatoriaController : Controller
    {
        private readonly AppDBContext _context;

        public PlaylistAleatoriaController(AppDBContext context)
        {
            _context = context;
        }

        // Muestra las playlists aleatorias al inicio
        public async Task<IActionResult> Inicio()
        {
            DateTime hoy = DateTime.Today;

            // Buscar playlists aleatorias del día de hoy
            var playlistsDelDia = await _context.PlaylistAleatorias
                .Include(p => p.PlaylistAleatoriaCanciones)
                    .ThenInclude(pac => pac.Cancion)
                .Where(p => p.FechaCreacion == hoy)
                .ToListAsync();

            // Si no existen, generarlas y guardarlas
            if (!playlistsDelDia.Any())
            {
                playlistsDelDia = await GenerarPlaylistsAleatoriasParaHoy(hoy);
            }

            // Mapear a ViewModel para la vista
            var playlistsVM = playlistsDelDia.Select(p => new PlaylistAleatoriaVM
            {
                PlaylistID = p.PlaylistID,
                Nombre = p.Nombre,
                FechaCreacion = p.FechaCreacion,
                Canciones = p.PlaylistAleatoriaCanciones.Select(pac => new CancionEnPlaylistViewModel
                {
                    CancionID = pac.Cancion.CancionID,
                    Titulo = pac.Cancion.Titulo,
                    Artista = pac.Cancion.Artista,
                    RutaArchivo = pac.Cancion.RutaArchivo,
                    DuracionSegundos = pac.Cancion.DuracionSegundos
                }).ToList()
            }).ToList();

            return View(playlistsVM);
        }
        private async Task<List<PlaylistAleatoria>> GenerarPlaylistsAleatoriasParaHoy(DateTime fecha)
        {
            var nuevasPlaylists = new List<PlaylistAleatoria>();

            for (int i = 0; i < 4; i++)
            {
                var canciones = await _context.Canciones
                    .OrderBy(c => Guid.NewGuid())  // aleatoriza
                    .Take(10)
                    .ToListAsync();

                var playlist = new PlaylistAleatoria
                {
                    Nombre = $"Playlist Aleatoria {i + 1} - {fecha.ToString("dd/MM/yyyy")}",
                    FechaCreacion = fecha,
                    PlaylistAleatoriaCanciones = canciones.Select(c => new PlaylistAleatoriaCancion
                    {
                        CancionID = c.CancionID
                    }).ToList()
                };

                nuevasPlaylists.Add(playlist);
            }

            _context.PlaylistAleatorias.AddRange(nuevasPlaylists);
            await _context.SaveChangesAsync();

            return nuevasPlaylists;
        }
        // Acción para mostrar detalle de playlist aleatoria con sus canciones
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var playlist = await _context.PlaylistAleatorias
                .Include(p => p.PlaylistAleatoriaCanciones)
                    .ThenInclude(pac => pac.Cancion)
                .FirstOrDefaultAsync(p => p.PlaylistID == id);

            if (playlist == null)
            {
                return NotFound();
            }

            var vm = new PlaylistAleatoriaVM
            {
                PlaylistID = playlist.PlaylistID,
                Nombre = playlist.Nombre,
                FechaCreacion = playlist.FechaCreacion,
                Canciones = playlist.PlaylistAleatoriaCanciones.Select(pac => new CancionEnPlaylistViewModel
                {
                    CancionID = pac.Cancion.CancionID,
                    Titulo = pac.Cancion.Titulo,
                    Artista = pac.Cancion.Artista,
                    RutaArchivo = pac.Cancion.RutaArchivo,
                    DuracionSegundos = pac.Cancion.DuracionSegundos
                }).ToList()
            };

            return View(vm);
        }

    }
}
