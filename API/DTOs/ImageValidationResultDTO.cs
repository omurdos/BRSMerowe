using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class ImageValidationResultDTO
    {
        [JsonPropertyName("faceDetected")]
        public bool FaceDetected { get; set; }
        [JsonPropertyName("faceCentered")]

        public bool FaceCentered { get; set; }
        [JsonPropertyName("appropriateHeadSize")]

        public bool AppropriateHeadSize { get; set; }
        [JsonPropertyName("whiteBackground")]

        public bool WhiteBackground { get; set; }
    }
}
