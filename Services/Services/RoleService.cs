using Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RoleService : IRoleService
    {
        public Task AssignMultiplePermissionsAsync(int roleId, IEnumerable<int> permissionIds, string? assignedBy = null)
        {
            throw new NotImplementedException();
        }

        public Task AssignPermissionToRoleAsync(int roleId, int permissionId, string? assignedBy = null)
        {
            throw new NotImplementedException();
        }

        public Task AssignRoleToUserAsync(int roleId, int userId, string? assignedBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<Role> CreateAsync(Role role, string? createdBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<Role> CreateRoleWithPermissionsAsync(Role role, IEnumerable<int> permissionIds, string? createdBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id, string? deletedBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetActiveRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetUsersInRoleAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllPermissionsFromRoleAsync(int roleId, string? removedBy = null)
        {
            throw new NotImplementedException();
        }

        public Task RemovePermissionFromRoleAsync(int roleId, int permissionId, string? removedBy = null)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRoleFromUserAsync(int roleId, int userId, string? removedBy = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleHasPermissionAsync(int roleId, int permissionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RoleHasPermissionAsync(int roleId, string resource, string action)
        {
            throw new NotImplementedException();
        }

        public Task<Role> UpdateAsync(Role role, string? modifiedBy = null)
        {
            throw new NotImplementedException();
        }
    }
}
