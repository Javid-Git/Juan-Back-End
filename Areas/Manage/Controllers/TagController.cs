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

    public class TagController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TagController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? status, int page = 1)
        {
            IQueryable<Tag> query = _context.Tags.AsQueryable();
            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = _context.Tags.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = _context.Tags.Where(b => !b.IsDeleted);
                }
            }
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            List<Tag> products = await query.Skip((page - 1) * itemcount).Take(itemcount).ToListAsync();
            ViewBag.Status = status;
            return View(PageNatedList<Tag>.Create(page, query, itemcount));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           

            
            if (await _context.Tags.AnyAsync(b => b.Name.ToLower().Trim() == tag.Name.ToLower().Trim() && !b.IsDeleted))
            {
                ModelState.AddModelError("Name", $"{tag.Name} Already Exists");
                return View();
            }

            TempData["success"] = "Product was created successfully";
            tag.CreatedAt = DateTime.UtcNow.AddHours(+4);

            tag.Name = tag.Name.Trim();
            await _context.AddAsync(tag);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            if (id == null)
            {
                return BadRequest();
            }
            Tag dbtag = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
            if (dbtag == null)
            {
                return NotFound();
            }
            return View(dbtag);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Tag tag, int? id)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id == null)
            {
                return BadRequest();
            }
            if (id != tag.Id)
            {
                return BadRequest();
            }
            Tag dbtag = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);


            dbtag.Name = tag.Name.Trim();


            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id, int status, int page)
        {
            ViewBag.Status = status;

            IQueryable<Tag> query = _context.Tags.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Tag dbtag = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);

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
            dbtag.IsDeleted = true;
            dbtag.DeletedAt = DateTime.UtcNow.AddHours(+4);
            _context.SaveChanges();
            return PartialView("_ColorIndexPartial", PageNatedList<Tag>.Create(page, query, itemcount));
        }
        public async Task<IActionResult> Restore(int? id, int status, int page)
        {
            IQueryable<Tag> query = _context.Tags.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Tag dbtag = await _context.Tags.FirstOrDefaultAsync(c => c.Id == id);
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

            dbtag.IsDeleted = false;
            _context.SaveChanges();
            return PartialView("_ColorIndexPartial", PageNatedList<Tag>.Create(page, query, itemcount));
        }

    }
}
