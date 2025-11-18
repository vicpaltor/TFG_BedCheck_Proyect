using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BedCheck.Areas.Empleado.Controllers
{
    [Authorize] // Esto requiere que el usuario esté autenticado para acceder al controlador.
    [Area("Empleado")]
    public class HomeController : Controller
    {

        private readonly IContenedorTrabajo _contenedorTrabajo;

        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                ListaHabitaciones = _contenedorTrabajo.Habitacion.GetAll(),
                ListaCamas = _contenedorTrabajo.Cama.GetAll(),
                ListaOperaciones = _contenedorTrabajo.Operacion.GetAll()

            };

            return View(homeVM);
        }

        [HttpGet]
        public IActionResult Detalles(int id)
        {
            var operacionExistente = _contenedorTrabajo.Operacion.GetFirstOrDefault(o => o.CamaId == id);

            if (operacionExistente != null)
            {
                return RedirectToAction("Edit", "Operaciones", new { area = "Admin", id = operacionExistente.IdOperacion });
            }
            else
            {
                OperacionVM operacionVM = new OperacionVM()
                {
                    Operacion = new Operacion()
                    {
                        CamaId = id // Preseleccionar la cama con el ID
                    },
                    ListaCamas = _contenedorTrabajo.Cama.GetListaOperaciones(),
                    ListaPacientes = _contenedorTrabajo.Paciente.GetListaOperaciones()
                };
                return RedirectToAction("Create", "Operaciones", new { area = "Admin", operacion = operacionVM , id = id });
                //return View("~/Areas/Admin/Views/Operaciones/Create.cshtml", operacionVM);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
