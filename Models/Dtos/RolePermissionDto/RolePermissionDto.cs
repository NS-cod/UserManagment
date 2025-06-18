using Models.Dtos.Permission;
using Models.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.RolePermissionDto
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string? AssignedBy { get; set; }
        public bool IsActive { get; set; }
        public RoleSummaryDto Role { get; set; } = new();
        public PermissionSummaryDto Permission { get; set; } = new();
    }
}
