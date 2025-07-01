using TMS.Domain.Entities;

namespace TMS.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();

        System.Threading.Tasks.Task AddAsync(User user);
        System.Threading.Tasks.Task UpdateAsync(User user);
        System.Threading.Tasks.Task DeleteAsync(User user);

        Task<bool> ExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
    }
}
