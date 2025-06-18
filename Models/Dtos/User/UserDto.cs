using Models.Dtos.Permission;
using Models.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; } = string.Empty;
        public List<RoleDto> Roles { get; set; } = new();
        public List<PermissionDto> Permissions { get; set; } = new();
    }
}
