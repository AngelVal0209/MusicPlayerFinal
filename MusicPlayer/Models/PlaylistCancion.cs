namespace MusicPlayer.Models
{
    public class PlaylistCancion
    {
        public int PlaylistID { get; set; }
        public int CancionID { get; set; }
        public DateTime FechaAgregado { get; set; }

        public Playlist Playlist { get; set; }
        public Cancion Cancion { get; set; }
    }
}
