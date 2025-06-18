using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();
        Task<Role?> GetRoleWithPermissionsAsync(int roleId);
        Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
    }
}
