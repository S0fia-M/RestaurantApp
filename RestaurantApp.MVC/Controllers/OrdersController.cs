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
using RestaurantApp.MVC.ViewModels.Orders;

namespace RestaurantApp.MVC.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDatabase _context;

        public OrdersController(ApplicationDatabase context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var menu = await _context.MenuItems.Select(x => new SelectListItem { Text = x.PositionName, Value = x.Id.ToString() }).
                ToListAsync();
            var orderNumber = await _context.Orders.CountAsync() + 1;
            return View(new CreateOrderViewModel { OrderId = orderNumber, MenuItems = menu });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var menuItemsIds = vm.MenuItems?.Where(x => x.Selected).Select(x => x.Value).ToList() ?? new List<string>();
                var orderMapped = new Order() { OrdersMenuItems = new List<OrdersMenuItems>() };
                var entity = await _context.AddAsync(orderMapped);
                await _context.SaveChangesAsync();

                foreach (var x in menuItemsIds)
                {
                    await _context.AddAsync(new OrdersMenuItems { MenuItemId = Convert.ToInt32(x), OrderId = entity.Entity.Id });
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var menu = await _context.MenuItems.Select(x => new SelectListItem { Text = x.PositionName, Value = x.Id.ToString() }).
                 ToListAsync();
            var existingOrderMenuItems = await _context.OrdersMenuItems.Where(x => x.OrderId == id).ToListAsync();
            menu.ForEach(x => x.Selected = existingOrderMenuItems?.Any(y => y.MenuItemId.ToString() == x.Value) ?? false);
            return View(new CreateOrderViewModel { OrderId = order.Id, MenuItems = menu });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateOrderViewModel vm)
        {
            var vmId = Convert.ToInt32(vm.OrderId);
            if (id != vmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var menuItemsIds = vm.MenuItems.Where(x => x.Selected).Select(x => x.Value);
                    var orderMapped = new Order() { Id = vmId, OrdersMenuItems = new List<OrdersMenuItems>() };
                    var existingMenuItems = await _context.OrdersMenuItems.Where(x => x.OrderId == vmId).ToListAsync();
                    _context.OrdersMenuItems.RemoveRange(existingMenuItems);
                    foreach (var x in menuItemsIds)
                    {
                        await _context.OrdersMenuItems.AddAsync(new OrdersMenuItems { MenuItemId = Convert.ToInt32(x), OrderId = vmId });
                    }
                    _context.Update(orderMapped);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(vmId))
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
            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            var ordersMenuItems = await _context.OrdersMenuItems.Where(x => x.OrderId == id).ToListAsync();
            _context.Orders.Remove(order);
            _context.OrdersMenuItems.RemoveRange(ordersMenuItems);
           
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
