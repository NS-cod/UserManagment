using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class GenericRepository<T>(UserManagementDbContext context, ILogger<GenericRepository<T>> logger) : IGenericRepository<T> where T : BaseModel
    {
        protected readonly UserManagementDbContext _context = context;
        protected readonly DbSet<T> _dbSet = context.Set<T>();
        protected readonly ILogger<GenericRepository<T>> _logger = logger;

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en GetByIdAsync con ID={id}");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.Where(x => x.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAllAsync");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).Where(x => x.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FindAsync");
                throw;
            }
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(x => x.IsActive).FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FirstOrDefaultAsync");
                throw;
            }
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en AddAsync para {typeof(T).Name}");
                throw;
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var exists = await _dbSet.AnyAsync(x => x.Id == entity.Id && x.IsActive);
                if (!exists)
                    throw new InvalidOperationException($"No se encontró {typeof(T).Name} con ID {entity.Id}");

                _dbSet.Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en UpdateAsync para {typeof(T).Name} ID={entity.Id}");
                throw;
            }
        }

        public virtual async Task PatchAsync(int id, Action<T> updateAction)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null || !entity.IsActive)
                    throw new InvalidOperationException($"No se encontró {typeof(T).Name} con ID {id}");

                updateAction(entity);
                _dbSet.Update(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en PatchAsync para {typeof(T).Name} ID={id}");
                throw;
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    entity.IsActive = false;
                    _dbSet.Update(entity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteAsync con ID={id}");
                throw;
            }
        }

        public virtual async Task DeleteAsync(T entity)
        {
            try
            {
                entity.IsActive = false;
                _dbSet.Update(entity);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en DeleteAsync para {typeof(T).Name} ID={entity.Id}");
                throw;
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _dbSet.AnyAsync(x => x.Id == id && x.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en ExistsAsync con ID={id}");
                throw;
            }
        }

        public virtual async Task<int> CountAsync()
        {
            try
            {
                return await _dbSet.CountAsync(x => x.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CountAsync");
                throw;
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(x => x.IsActive).CountAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en CountAsync con predicado");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                return await _dbSet
                    .Where(x => x.IsActive)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetPagedAsync");
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet
                    .Where(x => x.IsActive)
                    .Where(predicate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetPagedAsync con predicado");
                throw;
            }
        }
    }
}
