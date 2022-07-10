using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.ShopViewModels
{
    public class ShopVM
    {
        public List<Product> Products { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Color> Colors { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductSize> ProductSizes{ get; set; }
        public List<Category> Categories { get; set; }
    }
}
