using System.ComponentModel.DataAnnotations;

namespace MusicPlayer.ViewM
{
    public class RecuperarContraseñaUVM
    {
        [Required]
        public string NombreUsuario { get; set; }

        [Required, EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        public string NuevaContrasena { get; set; }

        [Required]
        public string ConfirmarNuevaContrasena { get; set; }
    }
}
