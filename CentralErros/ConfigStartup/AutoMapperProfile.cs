using AutoMapper;
using CentralErros.Models;
using CentralErros.DTO;
using CentralErros.Services;

namespace CentralErros.ConfigStartup
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Environment, EnvironmentDTO>().ReverseMap();
            CreateMap<Level, LevelDTO>().ReverseMap();
            CreateMap<ErrorOccurrence, ErrorOccurrenceDTO>().ReverseMap();
        }
    }
}
