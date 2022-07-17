using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels.BlogViewModels
{
    public class BlogVM
    {
        public PageNatedList<Blog> Blogs{ get; set; }
        public Blog Blog{ get; set; }
        public List<Tag> Tags{ get; set; }
        public List<BlogTag> BlogTags { get; set; }
        public List<BlCategory> BlCategories{ get; set; }
        public List<BlogBlCategory> BlogBlCategories{ get; set; }
        public List<Coment> Coments{ get; set; }
        public Coment Coment{ get; set; }
        public List<ComentReply> ComentReplies{ get; set; }
        public AppUser User{ get; set; }
        public List<AppUser> Users{ get; set; }
        public Nullable<int> PostComentId{ get; set; }

    }
}
