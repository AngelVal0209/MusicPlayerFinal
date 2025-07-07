using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.ViewM
{
    public class PlaylistUVM
    {
        public int PlaylistID { get; set; }

        [Required(ErrorMessage = "El nombre de la playlist es obligatorio.")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Nombre { get; set; }
    }
}
