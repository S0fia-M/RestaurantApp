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

namespace RestaurantApp.MVC.Controllers
{
    [Authorize]
    public class EmployersController : Controller
    {
        private readonly ApplicationDatabase _context;

        public enum SortState
        {
            NameAsc,    // по имени по возрастанию
            NameDesc,   // по имени по убыванию
            LastNameAsc, // по цене по возрастанию
            LastNameDesc,    // по цене по убыванию
            MiddleNameAsc, // по весу по возрастанию
            MiddleNameDesc // по весу по убыванию
        }

        public EmployersController(ApplicationDatabase context)
        {
            _context = context;
        }

        // GET: Employers
        public async Task<IActionResult> Index(string lastName, SortState sort = SortState.NameDesc)
        {
            ViewData["FirstNameSort"] = sort == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["LastNameSort"] = sort == SortState.LastNameAsc ? SortState.LastNameDesc : SortState.LastNameAsc;
            ViewData["MiddleNameSort"] = sort == SortState.MiddleNameAsc ? SortState.MiddleNameDesc : SortState.MiddleNameAsc;

            var employers = _context.Employers.Select(x=>x);

            employers = sort switch
            {
                SortState.NameDesc => employers.OrderByDescending(s => s.FirstName),
                SortState.LastNameAsc => employers.OrderBy(s => s.LastName),
                SortState.LastNameDesc => employers.OrderByDescending(s => s.LastName),
                SortState.MiddleNameAsc => employers.OrderBy(s => s.MiddleName),
                SortState.MiddleNameDesc => employers.OrderByDescending(s => s.MiddleName),
                _ => employers.OrderBy(s => s.FirstName),
            };

            if (!String.IsNullOrEmpty(lastName))
            {
                employers = employers.Where(p => p.LastName.Contains(lastName));
            }

            return View(await employers.Include(e => e.PassportData).ToListAsync());
        }

        // GET: Employers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers
                .Include(e => e.PassportData)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        public IActionResult Create()
        {
            ViewData["PassportData"] = new SelectList(_context.PassportDatas, "Id", "PassportNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,MiddleName,PassportDataId,Id")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PassportDataId"] = new SelectList(_context.PassportDatas, "Id", "Id", employer.PassportDataId);
            return View(employer);
        }

        // GET: Employers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers.FindAsync(id);
            if (employer == null)
            {
                return NotFound();
            }
            ViewData["PassportData"] = new SelectList(_context.PassportDatas, "Id", "PassportNumber", employer.PassportDataId);
            return View(employer);
        }

        // POST: Employers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FirstName,LastName,MiddleName,PassportDataId,Id")] Employer employer)
        {
            if (id != employer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployerExists(employer.Id))
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
            ViewData["PassportDataId"] = new SelectList(_context.PassportDatas, "Id", "Id", employer.PassportDataId);
            return View(employer);
        }

        // GET: Employers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employer = await _context.Employers
                .Include(e => e.PassportData)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employer == null)
            {
                return NotFound();
            }

            return View(employer);
        }

        // POST: Employers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employer = await _context.Employers.FindAsync(id);
            _context.Employers.Remove(employer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<FileResult> Export()
        {
            List<object> entities = await _context.Employers.Include(x => x.PassportData).Select(x => new[]
            {
                x.Id.ToString(),
                x.FirstName,
                x.LastName,
                x.MiddleName,
                x.PassportData.BirthDate.ToString(),
                x.PassportData.PassportNumber,
                x.PassportData.PassportSeries,
                x.PassportData.IssuedBy,
                x.PassportData.IssuedDate.ToString(),
                x.PassportData.PlaceOfBirth,
                x.PassportData.DepartmentCode
            }).ToListAsync<object>();

            //Insert the Column Names.
            entities.Insert(0, new string[11] { "EmployerId", "FirstName", "LastName", "MiddleName", "BirthDate",
                "PassportNumber","PassportSeries","IssuedBy", "IssuedDate", "PlaceOfBirth", "DepartmentCode" });

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < entities.Count; i++)
            {
                string[] customer = (string[])entities[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    //Append data with separator.
                    sb.Append(customer[j] + ',');
                }

                //Append new line character.
                sb.Append("\r\n");

            }

            return File(Encoding.Default.GetBytes(sb.ToString()), "text/csv", "Employers.csv");
        }

        private bool EmployerExists(int id)
        {
            return _context.Employers.Any(e => e.Id == id);
        }
    }
}
