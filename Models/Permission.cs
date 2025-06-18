using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Permission : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Resource { get; set; } = string.Empty; // User, Product, Order, etc

        [Required]
        [StringLength(50)]
        public string Action { get; set; } = string.Empty; // CRUD

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public bool IsSystemPermission { get; set; } = false; // Permisos del sistema no se pueden eliminar

        // Navegación
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
