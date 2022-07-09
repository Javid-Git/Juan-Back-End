using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public string MainImage { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double DiscountedPrice { get; set; }
        public bool IsTopSeller { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }

        public int CategoryId { get; set; }
        public Category Categories { get; set; }
    }
}
