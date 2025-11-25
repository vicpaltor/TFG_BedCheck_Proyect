using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Data;
using BedCheck.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BedCheck.AccesoDatos.Data.Repository
{
    internal class PacienteRepository : Repository<Paciente>, IPacienteRepositorio
    {
        private readonly ApplicationDbContext _db;
        public PacienteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetListaOperaciones()
        {
            return _db.Paciente.Select(c => new SelectListItem()
            {
                Text = c.StrNombrePaciente,
                Value = c.IdPaciente.ToString()
            });
        }
        public void Update(Paciente paciente)
        {
            var pacienteDesdeBb = _db.Paciente.FirstOrDefault(s => s.IdPaciente == paciente.IdPaciente);
            if (pacienteDesdeBb == null)
            {
                throw new Exception("No se encontró el paciente con el Id proporcionado.");
            }
            pacienteDesdeBb.StrNombrePaciente = paciente.StrNombrePaciente;
            pacienteDesdeBb.IntEdadPaciente = paciente.IntEdadPaciente;
            pacienteDesdeBb.StrSexoPaciente = paciente.StrSexoPaciente;
            pacienteDesdeBb.ListEnfermedades = paciente.ListEnfermedades;
            pacienteDesdeBb.ListTratamiento = paciente.ListTratamiento;
        }
    }
}
