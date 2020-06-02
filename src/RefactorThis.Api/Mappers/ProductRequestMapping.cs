using AutoMapper;
using RefactorThis.Api.Models;
using RefactorThis.Core.Models;

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