using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
    }
}
