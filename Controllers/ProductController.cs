using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Juan.ViewModels.ShopViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            IQueryable<Product> products = _context.Products./*Include(p => p.ProductSizes).*//*Include(p => p.ProductColors).*/AsQueryable();
            List<Product> catproducts = await _context.Products/*.Include(p => p.ProductSizes).Include(p => p.ProductColors)*/.ToListAsync();
            List<Size> sizes = await _context.Sizes.ToListAsync();
            List<Color> colors = await _context.Colors.ToListAsync();
            //List<Noneed2> productColors = await _context.ProductColors.ToListAsync();
            //List<Noneed1> productSizes = await _context.ProductSizes.ToListAsync();
            List<ProductColorSize> productColorSizes = await _context.ProductColorSizes.ToListAsync();
            List<Category> categories = await _context.Categories.ToListAsync();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);

            ShopVM shopVM = new ShopVM
            {
                Products = PageNatedList<Product>.Create(page, products, itemcount),
                ProductForCategory = catproducts,
                Sizes = sizes,
                Colors = colors,
                //ProductSizes = productSizes,
                //ProductColors = productColors,
                Categories = categories,
                ProductColorSizes = productColorSizes
            };
            return View(shopVM);
        }
        //public async Task<IActionResult> ModalView(int? id)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    Product product = await _context.Products.Include(p=>p.Photos).FirstOrDefaultAsync(p => p.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return PartialView("_ModalViewPartial", product);
        //}
        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _context.Products.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToListAsync();
            return PartialView("_SearchPartial", products);

        }
        //public async Task<IActionResult> SortByColor(int? id, int page=1)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    List<Noneed2> productColors = await _context.ProductColors.Where(p => p.ColorId == id).ToListAsync();
        //    List<Product> products = new List<Product>();
        //    int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);

        //    if (productColors == null)
        //    {
        //        return NotFound();
        //    }
        //    foreach (Noneed2 item in productColors)
        //    {
        //        Product product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
        //        products.Add(product);
        //    }
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }
        //    IQueryable<Product> query = products.AsQueryable();
        //    return PartialView("_ShopIndexPartial", PageNatedList<Product>.Create(page, query, itemcount));
        //}
        //public async Task<IActionResult> SortBySize(int? id, int page = 1)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    List<Noneed1> productSizes= await _context.ProductSizes.Where(p => p.SizeId == id).ToListAsync();
        //    List<Product> products = new List<Product>();
        //    int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);

        //    if (productSizes == null)
        //    {
        //        return NotFound();
        //    }
        //    foreach (Noneed1 item in productSizes)
        //    {
        //        Product product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
        //        products.Add(product);
        //    }
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }
        //    IQueryable<Product> query = products.AsQueryable();

        //    return PartialView("_ShopIndexPartial", PageNatedList<Product>.Create(page, query, itemcount));
        //}
        //public async Task<IActionResult> SortByCategory(int? id, int page = 1)
        //{
        //    if (id == null)
        //    {
        //        return BadRequest();
        //    }
        //    List<Product> products = await _context.Products.Where(p=>p.CategoryId == id).ToListAsync();
        //    int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);

        //    if (products == null)
        //    {
        //        return NotFound();
        //    }
        //    IQueryable<Product> query = products.AsQueryable();

        //    return PartialView("_ShopIndexPartial", PageNatedList<Product>.Create(page, query, itemcount));
        //}
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Detail(int? id)
        {
            
            List<Product> products = await _context.Products.ToListAsync();
            AppUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            Product product = new Product();
            if (id != null)
            {
                product = await _context.Products.Include(p=>p.Photos).FirstOrDefaultAsync(b => b.Id == id);

            }
            else
            {
                product = await _context.Products.Include(p => p.Photos).FirstOrDefaultAsync(b => b.Id == Convert.ToInt32(TempData["ProductId"]));
            }
            Coment coment = new Coment();
            if (id != null)
            {
                coment = await _context.Coments.FirstOrDefaultAsync(c => c.ProductId == product.Id);

            }
            else
            {
                coment = await _context.Coments.FirstOrDefaultAsync(c => c.ProductId == Convert.ToInt32(TempData["ProductId"]));
            }
            List<Coment> coments = new List<Coment>();
            if (id != null)
            {
                coments = await _context.Coments.Where(c => c.ProductId == id).ToListAsync();

            }
            else
            {
                coments = await _context.Coments.Where(c => c.ProductId == Convert.ToInt32(TempData["ProductId"])).ToListAsync();
            }
            if (product == null)
            {
                return NotFound();
            }
            List<AppUser> users = await _context.Users.ToListAsync();
            ProductDetailVM productDetailVM = new ProductDetailVM
            {
                Product = product,
                Products = products,
                Coment = coment,
                User = user,
                Coments = coments,
                Users = users
            };
            return View(productDetailVM);
        }
        public async Task<IActionResult> SortingBy(int? id, int? sizeId, int? colorId, int page = 1)
        {
            List<Product> products = new List<Product>();
            int itemcount = int.Parse(_context.Settings.FirstOrDefaultAsync(i => i.Key == "PageItemCount").Result.Value);

            if (id != null)
            {
                products = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
                ShopVM shop = new ShopVM
                {
                    ProductForCategory = products,
                    Sizes = await _context.Sizes.ToListAsync(),
                };
                ViewBag.CategoryBag = products;

            }
            if (sizeId != null)
            {
                List<ProductColorSize> productColorSizes = await _context.ProductColorSizes.Where(p => p.SizeId == sizeId).ToListAsync();
                products = new List<Product>();
                foreach (ProductColorSize item in productColorSizes)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    products.Add(product);
                }
                ViewBag.SizeBag = products;

            }
            if (colorId != null)
            {
                List<ProductColorSize> productColorSizes = await _context.ProductColorSizes.Where(p => p.ColorId == colorId).ToListAsync();
                products = new List<Product>();
                foreach (ProductColorSize item in productColorSizes)
                {
                    Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    products.Add(product);
                }
                ViewBag.ColorBag = products;
            }

            IQueryable<Product> query = products.AsQueryable();
            return PartialView("_ShopIndexPartial", PageNatedList<Product>.Create(page, query, itemcount));


        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> PostComent(int? productId, string? userId, IFormCollection collection, Coment coment)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Detail");
                    TempData["ProductId"] = productId;


                }
                if (productId == null)
                {
                    return NotFound();
                }
                if (userId == null)
                {
                    return RedirectToAction("login", "account", new { area = "" });

                }
                Coment dbreview = new Coment
                {
                    UserId = userId,
                    ProductId = productId,
                    Rating = Convert.ToInt32(collection["rating"]),
                    Date = DateTime.UtcNow.AddHours(+4),
                    Text = coment.Text
                };

                TempData["ProductId"] = productId;
                Coment postedreview = await _context.Coments.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);
                if (postedreview != null)
                {
                    _context.Coments.Remove(postedreview);
                }
                await _context.Coments.AddAsync(dbreview);
                await _context.SaveChangesAsync();
                return RedirectToAction("Detail");
            }
            else
            {
                return RedirectToAction("login", "account", new { area = "" });

            }

        }
    }
}
