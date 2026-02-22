using TMS.Domain.Entities;

namespace TMS.Domain.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId);
        Task<IEnumerable<Project>> GetAllAsync();

        System.Threading.Tasks.Task AddAsync(Project project);
        System.Threading.Tasks.Task UpdateAsync(Project project);
        System.Threading.Tasks.Task DeleteAsync(Project project);
        System.Threading.Tasks.Task<IEnumerable<Project>> GetByUserIdAsync(Guid userId);

        Task<bool> ExistsAsync(Guid id);
    }
}
