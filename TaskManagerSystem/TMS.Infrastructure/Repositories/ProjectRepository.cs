using Microsoft.EntityFrameworkCore;
using TMS.Domain.Entities;
using TMS.Domain.Interfaces;
using TMS.Infrastructure.Data;

namespace TMS.Infrastructure.Repositories
{
    public sealed class ProjectRepository : IProjectRepository
    {
        private readonly TmsDbContext _context;

        public ProjectRepository(TmsDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .Where(p => p.OwnerId == ownerId)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Tasks)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }
    }
}
