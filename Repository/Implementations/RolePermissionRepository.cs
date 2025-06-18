using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class RolePermissionRepository(UserManagementDbContext context, ILoggerFactory loggerFactory) : GenericRepository<RolePermission>(context, loggerFactory.CreateLogger<GenericRepository<RolePermission>>()), IRolePermissionRepository
    {
        public async Task<IEnumerable<RolePermission>> GetByRoleIdAsync(int roleId)
        {
            try
            {
                return await _dbSet
                    .Where(rp => rp.IsActive && rp.RoleId == roleId)
                    .Include(rp => rp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByRoleIdAsync con roleId={roleId}");
                throw;
            }
        }

        public async Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(int permissionId)
        {
            try
            {
                return await _dbSet
                    .Where(rp => rp.IsActive && rp.PermissionId == permissionId)
                    .Include(rp => rp.Role)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByPermissionIdAsync con permissionId={permissionId}");
                throw;
            }
        }

        public async Task<bool> HasPermissionAsync(int roleId, int permissionId)
        {
            try
            {
                return await _dbSet.AnyAsync(rp => rp.IsActive && rp.RoleId == roleId && rp.PermissionId == permissionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en HasPermissionAsync con roleId={roleId}, permissionId={permissionId}");
                throw;
            }
        }

        public async Task<bool> HasPermissionAsync(int roleId, string resource, string action)
        {
            try
            {
                return await _dbSet
                    .Where(rp => rp.IsActive && rp.RoleId == roleId)
                    .AnyAsync(rp => rp.Permission.Resource == resource && rp.Permission.Action == action && rp.Permission.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en HasPermissionAsync con roleId={roleId}, resource={resource}, action={action}");
                throw;
            }
        }

        public async Task AssignPermissionAsync(int roleId, int permissionId, string? assignedBy = null)
        {
            try
            {
                var existing = await _dbSet
                    .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

                if (existing != null)
                {
                    if (!existing.IsActive)
                    {
                        existing.IsActive = true;
                        existing.ModifiedTime = DateTime.UtcNow;
                        existing.ModifiedBy = assignedBy;
                        _logger.LogInformation($"Permiso reactivado: roleId={roleId}, permissionId={permissionId}");
                    }
                    else
                    {
                        _logger.LogWarning($"El permiso ya está asignado: roleId={roleId}, permissionId={permissionId}");
                        return;
                    }
                }
                else
                {
                    var rolePermission = new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId,
                        IsActive = true,
                        CreatedTime = DateTime.UtcNow,
                        CreatedBy = assignedBy
                    };

                    await _dbSet.AddAsync(rolePermission);
                    _logger.LogInformation($"Nuevo permiso asignado: roleId={roleId}, permissionId={permissionId}");
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en AssignPermissionAsync con roleId={roleId}, permissionId={permissionId}");
                throw;
            }
        }

        public async Task RemovePermissionAsync(int roleId, int permissionId)
        {
            try
            {
                var rolePermission = await _dbSet
                    .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && rp.IsActive);

                if (rolePermission != null)
                {
                    rolePermission.IsActive = false;
                    rolePermission.ModifiedTime = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Permiso removido: roleId={roleId}, permissionId={permissionId}");
                }
                else
                {
                    _logger.LogWarning($"No se encontró permiso activo para remover: roleId={roleId}, permissionId={permissionId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en RemovePermissionAsync con roleId={roleId}, permissionId={permissionId}");
                throw;
            }
        }

        public async Task RemoveAllRolePermissionsAsync(int roleId)
        {
            try
            {
                var rolePermissions = await _dbSet
                    .Where(rp => rp.RoleId == roleId && rp.IsActive)
                    .ToListAsync();

                if (rolePermissions.Any())
                {
                    foreach (var rolePermission in rolePermissions)
                    {
                        rolePermission.IsActive = false;
                        rolePermission.ModifiedTime = DateTime.UtcNow;
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Se removieron {rolePermissions.Count} permisos del rol: roleId={roleId}");
                }
                else
                {
                    _logger.LogWarning($"No se encontraron permisos activos para el rol: roleId={roleId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en RemoveAllRolePermissionsAsync con roleId={roleId}");
                throw;
            }
        }

        public async Task<IEnumerable<RolePermission>> GetRolePermissionsWithDetailsAsync(int roleId)
        {
            try
            {
                return await _dbSet
                    .Where(rp => rp.IsActive && rp.RoleId == roleId)
                    .Include(rp => rp.Permission)
                    .Include(rp => rp.Role)
                    .OrderBy(rp => rp.Permission.Resource)
                    .ThenBy(rp => rp.Permission.Action)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetRolePermissionsWithDetailsAsync con roleId={roleId}");
                throw;
            }
        }
    }
}
