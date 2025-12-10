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

        public ApiClientService(string baseUrl = "http://localhost:5155")  // ЗМІНЕНО на HTTP
        {
            _baseUrl = baseUrl;

            // Налаштування HttpClient для ігнорування SSL помилок (тільки для розробки!)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(_baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public IImageApiClient Images => new ImageApiClient(_httpClient);
        public ICollageApiClient Collages => new CollageApiClient(_httpClient);
    }
}
