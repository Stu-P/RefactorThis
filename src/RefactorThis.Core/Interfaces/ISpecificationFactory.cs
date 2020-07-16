using RefactorThis.Core.Models;

namespace RefactorThis.Core.Interfaces
{
    public interface ISpecificationFactory<T>
    {
        ISpecification<T> Create(SearchCritera searchCritera);
    }
}
