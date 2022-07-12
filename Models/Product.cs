using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public string Describtion { get; set; }
        public bool IsTopSeller { get; set; }
        public bool IsAvailable { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }

        [NotMapped]
        public IEnumerable<IFormFile> DetailFormImages { get; set; }
        [NotMapped]
        public IFormFile MainFormImage { get; set; }
    }
}
