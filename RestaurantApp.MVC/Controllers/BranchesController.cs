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
    public class BranchesController : Controller
    {
        private readonly ApplicationDatabase _context;

        public BranchesController(ApplicationDatabase context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDatabase = _context.Branches.Include(b => b.Employer).Include(b => b.WarehouseItem);
            return View(await applicationDatabase.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Employer)
                .Include(b => b.WarehouseItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        public async Task<IActionResult> Create()
        {
            var employers = await _context.Employers.Select(x => new { Id = x.Id, Name = $"{x.LastName} {x.FirstName}" }).ToListAsync();
            ViewData["EmployerId"] = new SelectList(employers, "Id", "Name");
            ViewData["WarehouseItemId"] = new SelectList(_context.WarehouseItems, "Id", "Address");
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Address,PhoneNumber,EmployerId,WarehouseItemId,Id")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var employers = await _context.Employers.Select(x => new { Id = x.Id, Name = $"{x.LastName} {x.FirstName}" }).ToListAsync();
            ViewData["EmployerId"] = new SelectList(employers, "Id", "Name");
            ViewData["WarehouseItemId"] = new SelectList(_context.WarehouseItems, "Id", "Address");
            return View(branch);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            var employers = await _context.Employers.Select(x => new { Id = x.Id, Name = $"{x.LastName} {x.FirstName}" }).ToListAsync();
            ViewData["EmployerId"] = new SelectList(employers, "Id", "Name");
            ViewData["WarehouseItemId"] = new SelectList(_context.WarehouseItems, "Id", "Address"); ;
            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Address,PhoneNumber,EmployerId,WarehouseItemId,Id")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            var employers = await _context.Employers.Select(x => new { Id = x.Id, Name = $"{x.LastName} {x.FirstName}" }).ToListAsync();
            ViewData["EmployerId"] = new SelectList(employers, "Id", "Name");
            ViewData["WarehouseItemId"] = new SelectList(_context.WarehouseItems, "Id", "Address");
            return View(branch);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches
                .Include(b => b.Employer)
                .Include(b => b.WarehouseItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branches.Any(e => e.Id == id);
        }
    }
}
