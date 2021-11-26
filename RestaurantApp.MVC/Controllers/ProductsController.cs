using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Data.DataAccess;
using RestaurantApp.Data.Models.Domain;
using RestaurantApp.MVC.ViewModels.PassportData;
using System.Threading.Tasks;

namespace RestaurantApp.MVC.Controllers
{
    [Authorize(Roles = "Admin, User")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDatabase db;

        public ProductsController(ApplicationDatabase db)
        {
            this.db = db;
        }

        // GET: PassportDatasController
        public async Task<ActionResult> Index() => View(await db.Products.ToListAsync());       

        // GET: PassportDatasController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var entity = await db.Products.FindAsync(id);
            return View(entity);
        }

        // GET: PassportDatasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PassportDatasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product item)
        {
            try
            {
                await db.Products.AddAsync(item);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PassportDatasController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var entity = await db.Products.FindAsync(id);
            return View(entity);
        }

        // POST: PassportDatasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product updated)
        {;
            try
            {
                updated.Id = id;
                db.Products.Update(updated);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PassportDatasController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await db.Products.FindAsync(id);
            return View(entity);
        }

        // POST: PassportDatasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Product product)
        {
            try
            {
                var entity = await db.Products.FindAsync(id);
                db.Products.Remove(entity);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
