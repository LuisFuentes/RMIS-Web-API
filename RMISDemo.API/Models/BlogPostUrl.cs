using System;
using System.Collections.Generic;

namespace RMISDemo.API.Models
{
    public partial class BlogPostUrl
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string UrlPath { get; set; }
        public string UrlTypeId { get; set; }

        public virtual BlogPost BlogPost { get; set; }
        public virtual BlogPostUrlCategory UrlType { get; set; }
    }
}
