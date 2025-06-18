using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RolePermission : BaseModel
    {
        [Required]
        public int RoleId { get; set; }

        [Required]
        public int PermissionId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? AssignedBy { get; set; }

        // Navegación
        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey(nameof(PermissionId))]
        public virtual Permission Permission { get; set; } = null!;
    }
}
