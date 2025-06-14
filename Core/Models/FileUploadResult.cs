namespace Core.Models
{
    public class FileUploadResult
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
