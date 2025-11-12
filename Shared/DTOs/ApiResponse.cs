using System.Text.Json.Serialization;
using Shared.DTOs;

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