using Core.Entities;

namespace Dashboard.ViewModel
{

    public class AnnouncementViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public bool IsDisplayed { get; set; }
        public string ImageUrl { get; set; }
        public string FacultyNumber { get; set; }
        public ICollection<Faculty> Faculties { get; set; }
        public string DepartmentNumber { get; set; }
        public ICollection<Department> Departments { get; set; }
        public decimal? ProgramId { get; set; }
        public ICollection<Core.Entities.Program> Programs { get; set; }
        public decimal? BatchId { get; set; }
        public ICollection<Batch> Batches { get; set; }

        public AnnouncementViewModel()
        {
            Title = string.Empty;
            Description = string.Empty;
            Date = string.Empty;
            Time = string.Empty;
            Type = string.Empty;
        }
    }

    public class EditAnnouncementViewModel : AnnouncementViewModel
    {
        public string Id { get; set; }
    }
}
