using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRolePermissionRepository : IGenericRepository<RolePermission>
    {
        Task<IEnumerable<RolePermission>> GetByRoleIdAsync(int roleId);
        Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(int permissionId);
        Task<bool> HasPermissionAsync(int roleId, int permissionId);
        Task<bool> HasPermissionAsync(int roleId, string resource, string action);
        Task AssignPermissionAsync(int roleId, int permissionId, string? assignedBy = null);
        Task RemovePermissionAsync(int roleId, int permissionId);
        Task RemoveAllRolePermissionsAsync(int roleId);
    }
}
