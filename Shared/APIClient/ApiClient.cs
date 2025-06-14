

using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Shared.APIClient{

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://localhost:5001/api/"; // Adjust the base URL as needed
    private readonly ILogger<ApiClient> _logger;
        public ApiClient(HttpClient httpClient, ILogger<ApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public Task DeleteAsync(string uri)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string uri)
    {
            try
            {
                var response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data from {Uri}", uri);
                return default;
            }
    }

        public Task<T> PostAsync<T>(string uri, object data)
        {
            throw new NotImplementedException();
        }

        public Task<T> PutAsync<T>(string uri, object data)
        {
            throw new NotImplementedException();
        }

    }




}