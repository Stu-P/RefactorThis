using System.Collections.Generic;

namespace RefactorThis.Api.Models
{
    public class GetAllResponse<T>
    {
        public GetAllResponse()
        {
            Items = new List<T>();
        }

        public GetAllResponse(IEnumerable<T> items)
        {
            Items = items;
        }

        public IEnumerable<T> Items { get; }
    }
}