using AutoMapper;
using DataAccessLayer.DataObject;
using SuperMarketSystem.DTOs;

namespace SuperMarketSystem
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CreateCategoryDTO, Category>().ReverseMap();
        }
    }
}
