using API.DTOs;
using System.Text.Json.Serialization;

public class ApiResponse
{
    public bool success { get; set; }
    public int code { get; set; }
    public ApiData data { get; set; }
}

public class ApiData
{
    [JsonPropertyName("registrations")]
    public List<StudentPaymentDTO> registrations { get; set; }
    [JsonPropertyName("payments")]
    public List<StudentInvoiceDTO> payments { get; set; }
}