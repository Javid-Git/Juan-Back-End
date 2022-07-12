﻿using Juan.DAL;
using Juan.Models;
using Juan.ViewModels.BlogViewModels;
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
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            List<Tag> tags = await _context.Tags.ToListAsync();
            List<BlogTag> blogTags = await _context.BlogTags.ToListAsync();
            List<BlCategory> blCategories = await _context.BlCategories.ToListAsync();
            List<BlogBlCategory> blogBlCategories = await _context.BlogBlCategories.ToListAsync();

            BlogVM blogVM = new BlogVM
            {
                Blogs = blogs,
                BlogTags = blogTags,
                BlCategories = blCategories,
                BlogBlCategories = blogBlCategories,
                Tags = tags
            };

            return View(blogVM);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            List<Blog> blogs = await _context.Blogs.Include(b=>b.BlogTags).ToListAsync();
            List<Tag> tags = await _context.Tags.ToListAsync();
            List<BlCategory> blCategories = await _context.BlCategories.ToListAsync();
            List<BlogBlCategory> blogBlCategories = await _context.BlogBlCategories.ToListAsync();
            Blog blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            BlogVM blogVM = new BlogVM
            {
                Blogs = blogs,
                BlCategories = blCategories,
                BlogBlCategories = blogBlCategories,
                Tags = tags,
                Blog = blog
            };
            return View(blogVM);
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
    }
}
