using System;

namespace RefactorThis.Core.Interfaces
{
    public interface IKeyGenerator
    {
        Guid NewKey();
    }
}