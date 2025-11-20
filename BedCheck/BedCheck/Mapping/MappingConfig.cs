using AutoMapper;
using BedCheck.Models;
using BedCheck.Models.DTOs;

namespace BedCheck.Mapping
{
    public class MappingConfig : Profile
    {

        public MappingConfig() {

            // Le decimos: "Aprende a convertir de Cama a CamaDto y al revés"
            CreateMap<Cama, CamaDto>().ReverseMap();


        
        }
    }
}
