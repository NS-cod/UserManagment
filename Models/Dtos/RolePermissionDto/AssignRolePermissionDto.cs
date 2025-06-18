using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RolePermissionDto
{
    public class AssignRolePermissionDto
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public int PermissionId { get; set; }
    }
}
