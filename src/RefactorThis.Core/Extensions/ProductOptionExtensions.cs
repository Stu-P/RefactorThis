using RefactorThis.Core.Models;

namespace RefactorThis.Core.Extensions
{
    public static class ProductOptionExtensions
    {
        public static void MapChanges(this ProductOption initial, ProductOption updates)
        {
            initial.Name = updates.Name;
            initial.Description = updates.Description;
        }
    }
}