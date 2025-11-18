using BedCheck.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.AccesoDatos.Data.Repository.IRepository
{
    public interface IOperacionRepositorio : IRepository<Operacion>
    {
        void DetachEntity(Operacion operacionDesdeBd);
        void Update(Operacion operacion);
    }
}
