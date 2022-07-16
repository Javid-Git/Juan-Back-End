using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.ShopViewModels
{
    public class ShopVM
    {
        public PageNatedList<Product> Products { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Product> ProductForCategory { get; set; }
        public List<Color> Colors { get; set; }
        //public List<Noneed2> ProductColors { get; set; }
        //public List<Noneed1> ProductSizes{ get; set; }
        public List<Category> Categories { get; set; }
        public List<ProductColorSize> ProductColorSizes { get; set; }
    }
}
