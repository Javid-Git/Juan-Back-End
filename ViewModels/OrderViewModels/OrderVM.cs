using Juan.Models;
using Juan.ViewModels.BasketViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.OrderViewModels
{
    public class OrderVM
    {
        public Order Order { get; set; }

        public List<Basket> Baskets { get; set; }
    }
}
