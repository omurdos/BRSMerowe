using Core.Entities;

namespace Dashboard.ViewModel
{
    public class PushNotificationViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
    public class CreatePushNotificationViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string SelectedFacultyNumber { get; set; }
        public string SelectedDepartmentNumber { get; set; }
        public List<Faculty> Faculties { get; set; }

    }
    public class CreateIndividualPushNotificationViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string StudentIdentifier { get; set; }

    }
}

