using AutoMapper;
using ProjectsApi.Models;
using ProjectsApi.Models.Dto;

namespace ProjectsApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapping = new MapperConfiguration(config =>
            {
                config.CreateMap<Project, ProjectDto>().ReverseMap();
                config.CreateMap<ProjectImage, ProjectImageDto>().ReverseMap();
            });
            return mapping;
        }
    }
}
