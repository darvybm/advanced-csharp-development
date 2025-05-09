using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string ExtraData { get; set; }
    }
}
