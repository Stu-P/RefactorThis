using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RefactorThis.Core.Clients
{
    public class MiscClient : IMiscApiClient
    {
        private readonly HttpClient _client;

        public MiscClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<dynamic> GetBooks(int id)
        {
            var response = await _client.GetAsync($"url/{id}");
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
        }
    }
}
