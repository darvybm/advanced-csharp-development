namespace TaskManagementAPI.DTOs
{
    public class TaskRequest
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string ExtraData { get; set; }
    }
}
