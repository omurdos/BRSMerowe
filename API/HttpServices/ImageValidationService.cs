using API.DTOs;
using System.Text;
using System.Text.Json;

namespace API.HttpServices
{
    public class ImageValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ImageValidationService> _logger;

        public ImageValidationService(HttpClient httpClient, ILogger<ImageValidationService> logger)
        {
            _httpClient = httpClient;
            _logger=logger;
        }


        public async Task<bool> ValidateImage(string imageBase64)
        {
            try
            {
                var client = new HttpClient();
                var url = "http://62.164.219.146:8000/image/validate";
                var requestBody = new
                {
                    image_base64 = imageBase64,
                };
                var json = JsonSerializer.Serialize(requestBody);
                client.DefaultRequestHeaders.Add("X-API-Key", "9tZwimURlQMdn8VaI2IAGSFQ4fgbJVd9");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                _logger.LogInformation("Sending image validation request to {Url}", url);
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = JsonSerializer.Deserialize<ImageValidationResultDTO>(responseString);

                    return result != null &&
                           result.FaceDetected &&
                           result.FaceCentered &&
                           result.AppropriateHeadSize &&
                           result.WhiteBackground;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating image");
                return false;
            }
        }




    }
}
