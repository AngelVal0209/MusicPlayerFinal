namespace MusicPlayer.Models
{
    public class HistorialReproduccion
    {
        public int HistorialID { get; set; }
        public int UsuarioID { get; set; }
        public int CancionID { get; set; }
        public DateTime FechaReproduccion { get; set; }

        public Usuario Usuario { get; set; }
        public Cancion Cancion { get; set; }
    }
}
