using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TMS.Domain.Interfaces;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories
{
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly TmsDbContext _context;

        public TaskRepository(TmsDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Task?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetByProjectIdAsync(Guid projectId)
        {
            return await _context.Tasks
                .Include(t => t.AssignedToUser)
                .Include(t => t.Project)
                .Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.AssignedToUserId == userId)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Task>> GetByStatusAsync(Domain.ValueObjects.TaskStatus status)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .Where(t => t.Status == status)
                .OrderBy(t=> t.CreatedAt)
                .ToListAsync();
        }
        public async Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .Where(t => t.DueDate.HasValue && t.DueDate < now && t.Status != Domain.ValueObjects.TaskStatus.Completed)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<Domain.Entities.Task>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedToUser)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(Domain.Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(Domain.Entities.Task task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
    }
}
