using AutoMapper;
using GeminiEducationAPI.Application.Features.Products.Quaries.GetAllProducts;
using GeminiEducationAPI.Application.Features.Products.Quaries.GetProductById;
using GeminiEducationAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiEducationAPI.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, GetAllProductsDto>();
            CreateMap<Product, GetProductByIdDto>();

        }
    }
}
