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
    public class PermissionRepository(UserManagementDbContext context, ILoggerFactory loggerFactory) : GenericRepository<Permission>(context, loggerFactory.CreateLogger<GenericRepository<Permission>>()), IPermissionRepository
    {
        public async Task<Permission?> GetByResourceActionAsync(string resource, string action)
        {
            try
            {
                return await _dbSet
                    .Where(p => p.IsActive)
                    .FirstOrDefaultAsync(p => p.Resource == resource && p.Action == action);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByResourceActionAsync con resource={resource}, action={action}");
                throw;
            }
        }

        public async Task<IEnumerable<Permission>> GetByResourceAsync(string resource)
        {
            try
            {
                return await _dbSet
                    .Where(p => p.IsActive && p.Resource == resource)
                    .OrderBy(p => p.Action)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByResourceAsync con resource={resource}");
                throw;
            }
        }


    }
}
