using Autoglass.Data.Entities;
using Autoglass.Services.Models;
using AutoMapper;

namespace Autoglass.Services.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
        }
    }
}
