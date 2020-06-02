using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Api.Models
{
    public class UpdateProductOptionRequest : CreateProductOptionRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}