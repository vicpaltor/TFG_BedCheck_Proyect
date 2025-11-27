using AutoMapper;
using BedCheck.AccesoDatos.Data.Repository.IRepository;
using BedCheck.Models;
using BedCheck.Models.DTOs;
using BedCheck.Servicios.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            // Incluimos Cama y Paciente para poder mapear sus nombres
            var lista = _unitOfWork.Operacion.GetAll(includeProperties: "Cama,Paciente");
            return _mapper.Map<IEnumerable<OperacionDto>>(lista);
        }

        public async Task<OperacionDto> ObtenerPorId(int id)
        {
            var obj = _unitOfWork.Operacion.Get(id);
            return _mapper.Map<OperacionDto>(obj);
        }

        public IEnumerable<SelectListItem> ObtenerListaCamasLibres()
        {
            // Solo devolvemos camas que NO estén usadas (CamaUsada == false)
            return _unitOfWork.Cama.GetAll(c => c.CamaUsada == false)
                .Select(i => new SelectListItem
                {
                    Text = i.NombreCama,
                    Value = i.IdCama.ToString()
                });
        }

        public IEnumerable<SelectListItem> ObtenerListaPacientes()
        {
            return _unitOfWork.Paciente.GetAll()
                .Select(i => new SelectListItem
                {
                    Text = i.StrNombrePaciente,
                    Value = i.IdPaciente.ToString()
                });
        }

        public async Task<string> Crear(OperacionDto dto)
        {
            // 1. Validar que la cama esté libre (Doble check de seguridad)
            var cama = _unitOfWork.Cama.Get(dto.CamaId);
            if (cama == null) return "La cama seleccionada no existe.";
            if (cama.CamaUsada) return "La cama seleccionada ya está ocupada.";

            // 2. Mapear
            var entidad = _mapper.Map<Operacion>(dto);

            // 3. Lógica Transaccional: Ocupar la cama
            cama.CamaUsada = true;
            _unitOfWork.Cama.Update(cama); // Marcamos la cama como ocupada

            // 4. Guardar Operación
            _unitOfWork.Operacion.Add(entidad);
            _unitOfWork.Save(); // Guarda ambos cambios (Cama y Operacion) en una transacción (por defecto en EF Core)

            return ""; // Éxito
        }

        public async Task<string> Actualizar(OperacionDto dto)
        {
            var operacionDb = _unitOfWork.Operacion.Get(dto.IdOperacion);
            if (operacionDb == null) return "Operación no encontrada.";

            // LOGICA COMPLEJA: Cambio de cama
            if (operacionDb.CamaId != dto.CamaId)
            {
                // 1. Liberar la cama antigua
                var camaAntigua = _unitOfWork.Cama.Get(operacionDb.CamaId);
                if (camaAntigua != null)
                {
                    camaAntigua.CamaUsada = false;
                    _unitOfWork.Cama.Update(camaAntigua);
                }

                // 2. Ocupar la cama nueva
                var camaNueva = _unitOfWork.Cama.Get(dto.CamaId);
                if (camaNueva == null) return "La nueva cama no existe.";
                if (camaNueva.CamaUsada) return "La nueva cama ya está ocupada.";

                camaNueva.CamaUsada = true;
                _unitOfWork.Cama.Update(camaNueva);
            }

            // Mapear el resto de cambios
            var entidad = _mapper.Map<Operacion>(dto);

            // Truco para EF Core: Desatachar la entidad leída anteriormente para evitar conflictos
            _unitOfWork.Detach(operacionDb);

            _unitOfWork.Operacion.Update(entidad);
            _unitOfWork.Save();
            return "";
        }

        public async Task<bool> Borrar(int id)
        {
            var entidad = _unitOfWork.Operacion.Get(id);
            if (entidad == null) return false;

            // Lógica Transaccional: Al borrar operación, LIBERAMOS la cama
            var cama = _unitOfWork.Cama.Get(entidad.CamaId);
            if (cama != null)
            {
                cama.CamaUsada = false;
                _unitOfWork.Cama.Update(cama);
            }

            _unitOfWork.Operacion.Remove(entidad);
            _unitOfWork.Save();
            return true;
        }
    }
}