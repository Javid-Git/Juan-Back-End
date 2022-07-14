using Juan.Models;
using Juan.ViewModels.BasketViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.ModalViewModels
{
    public class ModalVM
    {
        public BasketVM BasketVM { get; set; }
        public Product Product { get; set; }
    }
}
