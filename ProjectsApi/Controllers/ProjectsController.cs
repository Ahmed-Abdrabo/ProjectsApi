using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsApi.Data;
using ProjectsApi.Models;
using ProjectsApi.Models.Dto;

namespace ProjectsApi.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectsController(AppDbContext db, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
            _webHostEnvironment = webHostEnvironment;
        }
       
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Project> objlist = _db.Projects.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProjectDto>>(objlist);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpGet("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Project obj = _db.Projects.First(c => c.Id == id);
                _response.Result = _mapper.Map<ProjectDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost]
        public ResponseDto Post([FromForm]ProjectDto projectDto)
        {
            try
            {
                projectDto.Id = 0;
                Project project = _mapper.Map<Project>(projectDto);
                _db.Projects.Add(project);
                _db.SaveChanges();

                if (projectDto.ProjectImages != null)
                {
                    foreach (var projectImage in projectDto.ProjectImages)
                    {
                        // Create a folder for the project images if it doesn't exist
                        string projectFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProjectImages", projectDto.Id.ToString());
                        if (!Directory.Exists(projectFolderPath))
                            Directory.CreateDirectory(projectFolderPath);

                        // Generate a unique file name for the image
                        string fileName = projectDto.Id + "_" + Guid.NewGuid().ToString() + Path.GetExtension(projectImage.Image.FileName);
                        string filePath = Path.Combine(projectFolderPath, fileName);

                        // Save the image to the file system
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            projectImage.Image.CopyTo(fileStream);
                        }

                        // Update the image URL and local path
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        projectImage.ImageUrl = baseUrl + "/ProjectImages/" + projectDto.Id.ToString() + "/" + fileName;
                        projectImage.ImageLocalPath = filePath;


                        projectImage.ProjectId = projectDto.Id;
                        _mapper.Map<ProjectImage>(projectImage);
                        _db.ProjectImages.Add(_mapper.Map<ProjectImage>(projectImage));

                    }



                }

                _db.Projects.Update(project);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto Put(ProjectDto projectDto)
        {
            try
            {
                Project project = _mapper.Map<Project>(projectDto);

                if (projectDto.ProjectImages != null)
                {
                    foreach (var projectImage in projectDto.ProjectImages)
                    {
                        // Create a folder for the project images if it doesn't exist
                        string projectFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "ProjectImages", projectDto.Id.ToString());
                        if (!Directory.Exists(projectFolderPath))
                            Directory.CreateDirectory(projectFolderPath);

                        // Generate a unique file name for the image
                        string fileName = projectDto.Id + "_" + Guid.NewGuid().ToString() + Path.GetExtension(projectImage.Image.FileName);
                        string filePath = Path.Combine(projectFolderPath, fileName);

                        // Save the image to the file system
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            projectImage.Image.CopyTo(fileStream);
                        }

                        // Update the image URL and local path
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        projectImage.ImageUrl = baseUrl + "/ProjectImages/" + projectDto.Id.ToString() + "/" + fileName;
                        projectImage.ImageLocalPath = filePath;


                        projectImage.ProjectId = projectDto.Id;
                        _mapper.Map<ProjectImage>(projectImage);
                        _db.ProjectImages.Add(_mapper.Map<ProjectImage>(projectImage));

                    }
                }
                    _db.Projects.Update(project);
                _db.SaveChanges();

                _response.Result = _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Project obj = _db.Projects.First(c => c.Id == id);
                foreach (var projectImage in obj.ProjectImages)
                {
                    // Delete the image file from the file system if needed
                    if (!string.IsNullOrEmpty(projectImage.ImageLocalPath))
                    {
                        System.IO.File.Delete(projectImage.ImageLocalPath);
                    }

                    // Remove the image from the context
                    _db.ProjectImages.Remove(projectImage);
                }
                _db.Projects.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



        [HttpDelete("images/{imageId}")]
        public ResponseDto DeleteImage(int imageId)
        {
            try
            {
                // Find the image by its ID
                var projectImage = _db.ProjectImages.FirstOrDefault(u=>u.Id==imageId);

                if (projectImage == null)
                {
                    return _response;
                }

                // Delete the image file from the file system if needed
                if (!string.IsNullOrEmpty(projectImage.ImageLocalPath))
                {
                    System.IO.File.Delete(projectImage.ImageLocalPath);
                }

                // Remove the image from the context
                _db.ProjectImages.Remove(projectImage);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

    }
}

