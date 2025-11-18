using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BedCheck.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class UsuariosController : Controller
    {


        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo, UserManager<ApplicationUser> userManager)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var usuarios = _contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value).ToList();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                usuario.NormalizedUserName = roles.FirstOrDefault();
            }

            return View(usuarios); 
        }

        [HttpGet]
        public IActionResult Bloquear(string id) 
        {
            if (id == null) 
            { 
                return NotFound();
            }
            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
