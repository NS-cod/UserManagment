using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> IsUsernameExistsAsync(string username, int? excludeUserId = null);
        Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null);
        Task<IEnumerable<User>> GetActiveUsersAsync();
        Task UpdateLastLoginAsync(int userId);
        Task<IEnumerable<User>> SearchUsersAsync(string searchTerm);

    }
}
