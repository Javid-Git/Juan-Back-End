using Juan.DAL;
using Juan.Models;
using Juan.ViewModels.BasketViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
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
            return View(await _basketProduct(basketVMs));
        }
        public async Task<IActionResult> AddToBasket(int? id, int? count)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
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

            if (basketVMs.Exists(b => b.ProdId == id))
            {
                basketVMs.Find(b => b.ProdId == id).SelectCount++;
            }
            else
            {
                if (count != null)
                {
                    BasketVM basketVM = new BasketVM
                    {
                        ProdId = product.Id,
                        SelectCount = (int)count
                    };
                    basketVMs.Add(basketVM);

                }
                else
                {
                    BasketVM basketVM = new BasketVM
                    {
                        ProdId = product.Id,
                        SelectCount = 1
                    };
                    basketVMs.Add(basketVM);

                }


            }
            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                    if (dbBasketproduct != null)
                    {
                        dbBasketproduct.Counts += 1;
                    }
                    else
                    {
                        Basket newBasket = new Basket
                        {
                            ProductId = (int)id,
                            UserId = appUser.Id,
                            Counts = 1
                        };

                        appUser.Baskets.Add(newBasket);
                    }
                }
                else
                {
                    List<Basket> baskets = new List<Basket>
                    {
                        new Basket{ProductId = (int)id, Counts = 1}
                    };
                    appUser.Baskets = baskets;
                }
                await _context.SaveChangesAsync();
            }
            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);


            return Json(basketVMs.Count);
            //return PartialView("_AddToCartPartial", await _basketProduct(basketVMs));
        }
        public async Task<IActionResult> OpenBasket()
        {
           
            
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket); ;



            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);


            //return Json(basketVMs.Count);
            return PartialView("_AddToCartPartial", await _basketProduct(basketVMs));
        }
        public async Task<IActionResult> UpdateCount(int? id, int count)
        {
            if (id == null) return BadRequest();

            if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];

            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

                BasketVM basketVM = basketVMs.FirstOrDefault(b => b.ProdId == id);

                if (basketVM == null) return NotFound();
                if (User.Identity.IsAuthenticated)
                {
                    AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name && !u.IsAdmin);

                    if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                    {
                        Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                        if (dbBasketproduct != null)
                        {
                            dbBasketproduct.Counts = count <= 0 ? 1 : count;
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }

                basketVM.SelectCount = count <= 0 ? 1 : count;

                basket = JsonConvert.SerializeObject(basketVMs);

                HttpContext.Response.Cookies.Append("basket", basket);
            }
            else
            {
                return BadRequest();
            }

            return PartialView("_BasketIndexPartial", await _basketProduct(basketVMs));
        }
        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();

            BasketVM basketVM = basketVMs.Find(b => b.ProdId == id);
            if (basket == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                    if (dbBasketproduct != null)
                    {
                        appUser.Baskets.Remove(dbBasketproduct);
                        _context.Baskets.Remove(dbBasketproduct);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();

                    }
                }
                else
                {
                    return NotFound();
                }
            }
            basketVMs.Remove(basketVM);
            await _context.SaveChangesAsync();
            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_AddToCartPartial", await _basketProduct(basketVMs));

        }
        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = await _context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);

            if (string.IsNullOrWhiteSpace(basket)) return BadRequest();

            BasketVM basketVM = basketVMs.Find(b => b.ProdId == id);
            if (basket == null) return NotFound();
            if (User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.Users.Include(u => u.Baskets).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (appUser.Baskets != null && appUser.Baskets.Count() > 0)
                {
                    Basket dbBasketproduct = appUser.Baskets.FirstOrDefault(p => p.ProductId == id);
                    if (dbBasketproduct != null)
                    {
                        appUser.Baskets.Remove(dbBasketproduct);
                        _context.Baskets.Remove(dbBasketproduct);
                        await _context.SaveChangesAsync();
                        //_context.Baskets.Remove(dbBasketproduct);
                    }
                    else
                    {
                        return NotFound();

                    }
                }
                else
                {
                    return NotFound();
                }
            }
            basketVMs.Remove(basketVM);

            basket = JsonConvert.SerializeObject(basketVMs);
            HttpContext.Response.Cookies.Append("basket", basket);

            return PartialView("_BasketIndexPartial", await _basketProduct(basketVMs));
        }
        private async Task<List<BasketVM>> _basketProduct(List<BasketVM> basketVMs)
        {

            foreach (BasketVM item in basketVMs)
            {
                Product dbproduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProdId);

                item.Name = dbproduct.Name;
                item.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                item.Image = dbproduct.MainImage;
            };
            return basketVMs;
        }
        public async Task<IActionResult> DeleteUpdate()
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }

            return Json(basketVMs.Count);

        }
    }
}
