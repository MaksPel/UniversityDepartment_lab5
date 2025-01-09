using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Specialties;
using UniversityDepartment_lab5.ViewModels;

namespace UniversityDepartment_lab5.Controllers
{
    public class SpecialtiesController : Controller
    {
        private readonly int pageSize = 50;
        private readonly UniversityDbContext _context;

        public SpecialtiesController(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, string Name = "", string Department = "", SortState sortOrder = SortState.No)
        {
            IQueryable<Specialty> specialtiesContext = _context.Specialties
                .Include(c => c.Department);

            specialtiesContext = SortAndFilter(specialtiesContext, sortOrder, Name ?? "", Department ?? "");

            var count = await specialtiesContext.CountAsync();

            var specialties = await specialtiesContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            SpecialtiesViewModel viewModel = new SpecialtiesViewModel()
            {
                Specialties = specialties,
                Name = Name,
                Department = Department,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SpecialtySortViewModel(sortOrder)
            };

            return View(viewModel);
        }

        private IQueryable<Specialty> SortAndFilter(IQueryable<Specialty> specialtiesContext, SortState sortOrder, string Name, string Department)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    specialtiesContext = specialtiesContext.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    specialtiesContext = specialtiesContext.OrderByDescending(s => s.Name);
                    break;
                case SortState.DepartmentAsc:
                    specialtiesContext = specialtiesContext.OrderBy(s => s.Department.Name);
                    break;
                case SortState.DepartmentDesc:
                    specialtiesContext = specialtiesContext.OrderByDescending(s => s.Department.Name);
                    break;
            }
            if (Name != "")
            {
                specialtiesContext = specialtiesContext.Where(c => c.Name == Name);
            }
            if (Department != "")
            {
                specialtiesContext = specialtiesContext.Where(c => c.Department.Name == Department);
            }
            return specialtiesContext;
        }

        // GET: Specialties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.SpecialtyId == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // GET: Specialties/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View();
        }

        // POST: Specialties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialtyId,Name,DepartmentId")] Specialty specialty)
        {
            if (ModelState.IsValid)
            {
                specialty.SpecialtyId = Guid.NewGuid();
                _context.Add(specialty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", specialty.DepartmentId);
            return View(specialty);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", specialty.DepartmentId);
            return View(specialty);
        }

        // POST: Specialties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SpecialtyId,Name,DepartmentId")] Specialty specialty)
        {
            if (id != specialty.SpecialtyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialty.SpecialtyId))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", specialty.DepartmentId);
            return View(specialty);
        }

        // GET: Specialties/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties
                .Include(s => s.Department)
                .FirstOrDefaultAsync(m => m.SpecialtyId == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty != null)
            {
                _context.Specialties.Remove(specialty);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialtyExists(Guid id)
        {
            return _context.Specialties.Any(e => e.SpecialtyId == id);
        }
    }
}
