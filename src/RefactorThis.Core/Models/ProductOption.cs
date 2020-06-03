using System;
using Newtonsoft.Json;

namespace RefactorThis.Core.Models
{
    public class ProductOption
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}