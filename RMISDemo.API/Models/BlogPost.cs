using System;
using System.Collections.Generic;

namespace RMISDemo.API.Models
{
    public partial class BlogPost
    {
        public BlogPost()
        {
            BlogPostUrl = new HashSet<BlogPostUrl>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<BlogPostUrl> BlogPostUrl { get; set; }
    }
}
