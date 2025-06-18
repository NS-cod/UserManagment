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
    public class RoleRepository(UserManagementDbContext context, ILoggerFactory loggerFactory) : GenericRepository<Role>(context, loggerFactory.CreateLogger<GenericRepository<Role>>()), IRoleRepository
    {
        public async Task<Role?> GetByNameAsync(string name)
        {
            try
            {
                return await _dbSet
                    .Where(r => r.IsActive)
                    .FirstOrDefaultAsync(r => r.Name == name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByNameAsync con name={name}");
                throw;
            }
        }

        public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
        {
            try
            {
                var query = _dbSet.Where(r => r.IsActive && r.Name == name);

                if (excludeId.HasValue)
                    query = query.Where(r => r.Id != excludeId.Value);

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en IsNameExistsAsync con name={name}");
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetRolesWithPermissionsAsync()
        {
            try
            {
                return await _dbSet
                    .Where(r => r.IsActive)
                    .Include(r => r.RolePermissions.Where(rp => rp.IsActive))
                    .ThenInclude(rp => rp.Permission)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetRolesWithPermissionsAsync");
                throw;
            }
        }

        public async Task<Role?> GetRoleWithPermissionsAsync(int roleId)
        {
            try
            {
                return await _dbSet
                    .Where(r => r.IsActive && r.Id == roleId)
                    .Include(r => r.RolePermissions.Where(rp => rp.IsActive))
                    .ThenInclude(rp => rp.Permission)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetRoleWithPermissionsAsync con roleId={roleId}");
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
        {
            try
            {
                return await _context.UserRoles
                    .Where(ur => ur.IsActive && ur.UserId == userId)
                    .Select(ur => ur.Role)
                    .Where(r => r.IsActive)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetUserRolesAsync con userId={userId}");
                throw;
            }
        }
    }
}
