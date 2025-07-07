using MusicPlayer.Models;
using System.Collections.Generic;

namespace MusicPlayer.ViewM
{
    public class CancionBusquedaVM
    {
        public string? Query { get; set; }
        public List<Cancion> Resultados { get; set; } = new();

        // Playlists del usuario para el dropdown
        public List<Playlist> PlaylistsUsuario { get; set; } = new();
    }
}
