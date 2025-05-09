using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Models.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskModel> Tasks { get; set; }
    }
}
