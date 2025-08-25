namespace TMS.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<Entities.Task?> GetByIdAsync(Guid id);
        Task<IEnumerable<Entities.Task>> GetByProjectIdAsync(Guid projectId);
        Task<IEnumerable<Entities.Task>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Entities.Task>> GetByStatusAsync(ValueObjects.TaskStatus status);
        Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync();
        Task<IEnumerable<Entities.Task>> GetAllAsync();

        Task AddAsync(Entities.Task task);
        Task UpdateAsync(Entities.Task task);
        Task DeleteAsync(Entities.Task task);

        Task<bool> ExistsAsync(Guid id);
    }
}
