using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;
using TaskManagementAPI.Models.Data;

namespace TaskManagementAPI.Repositories
{
    public class TaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context) => _context = context;

        public async Task<List<TaskModel>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskModel?> GetByIdAsync(Guid id) =>
            await _context.Tasks.FindAsync(id);

        public async Task CreateAsync(TaskModel task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskModel task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
