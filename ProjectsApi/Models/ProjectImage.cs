using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectsApi.Models
{
    public class ProjectImage
    {
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        
    }
}
