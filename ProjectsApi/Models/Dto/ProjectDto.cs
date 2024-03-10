
namespace ProjectsApi.Models.Dto
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public List<ProjectImageDto> ProjectImages { get; set; }
    }
}
