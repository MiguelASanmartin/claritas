namespace TMS.Domain.Interfaces
{
    public interface ITaskRepository
    {
        Task<Entities.Task> GetByIdAsync(Guid id);
        Task<Entities.Task> GetByProjectIdAsync(Guid projectId);
        Task<Entities.Task> GetByUserIdAsync(Guid userId);
        Task<Entities.Task> GetByStatusAsync(TaskStatus status);
        Task<Entities.Task> GetOverdueTasksAsync();
        Task<IEnumerable<Entities.Task>> GetAllAsync();

        Task AddAsync(Entities.Task task);
        Task UpdateAsync(Entities.Task task);
        Task DeleteAsync(Entities.Task task);

        Task<bool> ExistsAsync(Guid id);
    }
}
