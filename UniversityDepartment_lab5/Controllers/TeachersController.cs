using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Teachers;
using UniversityDepartment_lab5.ViewModels;

namespace UniversityDepartment_lab5.Controllers
{
    public class TeachersController : Controller
    {
        private readonly int pageSize = 50;
        private readonly UniversityDbContext _context;

        public TeachersController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(int page = 1, string Name = "", string Surname = "", string Midname = "", string Position = "", int Age = 0, SortState sortOrder = SortState.No)
        {
            IQueryable<Teacher> teachersContext = _context.Teachers;

            teachersContext = SortAndFilter(teachersContext, sortOrder, Name ?? "", Surname ?? "", Midname ?? "", Position ?? "", Age);

            var count = teachersContext.Count();

            var teachers = await teachersContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            TeachersViewModel viewModel = new TeachersViewModel()
            {
                Teachers = teachers,
                Name = Name,
                Surname = Surname,
                Midname = Midname,
                Position = Position,
                Age = Age,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new TeacherSortViewModel(sortOrder)
            };

            return View(viewModel);
        }

        private IQueryable<Teacher> SortAndFilter(IQueryable<Teacher> teachersContext, SortState sortOrder, string Name, string Surname, string Midname, string Position, int Age)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    teachersContext = teachersContext.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    teachersContext = teachersContext.OrderByDescending(s => s.Name);
                    break;
                case SortState.SurnameAsc:
                    teachersContext = teachersContext.OrderBy(s => s.Surname);
                    break;
                case SortState.SurnameDesc:
                    teachersContext = teachersContext.OrderByDescending(s => s.Surname);
                    break;
                case SortState.MidnameAsc:
                    teachersContext = teachersContext.OrderBy(s => s.Midname);
                    break;
                case SortState.MidnameDesc:
                    teachersContext = teachersContext.OrderByDescending(s => s.Midname);
                    break;
                case SortState.PositionAsc:
                    teachersContext = teachersContext.OrderBy(s => s.Position);
                    break;
                case SortState.PositionDesc:
                    teachersContext = teachersContext.OrderByDescending(s => s.Position);
                    break;
                case SortState.AgeAsc:
                    teachersContext = teachersContext.OrderBy(s => s.Age);
                    break;
                case SortState.AgeDesc:
                    teachersContext = teachersContext.OrderByDescending(s => s.Age);
                    break;
            }
            if (Name != "")
            {
                teachersContext = teachersContext.Where(c => c.Name == Name);
            }
            if (Surname != "")
            {
                teachersContext = teachersContext.Where(c => c.Surname == Surname);
            }
            if (Midname != "")
            {
                teachersContext = teachersContext.Where(c => c.Midname == Midname);
            }
            if (Position != "")
            {
                teachersContext = teachersContext.Where(c => c.Position == Position);
            }
            if (Age != 0)
            {
                teachersContext = teachersContext.Where(c => c.Age == Age);
            }
            return teachersContext;
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,Name,Surname,Midname,Position,Age")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.TeacherId = Guid.NewGuid();
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TeacherId,Name,Surname,Midname,Position,Age")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(Guid id)
        {
            return _context.Teachers.Any(e => e.TeacherId == id);
        }
    }
}
