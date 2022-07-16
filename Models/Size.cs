using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Size
    {
        public int Id { get; set; }
        [StringLength(255), Required]
        public int Number { get; set; }
        public IEnumerable<ProductColorSize> ProductColorSizes { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
    }
}
