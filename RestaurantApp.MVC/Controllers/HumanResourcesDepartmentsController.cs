using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.DataAccess;
using RestaurantApp.Data.Models.Domain;

namespace RestaurantApp.MVC.Controllers
{
    [Authorize]
    public class HumanResourcesDepartmentsController : Controller
    {
        private readonly ApplicationDatabase _context;

        public HumanResourcesDepartmentsController(ApplicationDatabase context)
        {
            _context = context;
        }

        // GET: HumanResourcesDepartments
        public async Task<IActionResult> Index()
        {
            var applicationDatabase = _context.HumanResourcesDepartments.Include(h => h.Employer).Include(h => h.Position);
            return View(await applicationDatabase.ToListAsync());
        }

        // GET: HumanResourcesDepartments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var humanResourcesDepartments = await _context.HumanResourcesDepartments
                .Include(h => h.Employer)
                .Include(h => h.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (humanResourcesDepartments == null)
            {
                return NotFound();
            }

            return View(humanResourcesDepartments);
        }

        // GET: HumanResourcesDepartments/Create
        public IActionResult Create()
        {
            ViewData["EmployerId"] = new SelectList(_context.Employers, "Id", "LastName");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmploymentDate,EmployerId,PositionId,Id")] HumanResourcesDepartments humanResourcesDepartments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(humanResourcesDepartments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployerId"] = new SelectList(_context.Employers, "Id", "LastName", humanResourcesDepartments.EmployerId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", humanResourcesDepartments.PositionId);
            return View(humanResourcesDepartments);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var humanResourcesDepartments = await _context.HumanResourcesDepartments.FindAsync(id);
            if (humanResourcesDepartments == null)
            {
                return NotFound();
            }
            ViewData["EmployerId"] = new SelectList(_context.Employers, "Id", "LastName", humanResourcesDepartments.EmployerId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", humanResourcesDepartments.PositionId);
            return View(humanResourcesDepartments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmploymentDate,EmployerId,PositionId,Id")] HumanResourcesDepartments humanResourcesDepartments)
        {
            if (id != humanResourcesDepartments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(humanResourcesDepartments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HumanResourcesDepartmentsExists(humanResourcesDepartments.Id))
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
            ViewData["EmployerId"] = new SelectList(_context.Employers, "Id", "LastName", humanResourcesDepartments.EmployerId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name", humanResourcesDepartments.PositionId);
            return View(humanResourcesDepartments);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var humanResourcesDepartments = await _context.HumanResourcesDepartments
                .Include(h => h.Employer)
                .Include(h => h.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (humanResourcesDepartments == null)
            {
                return NotFound();
            }

            return View(humanResourcesDepartments);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var humanResourcesDepartments = await _context.HumanResourcesDepartments.FindAsync(id);
            _context.HumanResourcesDepartments.Remove(humanResourcesDepartments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HumanResourcesDepartmentsExists(int id)
        {
            return _context.HumanResourcesDepartments.Any(e => e.Id == id);
        }
    }
}
