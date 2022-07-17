using Juan.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Order
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public string AppUserId { get; set; }
        public double TotalPrice { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string TownCity { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Comment { get; set; }

        public AppUser AppUser { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
