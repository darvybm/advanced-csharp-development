using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;

namespace TaskManagementAPI.Services
{
    public class TaskService
    {
        public readonly TaskRepository _repository;
        public TaskService(TaskRepository repo) => _repository = repo;

        public Task<List<TaskModel>> GetAllAsync() => _repository.GetAllAsync();

        public Task<TaskModel?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

        public Task CreateAsync(TaskModel task) => _repository.CreateAsync(task);

        public Task UpdateAsync(TaskModel task) => _repository.UpdateAsync(task);

        public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
    }
}
