using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class ComentReply
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:500)]
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Nullable<int> BlogId { get; set; }
        public Blog Blog { get; set; }
        public Nullable<int> ProductId { get; set; }
        public Product Product { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int Rating { get; set; }
        public Nullable<int> ComentId { get; set; }
        public Coment Coment { get; set; }
    }
}
