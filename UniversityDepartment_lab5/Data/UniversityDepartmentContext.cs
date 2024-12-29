using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Users;

namespace UniversityDepartment_lab5.Data
{
    public class UniversityDbContext(DbContextOptions<UniversityDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public virtual DbSet<IdentityUser> Users {  get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Faculty> Faculties { get; set; }

        public virtual DbSet<Specialty> Specialties { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual DbSet<Teacher> Teachers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);*/
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UniversityDepartment_lab5.ViewModels.Users.IdentityUserViewModel> IdentityUserViewModel { get; set; } = default!;
    }
}
