using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<Permission?> GetByResourceActionAsync(string resource, string action);
        Task<IEnumerable<Permission>> GetByResourceAsync(string resource);
    }
}
