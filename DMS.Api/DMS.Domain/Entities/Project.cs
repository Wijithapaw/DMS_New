using DMS.Domain.Entities.Identity;
using DMS.Domain.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMS.Domain.Entities
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
        [ForeignKey("ProjectOwner")]
        public int ProjectOwnerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual User ProjectOwner { get; set; }
    }
}
