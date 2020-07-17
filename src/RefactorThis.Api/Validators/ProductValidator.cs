using System;
using FluentValidation;
using RefactorThis.Core.Models;

namespace RefactorThis.Api.Validators
{
    public class ProductValidator : AbstractValidator<CreateProductRequest>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Price).ExclusiveBetween(0, 15m);
        }
    }
}
