using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDepartment_lab5.Data;
using UniversityDepartment_lab5.Models;
using UniversityDepartment_lab5.ViewModels.Subjects;
using UniversityDepartment_lab5.ViewModels;

namespace UniversityDepartment_lab5.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly int pageSize = 50;
        private readonly UniversityDbContext _context;

        public SubjectsController(UniversityDbContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index(int page = 1, string Name = "", string ReportingType = "", int LectureHours = 0, int PracticalHours = 0, int LabHours = 0, SortState sortOrder = SortState.No)
        {
            IQueryable<Subject> subjectsContext = _context.Subjects;

            subjectsContext = SortAndFilter(subjectsContext, sortOrder, Name ?? "", ReportingType ?? "", LectureHours, PracticalHours, LabHours);

            var count = await subjectsContext.CountAsync();

            var subjects = await subjectsContext
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            SubjectsViewModel viewModel = new SubjectsViewModel()
            {
                Subjects = subjects,
                Name = Name,
                ReportingType = ReportingType,
                LectureHours = LectureHours,
                PracticalHours = PracticalHours,
                LabHours = LabHours,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SubjectSortViewModel(sortOrder)
            };

            return View(viewModel);
        }

        private IQueryable<Subject> SortAndFilter(IQueryable<Subject> subjectsContext, SortState sortOrder, string Name, string ReportingType, int LectureHours, int PracticalHours, int LabHours)
        {
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    subjectsContext = subjectsContext.OrderBy(s => s.Name);
                    break;
                case SortState.NameDesc:
                    subjectsContext = subjectsContext.OrderByDescending(s => s.Name);
                    break;
                case SortState.ReportingTypeAsc:
                    subjectsContext = subjectsContext.OrderBy(s => s.ReportingType);
                    break;
                case SortState.ReportingTypeDesc:
                    subjectsContext = subjectsContext.OrderByDescending(s => s.ReportingType);
                    break;
                case SortState.LectureHoursAsc:
                    subjectsContext = subjectsContext.OrderBy(s => s.LectureHours);
                    break;
                case SortState.LectureHoursDesc:
                    subjectsContext = subjectsContext.OrderByDescending(s => s.LectureHours);
                    break;
                case SortState.LabHoursAsc:
                    subjectsContext = subjectsContext.OrderBy(s => s.LabHours);
                    break;
                case SortState.LabHoursDesc:
                    subjectsContext = subjectsContext.OrderByDescending(s => s.LabHours);
                    break;
                case SortState.PracticalHoursAsc:
                    subjectsContext = subjectsContext.OrderBy(s => s.PracticalHours);
                    break;
                case SortState.PracticalHoursDesc:
                    subjectsContext = subjectsContext.OrderByDescending(s => s.PracticalHours);
                    break;
            }
            if (Name != "")
            {
                subjectsContext = subjectsContext.Where(c => c.Name == Name);
            }
            if (ReportingType != "")
            {
                subjectsContext = subjectsContext.Where(c => c.ReportingType == ReportingType);
            }
            if (LectureHours != 0)
            {
                subjectsContext = subjectsContext.Where(c => c.LectureHours == LectureHours);
            }
            if (PracticalHours != 0)
            {
                subjectsContext = subjectsContext.Where(c => c.PracticalHours == PracticalHours);
            }
            if (LabHours != 0)
            {
                subjectsContext = subjectsContext.Where(c => c.LabHours == LabHours);
            }
            return subjectsContext;
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubjectId,Name,LectureHours,PracticalHours,LabHours,ReportingType")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.SubjectId = Guid.NewGuid();
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SubjectId,Name,LectureHours,PracticalHours,LabHours,ReportingType")] Subject subject)
        {
            if (id != subject.SubjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.SubjectId))
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
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.SubjectId == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(Guid id)
        {
            return _context.Subjects.Any(e => e.SubjectId == id);
        }
    }
}
