using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Core.Models
{
    public class UpdateProductRequest : CreateProductRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}