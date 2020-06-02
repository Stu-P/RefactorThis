using System.ComponentModel.DataAnnotations;

namespace RefactorThis.Api.Models
{
    public class CreateProductRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 9999999999999999.99)]
        public decimal? Price { get; set; }

        [Required]
        [Range(0, 9999999999999999.99)]
        public decimal? DeliveryPrice { get; set; }
    }
}