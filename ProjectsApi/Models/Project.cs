using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ProjectsApi.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        [ValidateNever]
        public List<ProjectImage> ProjectImages { get; set; }
    }
}
