using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class SizeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SizeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? status, int page = 1)
        {
            IQueryable<Size> query = _context.Sizes.AsQueryable();
            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = _context.Sizes.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = _context.Sizes.Where(b => !b.IsDeleted);
                }
            }
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            List<Size> products = await query.Skip((page - 1) * itemcount).Take(itemcount).ToListAsync();
            ViewBag.Status = status;
            return View(PageNatedList<Size>.Create(page, query, itemcount));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           

            
            if (await _context.Sizes.AnyAsync(b => b.Number == size.Number && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{size.Number} Already Exists");
                return View();
            }

            TempData["success"] = "Product was created successfully";
            size.CreatedAt = DateTime.UtcNow.AddHours(+4);

            size.Number = size.Number;
            await _context.AddAsync(size);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            Size dbsize = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (dbsize == null)
            {
                return NotFound();
            }
            return View(dbsize);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Size size, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id == null)
            {
                return BadRequest();
            }
            if (id != size.Id)
            {
                return BadRequest();
            }
            Size dbsize = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);


            dbsize.Number = size.Number;


            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id, int status, int page)
        {
            ViewBag.Status = status;

            IQueryable<Size> query = _context.Sizes.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Size dbsize = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);

            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }
            dbsize.IsDeleted = true;
            dbsize.DeletedAt = DateTime.UtcNow.AddHours(+4);
            _context.SaveChanges();
            return PartialView("_SizeIndexPartial", PageNatedList<Size>.Create(page, query, itemcount));
        }
        public async Task<IActionResult> Restore(int? id, int status, int page)
        {
            IQueryable<Size> query = _context.Sizes.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Size dbsize = await _context.Sizes.FirstOrDefaultAsync(c => c.Id == id);
            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = query.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = query.Where(b => !b.IsDeleted);
                }
            }
            ViewBag.Status = status;

            dbsize.IsDeleted = false;
            _context.SaveChanges();
            return PartialView("_SizeIndexPartial", PageNatedList<Size>.Create(page, query, itemcount));
        }

    }
}
