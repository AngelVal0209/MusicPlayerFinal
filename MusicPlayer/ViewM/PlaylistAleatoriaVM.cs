namespace MusicPlayer.ViewM
{
    public class PlaylistAleatoriaVM
    {
        public int PlaylistID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Lista de canciones que pertenecen a esta playlist aleatoria
        public List<CancionEnPlaylistViewModel> Canciones { get; set; }
    }

    public class CancionEnPlaylistViewModel
    {
        public int CancionID { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string RutaArchivo { get; set; }
        public int? DuracionSegundos { get; set; }
    }
}
