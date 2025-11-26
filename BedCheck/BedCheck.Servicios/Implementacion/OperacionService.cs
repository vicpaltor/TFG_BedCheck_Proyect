using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedCheck.Servicios.Implementacion
{
    public class OperacionService : IOperacionService
    {

        private readonly IContenedorTrabajo _unitOfWork;
        private readonly IMapper _mapper;

        public OperacionService(IContenedorTrabajo unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<OperacionDto>> ObtenerTodas()
        {
            // IMPORTANTE: Incluir Paciente y Cama para poder mostrar sus nombres
            var lista = _unitOfWork.Operacion.GetAll(includeProperties: "Paciente,Cama");
            return _mapper.Map<IEnumerable<OperacionDto>>(lista);
        }

        public async Task<OperacionDto> ObtenerPorId(int id)
        {
            var obj = _unitOfWork.Operacion.Get(id);
            return _mapper.Map<OperacionDto>(obj);
        }

        public async Task<bool> Crear(OperacionDto dto)
        {

            var entidad = _mapper.Map<Operacion>(dto);
            _unitOfWork.Operacion.Add(entidad);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Actualizar(OperacionDto dto)
        {
            var entidad = _mapper.Map<Operacion>(dto);
            _unitOfWork.Operacion.Update(entidad);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> Borrar(int id)
        {
            var entidad = _unitOfWork.Operacion.Get(id);
            if (entidad == null) return false;

            _unitOfWork.Operacion.Remove(entidad);
            _unitOfWork.Save();
            return true;
        }

        // Rellenar Dropdowns
        public IEnumerable<SelectListItem> ObtenerListaPacientes()
        {
            return _unitOfWork.Paciente.GetAll().Select(i => new SelectListItem
            {
                Text = i.StrNombrePaciente,
                Value = i.IdPaciente.ToString()
            });
        }

        public IEnumerable<SelectListItem> ObtenerListaCamas()
        {
            // Opcional: Filtrar solo camas "Libres" o "Disponibles"
            return _unitOfWork.Cama.GetAll(c => c.EstadoCama == "Disponible" || c.EstadoCama == "Libre").Select(i => new SelectListItem
            {
                Text = i.NombreCama,
                Value = i.IdCama.ToString()
            });
        }
    }
}