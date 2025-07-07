using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicPlayer.ViewM
{
    public class AgregarCancionVM
    {
        public int CancionID { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public int? DuracionSegundos { get; set; }
        public DateTime FechaAgregado { get; set; }

        // Para el dropdown de playlists del usuario
        public List<SelectListItem> PlaylistsDisponibles { get; set; }

        // Seleccionada por el usuario en el formulario
        public int PlaylistSeleccionadaID { get; set; }
    }
}
