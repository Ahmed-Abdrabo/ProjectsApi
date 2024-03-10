using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProjectsApi.Models.Dto
{
    public class ProjectImageDto
    {
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public int ProjectId { get; set; }
        public IFormFile? Image { get; set; }

    }
}
