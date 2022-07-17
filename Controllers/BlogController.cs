using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Juan.ViewModels.BlogViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page=1)
        {
            IQueryable<Blog> blogs =  _context.Blogs.AsQueryable();
            List<Tag> tags = await _context.Tags.ToListAsync();
            List<BlogTag> blogTags = await _context.BlogTags.ToListAsync();
            List<BlCategory> blCategories = await _context.BlCategories.ToListAsync();
            List<BlogBlCategory> blogBlCategories = await _context.BlogBlCategories.ToListAsync();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);

            BlogVM blogVM = new BlogVM
            {
                Blogs = PageNatedList<Blog>.Create(page, blogs, itemcount),
                BlogTags = blogTags,
                BlCategories = blCategories,
                BlogBlCategories = blogBlCategories,
                Tags = tags
            };

            return View(blogVM);
        }
        [Authorize(Roles = "User")]

        public async Task<IActionResult> Detail(int? id, int page = 1)
        {
            IQueryable<Blog> blogs =  _context.Blogs.Include(b=>b.BlogTags).AsQueryable();
            List<Tag> tags = await _context.Tags.ToListAsync();
            List<BlCategory> blCategories = await _context.BlCategories.ToListAsync();
            List<BlogBlCategory> blogBlCategories = await _context.BlogBlCategories.ToListAsync();
            Blog blog = new Blog();
            if (id != null)
            {
                blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

            }
            else
            {
                blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(TempData["BlogId"]));
            }
            List<Coment> coments = await _context.Coments.ToListAsync();
            List<ComentReply> comentReplies = await _context.ComentReplies.ToListAsync();
            Coment coment = new Coment();
            if (id != null)
            {
                coment = await _context.Coments.FirstOrDefaultAsync(c => c.BlogId == blog.Id);

            }
            else
            {
                coment = await _context.Coments.FirstOrDefaultAsync(c => c.BlogId == Convert.ToInt32(TempData["BlogId"]));
            }
            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            List<AppUser> users = await _context.Users.ToListAsync();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (User.Identity.IsAuthenticated)
            {
                BlogVM blogVM = new BlogVM
                {
                    Blogs = PageNatedList<Blog>.Create(page, blogs, itemcount),
                    BlCategories = blCategories,
                    BlogBlCategories = blogBlCategories,
                    Tags = tags,
                    Blog = blog,
                    Coments = coments,
                    User = user,
                    Coment = coment,
                    Users = users,
                    ComentReplies = comentReplies
                };
                ViewBag.BlogVM = blogVM;
                return View(blogVM);
            }
            else
            {
                return RedirectToAction("login", "account", new { area = "" });
            }
            
        }
        public async Task<IActionResult> SortByTag(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            List<BlogTag> blogTags = await _context.BlogTags.Where(bl => bl.TagId == id).ToListAsync();
            List<Blog> blogs = new List<Blog>();
            foreach (BlogTag blogTag in blogTags)
            {
                Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == blogTag.BlogId);
                blogs.Add(blog);
            }
            if (blogs == null)
            {
                return NotFound();
            }

            return PartialView("_BlogIndexPartial", blogs);
        }
        public async Task<IActionResult> SortByCategory(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            List<BlogBlCategory> blogBlCategories = await _context.BlogBlCategories.Where(bl => bl.BlCategoryId == id).ToListAsync();
            List<Blog> blogs = new List<Blog>();
            foreach (BlogBlCategory blogBlCategory in blogBlCategories)
            {
                Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == blogBlCategory.BlogId);
                blogs.Add(blog);
            }
            if (blogs == null)
            {
                return NotFound();
            }

            return PartialView("_BlogIndexPartial", blogs);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> PostComent(Coment coment, int? id, string? userId, int? comentId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Detail");
                }
                if (id == null)
                {
                    return NotFound();
                }
                AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (coment.Text == null)
                {
                    return RedirectToAction("Detail");

                }
                if (comentId == null)
                {
                    Coment dbcoment = new Coment
                    {
                        Text = coment.Text,
                        Date = DateTime.UtcNow.AddHours(+4),
                        UserId = userId,
                        BlogId = id
                    };
                    TempData["BlogId"] = id;
                    await _context.Coments.AddAsync(dbcoment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Detail", id);
                }
                else
                {
                    ComentReply dbcomentreply = new ComentReply
                    {
                        Text = coment.Text,
                        Date = DateTime.UtcNow.AddHours(+4),
                        UserId = userId,
                        BlogId = id,
                        ComentId = comentId
                    };
                    TempData["BlogId"] = id;
                    await _context.ComentReplies.AddAsync(dbcomentreply);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Detail", id);
                }
                

            }
            else
            {
                return RedirectToAction("login", "account", new { area = "" });

            }

        }
        public async Task<IActionResult> AddReply(int? id, int? comentId, int page = 1)
        {
            if (comentId == null)
            {
                return BadRequest();
            }
            IQueryable<Blog> blogs = _context.Blogs.Include(b => b.BlogTags).AsQueryable();
            List<Tag> tags = await _context.Tags.ToListAsync();
            List<BlCategory> blCategories = await _context.BlCategories.ToListAsync();
            List<BlogBlCategory> blogBlCategories = await _context.BlogBlCategories.ToListAsync();
            Blog blog = new Blog();
            if (id != null)
            {
                blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

            }
            else
            {
                blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(TempData["BlogId"]));
            }
            List<Coment> coments = await _context.Coments.ToListAsync();
            Coment coment = new Coment();
            if (id != null)
            {
                coment = await _context.Coments.FirstOrDefaultAsync(c => c.BlogId == blog.Id);

            }
            else
            {
                coment = await _context.Coments.FirstOrDefaultAsync(c => c.BlogId == Convert.ToInt32(TempData["BlogId"]));
            }
            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            List<AppUser> users = await _context.Users.ToListAsync();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (User.Identity.IsAuthenticated)
            {
                BlogVM blogVM = new BlogVM
                {
                    Blogs = PageNatedList<Blog>.Create(page, blogs, itemcount),
                    BlCategories = blCategories,
                    BlogBlCategories = blogBlCategories,
                    Tags = tags,
                    Blog = blog,
                    Coments = coments,
                    User = user,
                    Coment = coment,
                    Users = users,
                    PostComentId = comentId
                };
                return PartialView("_BlogComentsPartial", blogVM);

            }
            else
            {
                return RedirectToAction("login", "account", new { area = "" });
            }
            

        }
    }
}
