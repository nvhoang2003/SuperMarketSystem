using AutoMapper;
using SuperMarketSystem.DTOs;

namespace SuperMarketSystem
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();
        }
    }
}
