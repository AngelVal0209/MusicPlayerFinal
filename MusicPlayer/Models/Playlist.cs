namespace MusicPlayer.Models
{
    public class Playlist
    {
        public int PlaylistID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int UsuarioID { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<PlaylistCancion> PlaylistCanciones { get; set; }
    }
}
