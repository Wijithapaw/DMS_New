using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.Data.Entities
{
    public class Project : BaseEntity 
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [ForeignKey("ProjectCategory")]
        public int ProjectCategoryId { get; set; }

        [Required]
        [ForeignKey("ProjectOwner")]
        public int ProjectOwnerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ProjectCategory ProjectCategory { get; set; }

        public virtual ApplicationUser ProjectOwner { get; set; }
    }
}
