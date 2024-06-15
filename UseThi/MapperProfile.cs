using AutoMapper;
using Data.Data.Entities;
using UseThi.Models;

namespace UseThi
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {
            CreateMap<Product, ProductCartModel>();
        }
    }
}
