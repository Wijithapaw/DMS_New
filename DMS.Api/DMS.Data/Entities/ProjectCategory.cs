using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DMS.Data.Entities
{
    public class ProjectCategory: BaseEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
