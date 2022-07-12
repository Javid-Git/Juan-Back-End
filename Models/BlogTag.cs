using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class BlogTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public Tag Tags { get; set; }
    }
}
