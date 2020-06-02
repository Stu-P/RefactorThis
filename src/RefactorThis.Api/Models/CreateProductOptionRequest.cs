using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Api.Models
{
    public class CreateProductOptionRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}