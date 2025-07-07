namespace MusicPlayer.Models
{
    public class PlaylistAleatoriaCancion
    {
        public int PlaylistID { get; set; }
        public int CancionID { get; set; }
        public DateTime FechaAgregado { get; set; }

        public PlaylistAleatoria PlaylistAleatoria { get; set; }
        public Cancion Cancion { get; set; }
    }
}
