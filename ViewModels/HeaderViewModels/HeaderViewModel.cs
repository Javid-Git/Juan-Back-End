using Juan.ViewModels.BasketViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.HeaderViewModels
{
    public class HeaderViewModel
    {
        public IDictionary<string,string> Settings { get; set; }
        public List<BasketVM> BasketVMs { get; set; }
    }
}
