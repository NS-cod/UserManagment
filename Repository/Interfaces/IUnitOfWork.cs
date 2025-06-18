using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositorios
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IPermissionRepository Permissions { get; }
        IRolePermissionRepository RolePermissions { get; }
        IUserRoleRepository UserRoles { get; }

        // Operaciones de persistencia
        Task<int> SaveChangesAsync();

        // Manejo de transacciones  
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Métodos de conveniencia para operaciones comunes
        Task<bool> UserHasPermissionAsync(int userId, string resource, string action);
        Task AssignRoleToUserAsync(int userId, int roleId, string? assignedBy = null);
        Task AssignPermissionToRoleAsync(int roleId, int permissionId, string? assignedBy = null);
    }
}
