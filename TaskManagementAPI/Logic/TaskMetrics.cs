using TaskManagementAPI.Models;

public static class TaskMetrics
{
    public static double CalculateCompletionRate(List<TaskModel> tasks)
    {
        if (tasks.Count == 0) return 0;
        var completedCount = tasks.Count(t => t.Status.Equals("Completado", StringComparison.OrdinalIgnoreCase));
        return (double)completedCount / tasks.Count * 100;
    }
}
