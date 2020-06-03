using System;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Services
{
    public class KeyGenerator : IKeyGenerator
    {
        public Guid NewKey() => Guid.NewGuid();
    }
}