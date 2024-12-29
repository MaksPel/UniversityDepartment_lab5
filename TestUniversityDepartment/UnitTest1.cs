using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Controllers;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Courses;
using Xunit;

namespace UniversityDepartment_lab5.Tests
{
    public class CoursesControllerTests
    {
        private readonly UniversityDbContext _context;
        private readonly CoursesController _controller;

        public CoursesControllerTests()
        {
            var options = new DbContextOptionsBuilder<UniversityDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new UniversityDbContext(options);

            // Seed data for testing
            SeedData();

            _controller = new CoursesController(_context);
        }

        private void SeedData()
        {
            var specialty = new Specialty { SpecialtyId = Guid.NewGuid(), Name = "Computer Science" };

            _context.Specialties.Add(specialty);
            _context.Courses.AddRange(
                new Course
                {
                    CourseId = Guid.NewGuid(),
                    CourseNumber = 1,
                    SemesterNumber = 1,
                    SpecialtyId = specialty.SpecialtyId,
                    Specialty = specialty
                },
                new Course
                {
                    CourseId = Guid.NewGuid(),
                    CourseNumber = 2,
                    SemesterNumber = 2,
                    SpecialtyId = specialty.SpecialtyId,
                    Specialty = specialty
                }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task Index_ReturnsViewWithCourses()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CourseViewModel>(viewResult.Model);
            Assert.NotEmpty(model.Courses);
        }

        [Fact]
        public async Task Details_ValidId_ReturnsCourseDetails()
        {
            // Arrange
            var course = _context.Courses.First();

            // Act
            var result = await _controller.Details(course.CourseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Course>(viewResult.Model);
            Assert.Equal(course.CourseId, model.CourseId);
        }

        [Fact]
        public async Task Details_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.Details(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidCourse_RedirectsToIndex()
        {
            // Arrange
            var course = new Course
            {
                CourseId = Guid.NewGuid(),
                CourseNumber = 3,
                SemesterNumber = 1,
                SpecialtyId = _context.Specialties.First().SpecialtyId
            };

            // Act
            var result = await _controller.Create(course);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_ValidId_UpdatesCourse()
        {
            // Arrange
            var course = _context.Courses.First();
            course.CourseNumber = 5;

            // Act
            var result = await _controller.Edit(course.CourseId, course);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);

            var updatedCourse = _context.Courses.Find(course.CourseId);
            Assert.Equal(5, updatedCourse.CourseNumber);
        }

        [Fact]
        public async Task DeleteConfirmed_ValidId_DeletesCourse()
        {
            // Arrange
            var course = _context.Courses.First();

            // Act
            var result = await _controller.DeleteConfirmed(course.CourseId);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_controller.Index), redirectResult.ActionName);
            Assert.Null(_context.Courses.Find(course.CourseId));
        }
    }
}
