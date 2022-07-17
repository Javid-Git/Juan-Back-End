using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.AccountViewModels
{
    public class MemberVM
    {
        public ProfileVM ProfileVM { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
