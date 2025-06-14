using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class NewsDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ShortDescription { get; set; }
    }

    public class CreateNewsDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string ShortDescription { get; set; }
    }

    public class UpdateNewsDTO
    {
        [Required]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ShortDescription { get; set; }
    }
}
