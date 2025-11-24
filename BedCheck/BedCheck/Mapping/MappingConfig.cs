using AutoMapper;
using BedCheck.Models;
using BedCheck.Models.DTOs;

namespace BedCheck.Mapping
{
    public class MappingConfig : Profile
    {

        public MappingConfig() {

            CreateMap<Cama, CamaDto>()
                .ForMember(dest => dest.NumeroHabitacion, opt => opt.MapFrom(src => src.Habitacion.NumHabitacion))
                .ReverseMap();

            CreateMap<Habitacion, HabitacionDto>()
                .ForMember(dest => dest.CamasOcupadas, opt => opt.MapFrom(src => src.CamasOcupadas))
                .ReverseMap();

            CreateMap<Paciente, PacienteDto>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.StrNombrePaciente))
                .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.IntEdadPaciente))
                .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => src.StrSexoPaciente))
                .ForMember(dest => dest.Enfermedades, opt => opt.MapFrom(src => src.ListEnfermedades))
                .ForMember(dest => dest.Tratamientos, opt => opt.MapFrom(src => src.ListTratamiento))
                .ReverseMap();

        }
    }
}
