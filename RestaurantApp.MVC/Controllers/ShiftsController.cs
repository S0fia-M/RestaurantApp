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
using RestaurantApp.MVC.ViewModels.Shifts;

namespace RestaurantApp.MVC.Controllers
{
    [Authorize]
    public class ShiftsController : Controller
    {
        private readonly ApplicationDatabase _context;

        public ShiftsController(ApplicationDatabase context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDatabase = _context.Shifts.Include(s => s.Employer);
            return View(await applicationDatabase.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var employers = await _context.Employers.Select(x => new { Id = x.Id, Name = $"{x.LastName} {x.FirstName}" }).ToListAsync();
            ViewData["EmployerId"] = new SelectList(employers, "Id", "Name");
            var alreadyUsedOrders = await _context.ShiftsOrders.Select(x => x.OrderId).ToListAsync();
            var availableOrders = await _context.Orders
                .Where(x => !alreadyUsedOrders.Contains(x.Id))
                .Select(x => new SelectListItem { Text = $"Заказ № {x.Id}", Value = x.Id.ToString() }).ToListAsync();
            return View(new CreateShiftViewModel { Orders = availableOrders});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateShiftViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var orderIds = vm.Orders?.Where(x => x.Selected).Select(x => x.Value).ToList() ?? new List<string>();
                var shiftMapped = new Shift()
                {
                    EmployerId = vm.EmployerId,
                    Profit = vm.Profit,
                    ShiftDate = vm.ShiftDate,
                    ShiftsOrders = new List<ShiftsOrders>()
                };
                var entity = await _context.AddAsync(shiftMapped);
                await _context.SaveChangesAsync();

                foreach (var x in orderIds)
                {
                    await _context.AddAsync(new ShiftsOrders { OrderId = Convert.ToInt32(x), ShiftId = entity.Entity.Id });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var employers = await _context.Employers.Select(x => new { Id = x.Id, Name = $"{x.LastName} {x.FirstName}" }).ToListAsync();
            ViewData["EmployerId"] = new SelectList(employers, "Id", "Name", vm.EmployerId);
            var alreadyUsedOrders = await _context.ShiftsOrders.Select(x => x.OrderId).ToListAsync();
            var availableOrders = await _context.Orders
                .Where(x => !alreadyUsedOrders.Contains(x.Id))
                .Select(x => new SelectListItem { Text = $"Заказ № {x.Id}", Value = x.Id.ToString() }).ToListAsync();

            return View(new CreateShiftViewModel {EmployerId = vm.EmployerId, ShiftDate = vm.ShiftDate, Profit = vm.Profit, Orders = availableOrders });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            var employers = await _context.Employers.Select(x => new { Id = x.Id, Name = $"{x.LastName} {x.FirstName}" }).ToListAsync();
            ViewData["EmployerId"] = new SelectList(employers, "Id", "Name");
            var alreadyUsedOrders = await _context.ShiftsOrders.Select(x => x.OrderId).ToListAsync();
            var ownedOrders = await _context.ShiftsOrders.Where(x => x.ShiftId == id).Select(x => x.OrderId).ToListAsync();
            var ordersToRemove = alreadyUsedOrders.Except(ownedOrders);
            var availableOrders = await _context.Orders
                .Where(x => !ordersToRemove.Contains(x.Id) )
                .Select(x => new SelectListItem { Text = $"Заказ № {x.Id}", Value = x.Id.ToString(), Selected = ownedOrders.Contains(x.Id) }).ToListAsync();
            return View(new CreateShiftViewModel { Id = shift.Id, EmployerId = shift.EmployerId, ShiftDate = shift.ShiftDate, Profit = shift.Profit, Orders = availableOrders });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateShiftViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var orderIds = vm.Orders.Where(x => x.Selected).Select(x => x.Value);
                    var shiftMapped = new Shift() 
                    { 
                        Id = vm.Id,
                        EmployerId = vm.EmployerId,
                        Profit = vm.Profit,
                        ShiftDate = vm.ShiftDate,
                        ShiftsOrders = new List<ShiftsOrders>() 
                    };
                    var existingOrders = await _context.ShiftsOrders.Where(x => x.ShiftId == vm.Id).ToListAsync();
                    _context.ShiftsOrders.RemoveRange(existingOrders);
                    foreach (var x in orderIds)
                    {
                        await _context.ShiftsOrders.AddAsync(new ShiftsOrders { OrderId = Convert.ToInt32(x), ShiftId = vm.Id });
                    }
                    _context.Update(shiftMapped);
                    await _context.SaveChangesAsync(); ;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShiftExists(vm.Id))
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
            var alreadyUsedOrders = await _context.ShiftsOrders.Select(x => x.OrderId).ToListAsync();
            var ownedOrders = await _context.ShiftsOrders.Where(x => x.ShiftId == id).Select(x => x.OrderId).ToListAsync();
            var ordersToRemove = alreadyUsedOrders.Except(ownedOrders);
            var availableOrders = await _context.Orders
                .Where(x => !ordersToRemove.Contains(x.Id))
                .Select(x => new SelectListItem { Text = $"Заказ № {x.Id}", Value = x.Id.ToString(), Selected = ownedOrders.Contains(x.Id) }).ToListAsync();
            return View(new CreateShiftViewModel { Id = vm.Id, EmployerId = vm.EmployerId, ShiftDate = vm.ShiftDate, Profit = vm.Profit, Orders = availableOrders });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shifts
                .Include(s => s.Employer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            var shiftsOrders = await _context.ShiftsOrders.Where(x => x.ShiftId == id).ToListAsync();
            _context.Shifts.Remove(shift);
            _context.ShiftsOrders.RemoveRange(shiftsOrders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShiftExists(int id)
        {
            return _context.Shifts.Any(e => e.Id == id);
        }
    }
}
