using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RolePermissionDto
{
    public class BulkAssignRolePermissionsDto
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public List<int> PermissionIds { get; set; } = new();
    }
}
