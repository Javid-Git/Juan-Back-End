using Juan.Models;
using Juan.ViewModels.BasketViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Product> Topsellers { get; set; }
        public List<BasketVM> BasketVMs{ get; set; }
    }
}
