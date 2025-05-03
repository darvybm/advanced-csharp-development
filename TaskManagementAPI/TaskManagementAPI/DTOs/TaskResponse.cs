namespace TaskManagementAPI.DTOs
{
    public class TaskResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public string? ErrorDetail { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static TaskResponse<T> Ok(T data, string message = "Operación exitosa.")
        {
            return new TaskResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static TaskResponse<T> Fail(string message, string? errorDetail = null)
        {
            return new TaskResponse<T>
            {
                Success = false,
                Message = message,
                ErrorDetail = errorDetail
            };
        }
    }
}
