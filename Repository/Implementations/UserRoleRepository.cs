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
    public class UserRoleRepository(UserManagementDbContext context, ILoggerFactory loggerFactory) : GenericRepository<UserRole>(context, loggerFactory.CreateLogger<GenericRepository<UserRole>>()), IUserRoleRepository
    {
        public async Task<IEnumerable<UserRole>> GetByUserIdAsync(int userId)
        {
            try
            {
                return await _dbSet
                    .Where(ur => ur.IsActive && ur.UserId == userId)
                    .Include(ur => ur.Role)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByUserIdAsync con userId={userId}");
                throw;
            }
        }

        public async Task<IEnumerable<UserRole>> GetByRoleIdAsync(int roleId)
        {
            try
            {
                return await _dbSet
                    .Where(ur => ur.IsActive && ur.RoleId == roleId)
                    .Include(ur => ur.User)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByRoleIdAsync con roleId={roleId}");
                throw;
            }
        }

        public async Task<bool> HasRoleAsync(int userId, int roleId)
        {
            try
            {
                return await _dbSet.AnyAsync(ur => ur.IsActive && ur.UserId == userId && ur.RoleId == roleId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en HasRoleAsync con userId={userId}, roleId={roleId}");
                throw;
            }
        }

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            try
            {
                return await _dbSet
                    .Where(ur => ur.IsActive && ur.UserId == userId)
                    .AnyAsync(ur => ur.Role.Name == roleName && ur.Role.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en HasRoleAsync con userId={userId}, roleName={roleName}");
                throw;
            }
        }

        public async Task AssignRoleAsync(int userId, int roleId, string? assignedBy = null)
        {
            try
            {
                var existingRole = await _dbSet
                    .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (existingRole != null)
                {
                    if (!existingRole.IsActive)
                    {
                        existingRole.IsActive = true;
                        existingRole.ModifiedTime = DateTime.UtcNow;
                        existingRole.ModifiedBy = assignedBy;
                        _dbSet.Update(existingRole);
                    }
                }
                else
                {
                    var userRole = new UserRole
                    {
                        UserId = userId,
                        RoleId = roleId,
                        AssignedBy = assignedBy,
                        CreatedBy = assignedBy
                    };
                    await _dbSet.AddAsync(userRole);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en AssignRoleAsync con userId={userId}, roleId={roleId}");
                throw;
            }
        }

        public async Task RemoveRoleAsync(int userId, int roleId)
        {
            try
            {
                var userRole = await _dbSet
                    .FirstOrDefaultAsync(ur => ur.IsActive && ur.UserId == userId && ur.RoleId == roleId);

                if (userRole != null)
                {
                    userRole.IsActive = false;
                    userRole.ModifiedTime = DateTime.UtcNow;
                    _dbSet.Update(userRole);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en RemoveRoleAsync con userId={userId}, roleId={roleId}");
                throw;
            }
        }

        public async Task RemoveAllUserRolesAsync(int userId)
        {
            try
            {
                var userRoles = await _dbSet
                    .Where(ur => ur.IsActive && ur.UserId == userId)
                    .ToListAsync();

                foreach (var userRole in userRoles)
                {
                    userRole.IsActive = false;
                    userRole.ModifiedTime = DateTime.UtcNow;
                }

                _dbSet.UpdateRange(userRoles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en RemoveAllUserRolesAsync con userId={userId}");
                throw;
            }
        }
    }
}
