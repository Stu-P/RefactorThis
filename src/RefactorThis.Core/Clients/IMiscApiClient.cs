using System;
using System.Threading.Tasks;

namespace RefactorThis.Core.Clients
{
    public interface IMiscApiClient
    {
        public Task<dynamic> GetBooks(int id);
    }
}
