namespace TaskManagementAPI.Models.Factory
{
    public static class TaskFactory
    {
        public static TaskModel CreateByPriority(string description, string prioridad)
        {
            var now = DateTime.Now;

            return prioridad.ToLower() switch
            {
                "alta" => new TaskModel
                {
                    Id = Guid.NewGuid(),
                    Description = description,
                    DueDate = now.AddDays(1),
                    Status = "urgente",
                    ExtraData = "Creada con prioridad alta"
                },

                "media" => new TaskModel
                {
                    Id = Guid.NewGuid(),
                    Description = description,
                    DueDate = now.AddDays(5),
                    Status = "pendiente",
                    ExtraData = "Creada con prioridad media"
                },

                "baja" => new TaskModel
                {
                    Id = Guid.NewGuid(),
                    Description = description,
                    DueDate = now.AddDays(10),
                    Status = "normal",
                    ExtraData = "Creada con prioridad baja"
                },

                _ => new TaskModel
                {
                    Id = Guid.NewGuid(),
                    Description = description,
                    DueDate = now.AddDays(7),
                    Status = "pendiente",
                    ExtraData = $"Prioridad personalizada: {prioridad}"
                }
            };
        }

        public static TaskModel CreateCustomTask(string description, DateTime dueDate, string status, string? extra = null)
        {
            return new TaskModel
            {
                Id = Guid.NewGuid(),
                Description = description,
                DueDate = dueDate,
                Status = status,
                ExtraData = extra
            };
        }
    }
}
