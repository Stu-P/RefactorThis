using System;
using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Core.Models
{
    public class UpdateProductOptionRequest : CreateProductOptionRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}