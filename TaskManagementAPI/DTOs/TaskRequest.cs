namespace TaskManagementAPI.DTOs
{
    public class TaskRequest
    {
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string? Status { get; set; }
        public string? ExtraData { get; set; }
    }
}
