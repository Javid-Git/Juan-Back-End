using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Juan.DAL;
using Juan.Enums;
using Juan.Models;
using Juan.ViewModels.BasketViewModel;
using Juan.ViewModels.OrderViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    [Authorize(Roles ="User")]
    public class OrderController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public OrderController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            List<Basket> baskets = await _context.Baskets.Include(b=>b.Product).Where(b => b.UserId == appUser.Id).ToListAsync();

            Order order = new Order 
            {
                FullName = appUser.FullName,
                Email = appUser.Email
            };

            OrderVM orderVM = new OrderVM
            {
                Order = order,
                Baskets = baskets
            };

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(Order order)
        {
            AppUser appUser = await _userManager.Users.Include(u=>u.Baskets).ThenInclude(b=>b.Product).FirstOrDefaultAsync(u=>u.UserName == User.Identity.Name);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (Basket basket in appUser.Baskets)
            {
                OrderItem orderItem = new OrderItem
                {
                    Price = basket.Product.DiscountedPrice > 0 ? basket.Product.DiscountedPrice : basket.Product.Price,
                    Count = basket.Counts,
                    ProductId = basket.ProductId,
                    TotalPrice = basket.Product.DiscountedPrice > 0 ? basket.Product.DiscountedPrice * basket.Counts : basket.Product.Price * basket.Counts
                };

                orderItems.Add(orderItem);
            }
            order.OrderItems = orderItems;
            order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.AppUserId = appUser.Id;
            order.OrderStatus = OrderStatus.Pending;
            order.TotalPrice = orderItems.Sum(o => o.TotalPrice);

            await _context.Orders.AddAsync(order);

            if (User.Identity.IsAuthenticated)
            {
                //string basketcookie = HttpContext.Request.Cookies["basket"];
                //List<string> basketVMS = JsonConvert.DeserializeObject<List<string>>(basketcookie);

                //basketVMS.Clear();
                //basketcookie = JsonConvert.SerializeObject(basketVMS);
                //HttpContext.Response.Cookies.Append("basket", basketcookie);

                //string basket = HttpContext.Request.Cookies["basket"];
                //List<BasketVM> basketVMs = null;
                //if (!string.IsNullOrWhiteSpace(basket))
                //{
                //    basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                //}
                //else
                //{
                //    basketVMs = new List<BasketVM>();
                //}

                List<Basket> baskets = await _context.Baskets.Where(b => b.UserId == appUser.Id).ToListAsync();
                foreach (Basket dbbasket in baskets)
                {
                    _context.Baskets.Remove(dbbasket);
                    _context.SaveChanges();
                }

            }
           
            await _context.SaveChangesAsync();

            return RedirectToAction("index", "home");
        }
    }
}
