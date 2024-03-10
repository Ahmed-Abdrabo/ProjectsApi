using Microsoft.EntityFrameworkCore;
using ProjectsApi.Models;

namespace ProjectsApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Title = "Dummy Title 1",
                    Description = "Description 1",
                    Author = "Author 1"
                },
                new Project
                {
                    Id = 2,
                    Title = "Dummy Title 2",
                    Description = "Description 2",
                    Author = "Author 2"
                },
                new Project
                {
                    Id = 3,
                    Title = "Dummy Title 3",
                    Description = "Description 3",
                    Author = "Author 3"
                },
                new Project
                {
                    Id = 4,
                    Title = "Dummy Title 4",
                    Description = "Description 4",
                    Author = "Author 4"
                },
                new Project
                {
                    Id = 5,
                    Title = "Dummy Title 5",
                    Description = "Description 5",
                    Author = "Author 5"
                },
                new Project
                {
                    Id = 6,
                    Title = "Dummy Title 6",
                    Description = "Description 6",
                    Author = "Author 6"
                }
            );
        }

    }
}
