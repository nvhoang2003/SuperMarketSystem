using AutoMapper;
using DataAccessLayer.DataObject;
using SuperMarketSystem.DTOs;
using SuperMarketSystem.Models;

namespace SuperMarketSystem
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
