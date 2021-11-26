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
using RestaurantApp.MVC.ViewModels.WarehouseItems;

namespace RestaurantApp.MVC.Controllers
{
    [Authorize]
    public class WarehouseItemsController : Controller
    {
        private readonly ApplicationDatabase _context;

        public WarehouseItemsController(ApplicationDatabase context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.WarehouseItems.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            var products = await _context.Products.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToListAsync();
            return View(new CreateWarehouseItemViewModel { Products = products });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWarehouseItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var productsIds = vm.Products?.Where(x => x.Selected).Select(x => x.Value).ToList() ?? new List<string>();
                var warehouseItemMapped = new WarehouseItem() 
                { 
                    Address = vm.Address,
                    Expenses = vm.Expenses,
                    Provider = vm.Provider,
                    WarehouseItemsProducts = new List<WarehouseItemsProducts>() 
                };
                var entity = await _context.AddAsync(warehouseItemMapped);
                await _context.SaveChangesAsync();

                foreach (var x in productsIds)
                {
                    await _context.AddAsync(new WarehouseItemsProducts { ProductId = Convert.ToInt32(x), WarehouseItemId = entity.Entity.Id });
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

            var warehouseItem = await _context.WarehouseItems.FindAsync(id);
            if (warehouseItem == null)
            {
                return NotFound();
            }

            var products = await _context.Products.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToListAsync();
            var existingProducts = await _context.WarehouseItemsProducts.Where(x => x.WarehouseItemId == id).ToListAsync();
            products.ForEach(x => x.Selected = existingProducts?.Any(y => y.ProductId.ToString() == x.Value) ?? false);
            return View(new CreateWarehouseItemViewModel 
            { Id = warehouseItem.Id, Address = warehouseItem.Address, Expenses = warehouseItem.Expenses, Provider = warehouseItem.Provider, Products = products });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateWarehouseItemViewModel vm)
        {
            var vmId = Convert.ToInt32(vm.Id);
            if (id != vmId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productIds = vm.Products.Where(x => x.Selected).Select(x => x.Value);
                    var warehouseItemMapped = new WarehouseItem()
                    {
                        Id = id,
                        Address = vm.Address,
                        Expenses = vm.Expenses,
                        Provider = vm.Provider,
                        WarehouseItemsProducts = new List<WarehouseItemsProducts>()
                    };
                    var existingProductItems = await _context.WarehouseItemsProducts.Where(x => x.WarehouseItemId == vmId).ToListAsync();
                    _context.WarehouseItemsProducts.RemoveRange(existingProductItems);
                    foreach (var x in productIds)
                    {
                        await _context.WarehouseItemsProducts.AddAsync(new WarehouseItemsProducts { ProductId = Convert.ToInt32(x), WarehouseItemId = vmId });
                    }
                    _context.Update(warehouseItemMapped);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseItemExists(vmId))
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

            var warehouseItem = await _context.WarehouseItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouseItem == null)
            {
                return NotFound();
            }

            return View(warehouseItem);
        }

        // POST: WarehouseItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouseItem = await _context.WarehouseItems.FindAsync(id);
            var warehouseItemsProducts = await _context.WarehouseItemsProducts.Where(x => x.WarehouseItemId == id).ToListAsync();
            _context.WarehouseItems.Remove(warehouseItem);
            _context.WarehouseItemsProducts.RemoveRange(warehouseItemsProducts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseItemExists(int id)
        {
            return _context.WarehouseItems.Any(e => e.Id == id);
        }
    }
}
