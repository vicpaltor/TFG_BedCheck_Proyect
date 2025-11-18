using BedCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.AccesoDatos.Data.Repository.IRepository
{
    public interface IEnfermeroRepositorio : IRepository<Enfermero>
    {
        void Update(Enfermero enfermero);
    }
}
