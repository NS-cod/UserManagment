using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IRoleService
    {
        // CRUD básico
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<IEnumerable<Role>> GetActiveRolesAsync();
        Task<Role> CreateAsync(Role role, string? createdBy = null);
        Task<Role> UpdateAsync(Role role, string? modifiedBy = null);
        Task<bool> DeleteAsync(int id, string? deletedBy = null);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);

        //// Operaciones con permisos
        //Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId);
        //Task AssignPermissionToRoleAsync(int roleId, int permissionId, string? assignedBy = null);
        //Task RemovePermissionFromRoleAsync(int roleId, int permissionId, string? removedBy = null);
        //Task RemoveAllPermissionsFromRoleAsync(int roleId, string? removedBy = null);
        //Task<bool> RoleHasPermissionAsync(int roleId, int permissionId);
        //Task<bool> RoleHasPermissionAsync(int roleId, string resource, string action);

        //// Operaciones con usuarios
        //Task<IEnumerable<User>> GetUsersInRoleAsync(int roleId);
        //Task AssignRoleToUserAsync(int roleId, int userId, string? assignedBy = null);
        //Task RemoveRoleFromUserAsync(int roleId, int userId, string? removedBy = null);

        //// Operaciones en lote
        //Task AssignMultiplePermissionsAsync(int roleId, IEnumerable<int> permissionIds, string? assignedBy = null);
        //Task<Role> CreateRoleWithPermissionsAsync(Role role, IEnumerable<int> permissionIds, string? createdBy = null);
    }
}
