using Juan.DAL;
using Juan.Extensions;
using Juan.Helpers;
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
    [Area("manage")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int? status, int page = 1)
        {
            IQueryable<Blog> query = _context.Blogs.AsQueryable();
            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = _context.Blogs.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = _context.Blogs.Where(b => !b.IsDeleted);
                }
            }
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            List<Blog> blogs = await query.Skip((page - 1) * itemcount).Take(itemcount).ToListAsync();
            ViewBag.Status = status;
            return View(PageNatedList<Blog>.Create(page, query, itemcount));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.BlCategories = await _context.BlCategories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.BlCategories = await _context.BlCategories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
           
            if (blog.FormImage != null)
            {
                if (blog.FormImage.CheckFileType("image/*"))
                {
                    ModelState.AddModelError("File", "The uploaded file should be an image (jpg, png, jpeg)");
                    return View();
                }
                if (blog.FormImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("File", "The uploaded file should not exceed 200Kb ");
                    return View();
                }
                blog.Image = await blog.FormImage.CreateAsync(_env, "assets", "img", "blog");

            }
            if (!ModelState.IsValid) return View();

            if (blog.TagIds == null)
            {
                ModelState.AddModelError("TagIds", "Tag Id s Is Required");
                return View();
            }


            if (blog.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "You Must Select Correct");
                return View();
            }

            if (!await _context.BlCategories.AnyAsync(c => !c.IsDeleted && c.Id == blog.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select Correct Category");
                return View();
            }
            

            TempData["success"] = "Blog was created successfully";
            blog.CreatedAt = DateTime.UtcNow.AddHours(+4);

            blog.Name = blog.Name.Trim();
            blog.Author = blog.Author;
            blog.Text = blog.Text;

            await _context.AddAsync(blog);
            _context.SaveChanges();

            BlogBlCategory blogBlCategory = new BlogBlCategory
            {
                BlogId = blog.Id,
                BlCategoryId = blog.CategoryId
            };
            await _context.BlogBlCategories.AddAsync(blogBlCategory);

            foreach (var tag in blog.TagIds)
            {
                BlogTag blogTag = new BlogTag
                {
                    BlogId = blog.Id,
                    TagId = tag
                };
                await _context.BlogTags.AddAsync(blogTag);

            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.BlCategories = await _context.BlCategories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();

            if (id == null)
            {
                return BadRequest();
            }
            Blog dbblog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
            if (dbblog == null)
            {
                return NotFound();
            }
            return View(dbblog);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Blog blog, int? id)
        {
            ViewBag.BlCategories = await _context.BlCategories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();


            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id == null)
            {
                return BadRequest();
            }
            if (id != blog.Id)
            {
                return BadRequest();
            }
            Blog dbblog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
            List<BlogTag> dbtags = await _context.BlogTags.Where(t => t.BlogId == id).ToListAsync();
            BlogBlCategory dbcategory = await _context.BlogBlCategories.FirstOrDefaultAsync(t => t.BlogId == id);
            foreach (var item in dbtags)
            {
                _context.BlogTags.Remove(item);
                _context.SaveChanges();
            }
            _context.BlogBlCategories.Remove(dbcategory);
            //Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == product.BrandId);
            //if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.Id == product.BrandId))
            //{
            //    ModelState.AddModelError("BrandId", "Select Correct Brand");
            //    return View();
            //}

            if (blog.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "You Must Select Correct");
                return View();
            }

            if (!await _context.BlCategories.AnyAsync(c => !c.IsDeleted && c.Id == blog.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select Correct Category");
                return View();
            }
            if (blog.FormImage != null)
            {
                if (blog.FormImage.CheckFileType("image/*"))
                {
                    ModelState.AddModelError("File", "The uploaded file should be an image (jpg, png, jpeg)");
                    return View();
                }
                if (blog.FormImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("File", "The uploaded file should not exceed 200Kb ");
                    return View();
                }
            }

            dbblog.Name = blog.Name.Trim();
            dbblog.Author = blog.Author;
            dbblog.Text = blog.Text;


            if (blog.FormImage != null)
            {
                FileHelper.DeleteFile(_env, dbblog.Image, "assets", "img", "blog");
                dbblog.Image = await blog.FormImage.CreateAsync(_env, "assets", "img", "blog");

            };
           
            _context.SaveChanges();
            BlogBlCategory blogBlCategory = new BlogBlCategory
            {
                BlogId = blog.Id,
                BlCategoryId = blog.CategoryId
            };
            await _context.BlogBlCategories.AddAsync(blogBlCategory);

            foreach (var tag in blog.TagIds)
            {
                BlogTag blogTag = new BlogTag
                {
                    BlogId = blog.Id,
                    TagId = tag
                };
                await _context.BlogTags.AddAsync(blogTag);

            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id, int status, int page)
        {
            ViewBag.Status = status;

            IQueryable<Blog> query = _context.Blogs.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Blog dbblog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);

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
            dbblog.IsDeleted = true;
            dbblog.DeletedAt = DateTime.UtcNow.AddHours(+4);
            _context.SaveChanges();
            return PartialView("_ProductIndexPartial", PageNatedList<Blog>.Create(page, query, itemcount));
        }
        public async Task<IActionResult> Restore(int? id, int status, int page)
        {
            IQueryable<Blog> query = _context.Blogs.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Blog dbblog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
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

            dbblog.IsDeleted = false;
            _context.SaveChanges();
            return PartialView("_ProductIndexPartial", PageNatedList<Blog>.Create(page, query, itemcount));
        }
    }
}
