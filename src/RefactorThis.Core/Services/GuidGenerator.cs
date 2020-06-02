using System;
using RefactorThis.Core.Interfaces;

namespace RefactorThis.Core.Services
{
    public class GuidGenerator : IGuidGenerator
    {
        public Guid NewGuid() => Guid.NewGuid();
    }
}