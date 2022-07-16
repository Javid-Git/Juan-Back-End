using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }
        public Coment Coment { get; set; }
        public List<Coment> Coments { get; set; }
        public List<Product> Products { get; set; }
        public AppUser User { get; set; }
        public List<AppUser> Users { get; set; }
    }
}
