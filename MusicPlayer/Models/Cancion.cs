namespace MusicPlayer.Models
{
    public class Cancion
    {
        public int CancionID { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string RutaArchivo { get; set; }
        public int? DuracionSegundos { get; set; }
        public DateTime FechaAgregado { get; set; }

        public ICollection<PlaylistCancion> PlaylistCanciones { get; set; }
        public ICollection<Favorito> Favoritos { get; set; }
        public ICollection<HistorialReproduccion> HistorialReproduccion { get; set; }
        public ICollection<PlaylistAleatoriaCancion> PlaylistAleatoriaCanciones { get; set; }
    }
}
