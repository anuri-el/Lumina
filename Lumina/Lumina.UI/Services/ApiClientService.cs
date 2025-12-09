using Lumina.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.UI.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiClientService(string baseUrl = "https://localhost:7001")
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public IImageApiClient Images => new ImageApiClient(_httpClient);
        public ICollageApiClient Collages => new CollageApiClient(_httpClient);
    }
}
