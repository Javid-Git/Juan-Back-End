using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
