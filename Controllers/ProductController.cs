using Juan.DAL;
using Juan.Models;
using Juan.ViewModels.ShopViewModels;
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
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.Include(p => p.ProductSizes).Include(p => p.ProductColors).ToListAsync();
            List<Size> sizes = await _context.Sizes.ToListAsync();
            List<Color> colors = await _context.Colors.ToListAsync();
            List<ProductColor> productColors = await _context.ProductColors.ToListAsync();
            List<ProductSize> productSizes = await _context.ProductSizes.ToListAsync();
            List<Category> categories = await _context.Categories.ToListAsync();
            ShopVM shopVM = new ShopVM
            {
                Products = products,
                Sizes = sizes,
                Colors = colors,
                ProductSizes = productSizes,
                ProductColors = productColors,
                Categories = categories
            };

            return View(shopVM);
        }
        public async Task<IActionResult> ModalView(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products.Include(p=>p.Photos).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView("_ModalViewPartial", product);
        }
        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _context.Products.Where(p => p.Name.ToLower().Contains(search.ToLower())).ToListAsync();
            return PartialView("_SearchPartial", products);

        }
        public async Task<IActionResult> SortByColor(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            List<ProductColor> productColors = await _context.ProductColors.Where(p => p.ColorId == id).ToListAsync();
            List<Product> products = new List<Product>();
            if (productColors == null)
            {
                return NotFound();
            }
            foreach (ProductColor item in productColors)
            {
                Product product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                products.Add(product);
            }
            if (products == null)
            {
                return NotFound();
            }
            return PartialView("_ShopIndexPartial", products);
        }
        public async Task<IActionResult> SortBySize(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            List<ProductSize> productSizes= await _context.ProductSizes.Where(p => p.SizeId == id).ToListAsync();
            List<Product> products = new List<Product>();
            if (productSizes == null)
            {
                return NotFound();
            }
            foreach (ProductSize item in productSizes)
            {
                Product product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                products.Add(product);
            }
            if (products == null)
            {
                return NotFound();
            }
            return PartialView("_ShopIndexPartial", products);
        }
        public async Task<IActionResult> SortByCategory(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            List<Product> products = await _context.Products.Where(p=>p.CategoryId == id).ToListAsync();
            if (products == null)
            {
                return NotFound();
            }
            
            return PartialView("_ShopIndexPartial", products);
        }

    }
}
