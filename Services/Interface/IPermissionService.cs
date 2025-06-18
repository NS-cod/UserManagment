using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IPermissionService
    {
        // CRUD básico
        Task<Permission?> GetByIdAsync(int id);
        Task<Permission?> GetByResourceAndActionAsync(string resource, string action);
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<IEnumerable<Permission>> GetActivePermissionsAsync();
        Task<IEnumerable<Permission>> GetByResourceAsync(string resource);
        Task<Permission> CreateAsync(Permission permission, string? createdBy = null);
        Task<Permission> UpdateAsync(Permission permission, string? modifiedBy = null);
        Task<bool> DeleteAsync(int id, string? deletedBy = null);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByResourceAndActionAsync(string resource, string action);

        // Operaciones con roles
        Task<IEnumerable<Role>> GetRolesWithPermissionAsync(int permissionId);
        Task AssignPermissionToRoleAsync(int permissionId, int roleId, string? assignedBy = null);
        Task RemovePermissionFromRoleAsync(int permissionId, int roleId, string? removedBy = null);

        // Consultas avanzadas
        Task<IEnumerable<Permission>> GetPermissionsByUserAsync(int userId);
        Task<IEnumerable<string>> GetResourcesAsync();
        Task<IEnumerable<string>> GetActionsByResourceAsync(string resource);
        Task<Dictionary<string, List<string>>> GetResourceActionsMapAsync();

        // Operaciones en lote
        Task<IEnumerable<Permission>> CreateMultipleAsync(IEnumerable<Permission> permissions, string? createdBy = null);
        Task<bool> UserHasPermissionAsync(int userId, string resource, string action);
    }
}
