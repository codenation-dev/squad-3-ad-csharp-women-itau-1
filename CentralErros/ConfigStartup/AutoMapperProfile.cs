using AutoMapper;
using CentralErros.Api.Models;
using CentralErros.DTO;

namespace CentralErros.ConfigStartup
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Environment, EnvironmentDTO>().ReverseMap();
        }
    }
}
