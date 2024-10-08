using AutoMapper;
using TaskCSV.DB;
using TaskCSV.DTOs;

namespace CRUDAppBackend.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonDTO, Person>().ReverseMap();
        }
    }
}
