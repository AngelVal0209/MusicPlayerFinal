namespace MusicPlayer.Models
{
    public class PlaylistAleatoria
    {
        public int PlaylistID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

        public ICollection<PlaylistAleatoriaCancion> PlaylistAleatoriaCanciones { get; set; }
    }
}
