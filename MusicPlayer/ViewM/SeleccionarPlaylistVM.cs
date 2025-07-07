using MusicPlayer.Models;

namespace MusicPlayer.ViewM
{
    public class SeleccionarPlaylistVM
    {
        public int CancionId { get; set; }
        public List<Playlist> Playlists { get; set; }
        public int PlaylistSeleccionadaId { get; set; }
    }

}
