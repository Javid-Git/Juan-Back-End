﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsUpdated { get; set; }
        public Nullable<DateTime> CreatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public IEnumerable<BlogBlCategory> BlogCategories{ get; set; }
        public IEnumerable<BlogTag> BlogTags{ get; set; }
        public List<Coment> Coments { get; set; }

    }
}
