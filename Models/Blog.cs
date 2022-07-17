using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Blog
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }
        [StringLength(maximumLength: 255)]
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
        [NotMapped]
        public IEnumerable<int> TagIds { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }
        [NotMapped]
        public IFormFile FormImage { get; set; }
    }
}
