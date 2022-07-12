using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class BlogBlCategory
    {
        public int Id { get; set; }
        public int BlCategoryId { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public BlCategory BlCategory{ get; set; }
    }
}
