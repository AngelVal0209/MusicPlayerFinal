using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        [Required]
        public string Contrasena { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public DateTime FechaRegistro { get; set; }

        public ICollection<Playlist> Playlists { get; set; }
        public ICollection<Favorito> Favoritos { get; set; }
        public ICollection<HistorialReproduccion> HistorialReproduccion { get; set; }
    }
}
