using API.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HttpServices
{
    public class StudentDetailsService
    {
        private readonly HttpClient _httpClient;

        public StudentDetailsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // public async Task<StudentPaymentDTO> GetStudentPaymentDetailsAsync(int studentId)
        // {
        //     var response = await _httpClient.GetAsync($"api/studentpayments/{studentId}");
        //     response.EnsureSuccessStatusCode();

        //     var content = await response.Content.ReadAsStringAsync();
        //     return JsonSerializer.Deserialize<StudentPaymentDTO>(content, new JsonSerializerOptions
        //     {
        //         PropertyNameCaseInsensitive = true
        //     });
        // }

        public async Task<List<StudentPaymentDTO>> GetStudentDetailsFromApi(string studentId, string bankToken = "TokenForNileBank77")
        {
            try {
                var client = new HttpClient();
                var url = "https://payment.uofs.edu.sd/api/students/details";
                var requestBody = new
                {
                    student_id = studentId,
                    bank_identifier = bankToken
                };
                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseString);

                    if (apiResponse != null && apiResponse.success)
                    {
                        return apiResponse.data.registrations;
                    }
                }

                return null;
            }
            catch (Exception ex) {
                throw ex;
            }
        }


        public async Task<List<StudentInvoiceDTO>> GetStudentInvoices(string studentId, string bankToken = "TokenForNileBank77")
        {
            try
            {
                var client = new HttpClient();
                var url = "https://payment.uofs.edu.sd/api/students/details";
                var requestBody = new
                {
                    student_id = studentId,
                    bank_identifier = bankToken
                };
                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    //var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseString);

                    if (apiResponse != null && apiResponse.success)
                    {
                        return apiResponse.data.payments;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}