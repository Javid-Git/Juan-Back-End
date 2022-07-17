using Juan.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(/*IDictionary<string, string> settings*/)
        {
            IDictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(x => x.Key, s => s.Value);

            return View(await Task.FromResult(settings));
        }
    }
}
