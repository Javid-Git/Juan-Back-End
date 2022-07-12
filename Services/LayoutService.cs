using Juan.DAL;
using Juan.Interfaces;
using Juan.Models;
using Juan.ViewModels.BasketViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        public LayoutService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;

        }

        public async Task<List<BasketVM>> GetBasket()
        {
            string basket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            List<BasketVM> basketVMs = null;

            if (!string.IsNullOrWhiteSpace(basket))
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            else
            {
                basketVMs = new List<BasketVM>();
            }


            foreach (BasketVM basketVM in basketVMs)
            {
                Product dbproduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == basketVM.ProdId);

                basketVM.Name = dbproduct.Name;
                basketVM.Price = dbproduct.DiscountedPrice > 0 ? dbproduct.DiscountedPrice : dbproduct.Price;
                basketVM.Image = dbproduct.MainImage;

            };


            return basketVMs;
        }
    }
}
