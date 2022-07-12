using Juan.DAL;
using Juan.Models;
using Juan.ViewModels;
using Juan.ViewModels.BasketViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }

            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            HomeVM homeVM = new HomeVM
            {
                Products = products,
                Topsellers = products.Where(p => p.IsTopSeller).ToList(),
                BasketVMs = basketVMs
            };
            return View(homeVM);
        }
        public async Task<IActionResult> ModalView(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return PartialView("_ModalViewPartial", product);
        }
    }
}
