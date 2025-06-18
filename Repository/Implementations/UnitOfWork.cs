using Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Repository.Implementations;
using System;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class UnitOfWork(UserManagementDbContext context, ILoggerFactory loggerFactory, ILogger<UnitOfWork> logger) : IUnitOfWork, IDisposable
    {
        private readonly UserManagementDbContext _context = context;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly ILogger<UnitOfWork> _logger = logger;
        private IDbContextTransaction? _transaction;

        // Repositorios privados
        private IUserRepository? _users;
        private IRoleRepository? _roles;
        private IPermissionRepository? _permissions;
        private IRolePermissionRepository? _rolePermissions;
        private IUserRoleRepository? _userRoles;

        // Propiedades públicas de repositorios
        public IUserRepository Users => _users ??= new UserRepository(_context, _loggerFactory);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context, _loggerFactory);
        public IPermissionRepository Permissions => _permissions ??= new PermissionRepository(_context, _loggerFactory);
        public IRolePermissionRepository RolePermissions => _rolePermissions ??= new RolePermissionRepository(_context, _loggerFactory);
        public IUserRoleRepository UserRoles => _userRoles ??= new UserRoleRepository(_context, _loggerFactory);

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en SaveChangesAsync");
                throw;
            }
        }

        public async Task BeginTransactionAsync()
        {
            try
            {
                if (_transaction == null)
                {
                    _transaction = await _context.Database.BeginTransactionAsync();
                    _logger.LogInformation("Transaction started.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar la transacción");
                throw;
            }
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _context.SaveChangesAsync();
                    await _transaction.CommitAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                    _logger.LogInformation("Transaction committed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al hacer commit en la transacción");
                await RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                    await _transaction.DisposeAsync();
                    _transaction = null;
                    _logger.LogInformation("Transaction rolled back.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al hacer rollback en la transacción");
                throw;
            }
        }

        // Métodos de conveniencia para operaciones comunes
        public async Task<bool> UserHasPermissionAsync(int userId, string resource, string action)
        {
            try
            {
                var userRoles = await UserRoles.GetByUserIdAsync(userId);

                foreach (var userRole in userRoles)
                {
                    var hasPermission = await RolePermissions.HasPermissionAsync(userRole.RoleId, resource, action);
                    if (hasPermission)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verificando permisos para usuario {userId}, recurso {resource}, acción {action}");
                throw;
            }
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId, string? assignedBy = null)
        {
            try
            {
                await BeginTransactionAsync();

                // Verificar que el usuario y rol existen
                var user = await Users.GetByIdAsync(userId);
                var role = await Roles.GetByIdAsync(roleId);

                if (user == null || role == null)
                    throw new ArgumentException("Usuario o rol no encontrado");

                await UserRoles.AssignRoleAsync(userId, roleId, assignedBy);
                await CommitTransactionAsync();

                _logger.LogInformation($"Rol {roleId} asignado a usuario {userId} por {assignedBy}");
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync();
                _logger.LogError(ex, $"Error asignando rol {roleId} a usuario {userId}");
                throw;
            }
        }

        public async Task AssignPermissionToRoleAsync(int roleId, int permissionId, string? assignedBy = null)
        {
            try
            {
                await BeginTransactionAsync();

                // Verificar que el rol y permiso existen
                var role = await Roles.GetByIdAsync(roleId);
                var permission = await Permissions.GetByIdAsync(permissionId);

                if (role == null || permission == null)
                    throw new ArgumentException("Rol o permiso no encontrado");

                await RolePermissions.AssignPermissionAsync(roleId, permissionId, assignedBy);
                await CommitTransactionAsync();

                _logger.LogInformation($"Permiso {permissionId} asignado a rol {roleId} por {assignedBy}");
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync();
                _logger.LogError(ex, $"Error asignando permiso {permissionId} a rol {roleId}");
                throw;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}