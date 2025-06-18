using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        Task<IEnumerable<UserRole>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserRole>> GetByRoleIdAsync(int roleId);
        Task<bool> HasRoleAsync(int userId, int roleId);
        Task<bool> HasRoleAsync(int userId, string roleName);
        Task AssignRoleAsync(int userId, int roleId, string? assignedBy = null);
        Task RemoveRoleAsync(int userId, int roleId);
        Task RemoveAllUserRolesAsync(int userId);
    }
}
