using Models.Dtos.Role;
using Models.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.UserRoleDto
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string? AssignedBy { get; set; }
        public bool IsActive { get; set; }
        public UserSummaryDto User { get; set; } = new();
        public RoleSummaryDto Role { get; set; } = new();
    }
}
