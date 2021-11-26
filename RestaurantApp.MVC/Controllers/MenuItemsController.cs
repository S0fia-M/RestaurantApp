using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.DataAccess;
using RestaurantApp.Data.Models.Domain;
using RestaurantApp.MVC.ViewModels.MenuItems;

namespace RestaurantApp.MVC.Controllers
{
    [Authorize]
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDatabase _context;

        public class IndexPayload
        {
            public string Name { get; set; }
        }

        public enum SortState
        {
            NameAsc,    // по имени по возрастанию
            NameDesc,   // по имени по убыванию
            PriceAsc, // по цене по возрастанию
            PriceDesc,    // по цене по убыванию
            WeightAsc, // по весу по возрастанию
            WeightDesc // по весу по убыванию
        }

        public MenuItemsController(ApplicationDatabase context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string name,
            SortState sort = SortState.NameDesc)
        {

            ViewData["NameSort"] = sort == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["PriceSort"] = sort == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ViewData["WeightSort"] = sort == SortState.WeightAsc ? SortState.WeightDesc : SortState.WeightAsc;

            IQueryable<MenuItem> menuItems = _context.MenuItems;

            menuItems = sort switch
            {
                SortState.NameDesc => menuItems.OrderByDescending(s => s.PositionName),
                SortState.PriceAsc => menuItems.OrderBy(s => s.Price),
                SortState.PriceDesc => menuItems.OrderByDescending(s => s.Price),
                SortState.WeightAsc => menuItems.OrderBy(s => s.Weight),
                SortState.WeightDesc => menuItems.OrderByDescending(s => s.Weight),
                _ => menuItems.OrderBy(s => s.PositionName),
            };

            if (!String.IsNullOrEmpty(name))
            {
                menuItems = menuItems.Where(p => p.PositionName.Contains(name));
            }

            var list = await menuItems.AsNoTracking().ToListAsync();

            var ingridientsList = new List<string>();
            foreach(var item in list)
            {
                var ingridients = await _context.MenuItemsCompositions
                    .Where(x => x.MenuItemId == item.Id).Select(x => x.CompositionId).ToListAsync();

                string str = "";
                foreach (var i in ingridients)
                {
                    str += $"{(await _context.Compositions.FindAsync(i)).Ingredient}, ";
                    str = str.Substring(0, str.Length - 2);
                }
                ingridientsList.Add(str);
            }

            var vm = new MenuItemListViewModel
            {
                Name = name,
                MenuItems = list,
                Ingridients = ingridientsList
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var ingridients = await _context.Compositions.Select(x => new SelectListItem
            { Text = x.Ingredient, Value = x.Id.ToString() }).
                ToListAsync();
            return View(new CreateMenuItemViewModel { Ingridients = ingridients });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMenuItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var ingridientsIds = vm.Ingridients?.Where(x => x.Selected).Select(x => x.Value).ToList() ?? new List<string>();
                var mapped = new MenuItem() 
                {
                    PositionName = vm.PositionName,
                    Price = vm.Price,
                    Weight = vm.Weight,
                    MenuItemsCompositions = new List<MenuItemsCompositions>() 
                };
                var entity = await _context.AddAsync(mapped);
                await _context.SaveChangesAsync();

                foreach (var x in ingridientsIds)
                {
                   await _context.AddAsync(new MenuItemsCompositions { CompositionId = Convert.ToInt32(x), MenuItemId = entity.Entity.Id }); 
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

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            var ingridients = await _context.Compositions.Select(x => new SelectListItem { Text = x.Ingredient, Value = x.Id.ToString() }).
                 ToListAsync();
            var existing = await _context.MenuItemsCompositions.Where(x => x.MenuItemId == id).ToListAsync();
            ingridients.ForEach(x => x.Selected = existing?.Any(y => y.MenuItemId.ToString() == x.Value) ?? false);
            return View(new CreateMenuItemViewModel
            {
                Ingridients = ingridients,
                Id = menuItem.Id,
                PositionName = menuItem.PositionName,
                Price = menuItem.Price,
                Weight = menuItem.Weight
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateMenuItemViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var ingridientsIds = vm.Ingridients?.Where(x => x.Selected).Select(x => x.Value).ToList() ?? new List<string>();
                    var mapped = new MenuItem()
                    {
                        Id = vm.Id,
                        PositionName = vm.PositionName,
                        Price = vm.Price,
                        Weight = vm.Weight,
                        MenuItemsCompositions = new List<MenuItemsCompositions>()
                    };
                    var existing = await _context.MenuItemsCompositions.Where(x => x.MenuItemId == vm.Id).ToListAsync();
                    _context.MenuItemsCompositions.RemoveRange(existing);
                    _context.Update(mapped);
                    foreach (var x in ingridientsIds)
                    {
                        await _context.AddAsync(new MenuItemsCompositions { 
                            CompositionId = Convert.ToInt32(x), MenuItemId = id });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(vm.Id))
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

            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            var additionalData = await _context.MenuItemsCompositions.Where(x => x.MenuItemId == id).ToListAsync();
            _context.MenuItemsCompositions.RemoveRange(additionalData);
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<FileResult> Export()
        {
            var list = await _context.MenuItems.AsNoTracking().ToListAsync();
            var ingridientsList = new List<string>();
            foreach (var item in list)
            {
                var ingridients = await _context.MenuItemsCompositions
                    .Where(x => x.MenuItemId == item.Id).Select(x => x.CompositionId).ToListAsync();

                string str = "";
                foreach (var i in ingridients)
                {
                    str += $"{(await _context.Compositions.FindAsync(i)).Ingredient}, ";
                    str = str.Substring(0, str.Length - 2);
                }
                ingridientsList.Add(str);
            }

            List<object> entities = await _context.MenuItems.Select(x => new[]
            {
                x.Id.ToString(),
                x.PositionName,
                x.Price.ToString(),
                x.Weight.ToString()
            }).ToListAsync<object>();

            //Insert the Column Names.
            entities.Insert(0, new string[4] { "MenuItemID", "PositionName", "Price", "Weight" });

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < entities.Count; i++) //"Composition"
            {
                string[] customer = (string[])entities[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    //Append data with separator.
                    sb.Append(customer[j] + ',');
                }

                if (i == 0)
                {
                    sb.Append("Composition");
                }
                else
                {
                    sb.Append(ingridientsList[i - 1]);
                }

                //Append new line character.
                sb.Append("\r\n");

            }

            return File(Encoding.Default.GetBytes(sb.ToString()), "text/csv", "Menu.csv");
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
