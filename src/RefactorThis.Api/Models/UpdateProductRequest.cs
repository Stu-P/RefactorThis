using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Api.Models
{
    public class UpdateProductRequest : CreateProductRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}