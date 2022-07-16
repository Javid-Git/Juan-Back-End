using Juan.DAL;
using Juan.Extensions;
using Juan.Helpers;
using Juan.Models;
using Juan.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Areas.Manage.Controllers
{
    [Area("manage")]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public class ProductController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index(int? status, int page = 1)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();
            if (status != null && status > 0)
            {
                if (status == 1)
                {
                    query = _context.Products.Where(b => b.IsDeleted);
                }
                else if (status == 2)
                {
                    query = _context.Products.Where(b => !b.IsDeleted);
                }
            }
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            List<Product> products = await query.Skip((page - 1) * itemcount).Take(itemcount).ToListAsync();
            ViewBag.Status = status;
            return View(PageNatedList<Product>.Create(page, query, itemcount));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Tags = await _context.Tags.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            //ViewBag.Categories = await _context.Categories.ToListAsync();
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            //if (product.MainFormImage == null )
            //{
            //    ModelState.AddModelError("File", "You should add an image");
            //    return View();

            //}


            //if (await _context.Products.AnyAsync(b => b.Name.ToLower().Trim() == product.Name.ToLower().Trim() && !b.IsDeleted))
            //{
            //    ModelState.AddModelError("Name", $"{product.Name} Already Exists");
            //    return View();
            //}
            if (product.MainFormImage != null)
            {
                if (product.MainFormImage.CheckFileType("image/*"))
                {
                    ModelState.AddModelError("File", "The uploaded file should be an image (jpg, png, jpeg)");
                    return View();
                }
                if (product.MainFormImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("File", "The uploaded file should not exceed 200Kb ");
                    return View();
                }
                product.MainImage = await product.MainFormImage.CreateAsync(_env, "assets", "img", "product");

            }
            if (!ModelState.IsValid) return View();

            if (product.TagIds == null && product.Count <= 0)
            {
                ModelState.AddModelError("TagIds", "Tag Id s Is Required");
                return View();
            }
            

            if (product.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "You Must Select Correct");
                return View();
            }

            if (!await _context.Categories.AnyAsync(c => !c.IsDeleted &&  c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select Correct Category");
                return View();
            }

            if (product.ColorIds.Count() != product.SizeIds.Count() || product.ColorIds.Count() != product.Counts.Count() || product.SizeIds.Count() != product.Counts.Count())
            {
                ModelState.AddModelError("", "Incorrect List");
                return View();
            }

            List<ProductColorSize> productColorSizes = new List<ProductColorSize>();

            for (int i = 0; i < product.ColorIds.Count(); i++)
            {
                if (!await _context.Colors.AnyAsync(c => c.Id == product.ColorIds[i]))
                {
                    ModelState.AddModelError("", $"this color id {product.ColorIds[i]} is Incorrect");
                    return View();
                }

                if (!await _context.Sizes.AnyAsync(c => c.Id == product.SizeIds[i]))
                {
                    ModelState.AddModelError("", $"this size id {product.SizeIds[i]} is Incorrect");
                    return View();
                }

                if (product.Counts[i] < 0)
                {
                    ModelState.AddModelError("", "Count is Incorrect");
                    return View();
                }

                ProductColorSize productColorSize = new ProductColorSize
                {
                    ColorId = product.ColorIds[i],
                    SizeId = product.SizeIds[i],
                    Count = product.Counts[i]
                };

                productColorSizes.Add(productColorSize);
            }

            product.ProductColorSizes = productColorSizes;

            //List<ProductTag> productTags = new List<ProductTag>();

            //foreach (int tagId in product.TagIds)
            //{
            //    if (!await _context.Tags.AnyAsync(t => t.Id == tagId))
            //    {
            //        ModelState.AddModelError("TagIds", $"Tag {tagId} IsCorrect");
            //        return View();
            //    }

            //    ProductTag productTag = new ProductTag
            //    {
            //        TagId = tagId
            //    };

            //    productTags.Add(productTag);
            //}

            //product.ProductTags = productTags;

            TempData["success"] = "Product was created successfully";
            product.CreatedAt = DateTime.UtcNow.AddHours(+4);

            product.Name = product.Name.Trim();
            product.Price = product.Price;
            product.DiscountedPrice = product.DiscountedPrice;
            product.Count = product.Count;
            product.Describtion = product.Describtion;
            product.Count = productColorSizes.Sum(x => x.Count);

            await _context.AddAsync(product);
            _context.SaveChanges();
            if (product.DetailFormImages != null)
            {
                foreach (IFormFile image in product.DetailFormImages)
                {
                    if (image != null)
                    {
                        if (image.CheckFileType("image/*"))
                        {
                            ModelState.AddModelError("File", "The uploaded file should be an image (jpg, png, jpeg)");
                            return View();
                        }
                        if (image.CheckFileSize(200))
                        {
                            ModelState.AddModelError("File", "The uploaded file should not exceed 200Kb ");
                            return View();
                        }
                        Photo photo = new Photo();
                        photo.Image = await FileManager.CreateAsync(image, _env, "assets", "img", "details"); ;
                        photo.ProductId = product.Id;
                        await _context.AddAsync(photo);
                        _context.SaveChanges();
                    }
                }
            }

            //string[] images = product.DetailImages.Split(',');
            //foreach (string image in images)
            //{
            //    Photo photo = new Photo();
            //    photo.Image = image;
            //    photo.ProductId = product.Id;
            //    await _context.AddAsync(photo);
            //    _context.SaveChanges();

            //}

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
            Product dbproduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            if (dbproduct == null)
            {
                return NotFound();
            }
            return View(dbproduct);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Update(Product product, int? id)
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
            if (id != product.Id)
            {
                return BadRequest();
            }
            Product dbproduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            //Brand brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == product.BrandId);
            //if (!await _context.Brands.AnyAsync(b => !b.IsDeleted && b.Id == product.BrandId))
            //{
            //    ModelState.AddModelError("BrandId", "Select Correct Brand");
            //    return View();
            //}

            if (product.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "You Must Select Correct");
                return View();
            }

            if (!await _context.Categories.AnyAsync(c => !c.IsDeleted &&  c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Select Correct Category");
                return View();
            }
            if (product.MainFormImage != null)
            {
                if (product.MainFormImage.CheckFileType("image/*"))
                {
                    ModelState.AddModelError("File", "The uploaded file should be an image (jpg, png, jpeg)");
                    return View();
                }
                if (product.MainFormImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("File", "The uploaded file should not exceed 200Kb ");
                    return View();
                }
            }

            dbproduct.Name = product.Name.Trim();
            dbproduct.Price = product.Price;
            dbproduct.DiscountedPrice = product.DiscountedPrice;
            dbproduct.Count = product.Count;
            dbproduct.Describtion = product.Describtion;
            //_context.Products.Update(product);


            if (product.MainFormImage != null)
            {
                FileHelper.DeleteFile(_env, dbproduct.MainImage, "assets", "img", "product");
                dbproduct.MainImage = await product.MainFormImage.CreateAsync(_env, "assets", "img", "product");

            };

            if (product.DetailFormImages != null)
            {
                List<Photo> photos = _context.Photos.Where(p => p.ProductId == product.Id).ToList();
                foreach (Photo photo in photos)
                {
                    FileHelper.DeleteFile(_env, photo.Image, "assets", "img", "details");
                    _context.Photos.Remove(photo);
                    _context.SaveChanges();


                }
                foreach (IFormFile image in product.DetailFormImages)
                {
                    Photo photo = new Photo();
                    photo.Image = await FileManager.CreateAsync(image, _env, "assets", "img", "details"); ;
                    photo.ProductId = product.Id;
                    await _context.AddAsync(photo);
                    _context.SaveChanges();

                }
                //FileHelper.DeleteMultipleFiles(_env, dbproduct.DetailImages, "assets", "images", "detailimages");
                //dbproduct.DetailImages = await product.DetailFormImages.CreateMultipleAsync(_env, "assets", "images", "detailimages");
                //List<Photo> photos = _context.Photos.Where(p => p.ProductId == dbproduct.Id).ToList();

            }
            //_context.Entry(dbproduct).CurrentValues.SetValues(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id, int status, int page)
        {
            ViewBag.Status = status;

            IQueryable<Product> query = _context.Products.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Product dbproduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);

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
            dbproduct.IsDeleted = true;
            dbproduct.DeletedAt = DateTime.UtcNow.AddHours(+4);
            _context.SaveChanges();
            return PartialView("_ProductIndexPartial", PageNatedList<Product>.Create(page, query, itemcount));
        }
        public async Task<IActionResult> Restore(int? id, int status, int page)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);
            if (id == null)
            {
                return BadRequest();
            }
            Product dbproduct = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
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

            dbproduct.IsDeleted = false;
            _context.SaveChanges();
            return PartialView("_ProductIndexPartial", PageNatedList<Product>.Create(page, query, itemcount));
        }
        public async Task<IActionResult> GetProductColorSizePartial()
        {
            ViewBag.Colors = await _context.Colors.ToListAsync();
            ViewBag.Sizes = await _context.Sizes.ToListAsync();

            return PartialView("_ProductColorSizePatial");
        }
    }
}
