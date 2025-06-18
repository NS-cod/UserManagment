using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Role : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public bool IsSystemRole { get; set; } = false; // Roles del sistema no se pueden eliminar

        // Navegación
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
