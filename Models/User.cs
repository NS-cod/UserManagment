using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User : BaseModel
    {
       [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [StringLength(320)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        public DateTime? LastLoginDate { get; set; }
        
        [Required]
        public bool IsEmailVerified { get; set; } = false;
        
        [Required]
        public bool IsLocked { get; set; } = false;
        
        public DateTime? LockoutEnd { get; set; }
        
        public int FailedLoginAttempts { get; set; } = 0;
        
        // Navegación
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
        
        // Propiedades calculadas
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        
        [NotMapped]
        public IEnumerable<Role> Roles => UserRoles.Where(ur => ur.IsActive).Select(ur => ur.Role);
        
        [NotMapped]
        public IEnumerable<Permission> Permissions => 
            UserRoles.Where(ur => ur.IsActive)
                    .SelectMany(ur => ur.Role.RolePermissions.Where(rp => rp.IsActive))
                    .Select(rp => rp.Permission)
                    .Distinct();
    }
}
