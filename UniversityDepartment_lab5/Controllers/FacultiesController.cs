using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Faculties;
using UniversityDepartment_lab5.ViewModels;

namespace UniversityDepartment_lab5.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly int pageSize = 50;
        private readonly UniversityDbContext _context;

        public FacultiesController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Faculties
        public async Task<IActionResult> Index(int page = 1, string Name = "", SortState sortOrder = SortState.No)
        {
            IQueryable<Faculty> facultiesContext = _context.Faculties;

            facultiesContext = SortAndFilter(facultiesContext, sortOrder, Name ?? "");

            var count = await facultiesContext.CountAsync();

            var faculties = await facultiesContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            FacultiesViewModel viewModel = new FacultiesViewModel()
            {
                Faculties = faculties,
                Name = Name,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new FacultySortViewModel(sortOrder)
            };

            return View(viewModel);
        }

        private IQueryable<Faculty> SortAndFilter(IQueryable<Faculty> facultiesContext, SortState sortOrder, string Name)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    facultiesContext = facultiesContext.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    facultiesContext = facultiesContext.OrderByDescending(s => s.Name);
                    break;
            }
            if (Name != "")
            {
                facultiesContext = facultiesContext.Where(c => c.Name == Name);
            }
            return facultiesContext;
        }

        // GET: Faculties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // GET: Faculties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacultyId,Name")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                faculty.FacultyId = Guid.NewGuid();
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("FacultyId,Name")] Faculty faculty)
        {
            if (id != faculty.FacultyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.FacultyId))
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
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties
                .FirstOrDefaultAsync(m => m.FacultyId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var faculty = await _context.Faculties.FindAsync(id);
            if (faculty != null)
            {
                _context.Faculties.Remove(faculty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(Guid id)
        {
            return _context.Faculties.Any(e => e.FacultyId == id);
        }
    }
}
