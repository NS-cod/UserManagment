using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedTime { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
