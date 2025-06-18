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
    public class UserRepository(UserManagementDbContext context, ILoggerFactory loggerFactory) : GenericRepository<User>(context, loggerFactory.CreateLogger<GenericRepository<User>>()), IUserRepository
    {
        public async Task<User?> GetByUsernameAsync(string username)
        {
            try
            {
                return await _dbSet
                    .Where(x => x.IsActive)
                    .FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByUsernameAsync con username={username}");
                throw;
            }
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                return await _dbSet
                    .Where(x => x.IsActive)
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByEmailAsync con email={email}");
                throw;
            }
        }

        public async Task<bool> IsUsernameExistsAsync(string username, int? excludeUserId = null)
        {
            try
            {
                var query = _dbSet.Where(x => x.IsActive && x.Username == username);

                if (excludeUserId.HasValue)
                    query = query.Where(x => x.Id != excludeUserId.Value);

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en IsUsernameExistsAsync con username={username} y excludeUserId={excludeUserId}");
                throw;
            }
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null)
        {
            try
            {
                var query = _dbSet.Where(x => x.IsActive && x.Email == email);

                if (excludeUserId.HasValue)
                    query = query.Where(x => x.Id != excludeUserId.Value);

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en IsEmailExistsAsync con email={email} y excludeUserId={excludeUserId}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            try
            {
                return await _dbSet
                    .Where(x => x.IsActive)
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetActiveUsersAsync");
                throw;
            }
        }
        public async Task UpdateLastLoginAsync(int userId)
        {
            try
            {
                var user = await _dbSet.FindAsync(userId);
                if (user != null)
                {
                    user.LastLoginDate = DateTime.UtcNow;
                    _dbSet.Update(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en UpdateLastLoginAsync para userId={userId}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetActiveUsersAsync();

                var term = searchTerm.ToLower();

                return await _dbSet
                    .Where(x => x.IsActive)
                    .Where(u => u.FirstName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                                u.LastName.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                                u.Email.Contains(term, StringComparison.CurrentCultureIgnoreCase) ||
                                u.Username.Contains(term, StringComparison.CurrentCultureIgnoreCase))
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en SearchUsersAsync con searchTerm={searchTerm}");
                throw;
            }
        }
    }
}

