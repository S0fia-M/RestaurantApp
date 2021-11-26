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
    public class PassportDatasController : Controller
    {
        private readonly ApplicationDatabase db;

        public PassportDatasController(ApplicationDatabase db)
        {
            this.db = db;
        }

        // GET: PassportDatasController
        public async Task<ActionResult> Index() => View(await db.PassportDatas.ToListAsync());       

        // GET: PassportDatasController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var entity = await db.PassportDatas.FindAsync(id);
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
        public async Task<ActionResult> Create(CreatePassportDataViewModel item)
        {
            var newData = new PassportData
            {
                PassportSeries = item.PassportSeries,
                BirthDate = item.BirthDate,
                DepartmentCode = item.DepartmentCode,
                Gender = item.Gender,
                IssuedBy = item.IssuedBy,
                IssuedDate = item.IssuedDate,
                PassportNumber = item.PassportNumber,
                PlaceOfBirth = item.PlaceOfBirth,
            };

            try
            {
                await db.PassportDatas.AddAsync(newData);
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
            var entity = await db.PassportDatas.FindAsync(id);
            return View(entity);
        }

        // POST: PassportDatasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PassportData updated)
        {;
            try
            {
                updated.Id = id;
                db.PassportDatas.Update(updated);
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
            var entity = await db.PassportDatas.FindAsync(id);
            return View(entity);
        }

        // POST: PassportDatasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var entity = await db.PassportDatas.FindAsync(id);
                db.PassportDatas.Remove(entity);
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
