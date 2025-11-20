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



        }
    }
}
