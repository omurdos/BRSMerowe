using System.Text.Json.Serialization;

namespace Shared.DTOs
{
    public class WhatsAppOTPMessageDTO
    {
        [JsonPropertyName("messaging_product")]  
        public string MessagingProduct { get; set; }
        [JsonPropertyName("recipient_type")]
        public string RecipientType { get; set; }
        public string To { get; set; }
        public string Type { get; set; }
        public Template Template { get; set; }
    }

    public class Template
    {
        public string Name { get; set; }
        public Language Language { get; set; }
        public List<Component> Components { get; set; }
    }

    public class Language
    {
        public string Code { get; set; }
    }

    public class Component
    {
        public string Type { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class Parameter
    {
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
