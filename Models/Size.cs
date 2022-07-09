using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Size
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
    }
}
