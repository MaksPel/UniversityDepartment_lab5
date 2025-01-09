using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Courses;
using UniversityDepartment_lab5.ViewModels;

namespace UniversityDepartment_lab5.Controllers
{
    public class CoursesController : Controller
    {
        private readonly int pageSize = 50;
        private readonly UniversityDbContext _context;

        public CoursesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int page = 1, string Specialty = "", int CourseNumber = 0, int SemesterNumber = 0, SortState sortOrder = SortState.No)
        {
            IQueryable<Course> coursesContext = _context.Courses
                .Include(c => c.Specialty);

            coursesContext = SortAndFilter(coursesContext, sortOrder, Specialty ?? "", CourseNumber, SemesterNumber);

            var count = await coursesContext.CountAsync();

            var courses = await coursesContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            CourseViewModel viewModel = new CourseViewModel()
            {
                Courses = courses,
                Specialty = Specialty,
                CourseNumber = CourseNumber,
                SemesterNumber = SemesterNumber,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new CoursesSortViewModel(sortOrder)
            };

            return View(viewModel);
        }

        private IQueryable<Course> SortAndFilter(IQueryable<Course> coursesContext, SortState sortOrder, string Specialty, int courseNumber, int semesterNumber)
        {
            switch (sortOrder)
            {
                case SortState.SpecialtyAsc:
                    coursesContext = coursesContext.OrderBy(s => s.Specialty.Name);
                    break;
                case SortState.SpecialtyDesc:
                    coursesContext = coursesContext.OrderByDescending(s => s.Specialty.Name);
                    break;
                case SortState.CourseNumberAsc:
                    coursesContext = coursesContext.OrderBy(s => s.CourseNumber);
                    break;
                case SortState.CourseNumberDesc:
                    coursesContext = coursesContext.OrderByDescending(s => s.CourseNumber);
                    break;
                case SortState.SemesterAsc:
                    coursesContext = coursesContext.OrderBy(s => s.SemesterNumber);
                    break;
                case SortState.SemesterDesc:
                    coursesContext = coursesContext.OrderByDescending(s => s.SemesterNumber);
                    break;
            }
            if (Specialty != "")
            {
                coursesContext = coursesContext.Where(c => c.Specialty.Name == Specialty);
            }
            if (courseNumber != 0)
            {
                coursesContext = coursesContext.Where(c => c.CourseNumber == courseNumber);
            }
            if (semesterNumber != 0)
            {
                coursesContext = coursesContext.Where(c => c.SemesterNumber == semesterNumber);
            }
            return coursesContext;
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Specialty)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "SpecialtyId", "Name");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseNumber,SemesterNumber,SpecialtyId")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.CourseId = Guid.NewGuid();
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "SpecialtyId", "Name", course.SpecialtyId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "SpecialtyId", "Name", course.SpecialtyId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CourseId,CourseNumber,SemesterNumber,SpecialtyId")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpecialtyId"] = new SelectList(_context.Specialties, "SpecialtyId", "Name", course.SpecialtyId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Specialty)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(Guid id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
