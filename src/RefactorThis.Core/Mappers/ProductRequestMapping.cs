using AutoMapper;
using RefactorThis.Core.Models;

namespace RefactorThis.Core.Mappers
{
    public class ProductRequestMapping : Profile
    {
        public ProductRequestMapping()
        {
            CreateMap<CreateProductRequest, Product>();
            CreateMap<UpdateProductRequest, Product>();
            CreateMap<CreateProductOptionRequest, ProductOption>();
            CreateMap<UpdateProductOptionRequest, ProductOption>();
        }
    }
}