using Newtonsoft.Json;

namespace API.DTOs
{
    public class CreatePaymentOrderDTO
    {
        [JsonProperty("cln")]
        public string ClientId { get; set; }
        
        public double Amount { get; set; }

    }
}
