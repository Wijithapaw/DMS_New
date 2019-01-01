using System;

namespace DMS.Domain.Dtos.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectCategoryId { get; set; }
        public int OwnerId { get; set; }
        public string ProjectCategory { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
