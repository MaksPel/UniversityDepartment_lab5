using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Departments;
using UniversityDepartment_lab5.ViewModels;

namespace UniversityDepartment_lab5.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly int pageSize = 50;
        private readonly UniversityDbContext _context;

        public DepartmentsController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(int page = 1, string Name = "", string Faculty = "", bool isGraduating = false, SortState sortOrder = SortState.No)
        {
            IQueryable<Department> departmentsContext = _context.Departments
                .Include(c => c.Faculty);

            departmentsContext = SortAndFilter(departmentsContext, sortOrder, Name ?? "", Faculty ?? "", isGraduating);

            var count = await departmentsContext.CountAsync();

            var departments = await departmentsContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            DepartmentViewModel viewModel = new DepartmentViewModel()
            {
                Departments = departments,
                Name = Name,
                Faculty = Faculty,
                IsGraduating = isGraduating,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new DepartmentSortViewModel(sortOrder)
            };

            return View(viewModel);
        }

        private IQueryable<Department> SortAndFilter(IQueryable<Department> departmentsContext, SortState sortOrder, string Name, string Faculty, bool isGraduating)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    departmentsContext = departmentsContext.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    departmentsContext = departmentsContext.OrderByDescending(s => s.Name);
                    break;
                case SortState.FacultyAsc:
                    departmentsContext = departmentsContext.OrderBy(s => s.Faculty.Name);
                    break;
                case SortState.FacultyDesc:
                    departmentsContext = departmentsContext.OrderByDescending(s => s.Faculty.Name);
                    break;
            }
            if (Name != "")
            {
                departmentsContext = departmentsContext.Where(c => c.Name == Name);
            }
            if (Faculty != "")
            {
                departmentsContext = departmentsContext.Where(c => c.Faculty.Name == Faculty);
            }
            if (isGraduating == true)
            {
                departmentsContext = departmentsContext.Where(c => c.IsGraduating == isGraduating);
            }
            return departmentsContext;
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyId");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,Name,IsGraduating,FacultyId")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.DepartmentId = Guid.NewGuid();
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyId", department.FacultyId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyId", department.FacultyId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DepartmentId,Name,IsGraduating,FacultyId")] Department department)
        {
            if (id != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentId))
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
            ViewData["FacultyId"] = new SelectList(_context.Faculties, "FacultyId", "FacultyId", department.FacultyId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Faculty)
                .FirstOrDefaultAsync(m => m.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(Guid id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }
    }
}
