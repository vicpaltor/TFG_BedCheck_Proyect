using BedCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        //Aqui se deben de ir agregando los diferentes repositorios
        IHabitacionRepositorio Habitacion { get; }
        ICamaRepositorio Cama { get; }
        IEnfermeroRepositorio Enfermero{ get; }
        IPacienteRepositorio Paciente { get; }
        IOperacionRepositorio Operacion { get; }

        IUsuarioRepository Usuario { get; }

        void Detach(Cama camaDesdeBd);
        void Save();

    }
}
