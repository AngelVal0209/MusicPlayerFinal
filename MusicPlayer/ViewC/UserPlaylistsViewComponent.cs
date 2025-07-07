using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Data;
using System.Security.Claims;

using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class UserPlaylistsViewComponent : ViewComponent
{
    private readonly AppDBContext _context;

    public UserPlaylistsViewComponent(AppDBContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var claimsPrincipal = User as ClaimsPrincipal;
        var userIdStr = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdStr == null)
        {
            return View("Empty"); // Vista vacía o mensaje cuando no está autenticado
        }

        int userId = int.Parse(userIdStr);

        var playlists = await _context.Playlists
            .Where(p => p.UsuarioID == userId)
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();

        return View(playlists);
    }
}
