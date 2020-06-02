using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RefactorThis.Core.Models
{
    public class Product
    {
        public Product()
        {
            Options = new List<ProductOption>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public ICollection<ProductOption> Options { get; set; }
    }
}