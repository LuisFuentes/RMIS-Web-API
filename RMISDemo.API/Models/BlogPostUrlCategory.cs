using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RMISDemo.API.Models
{
    [DataContract]
    public partial class BlogPostUrlCategory
    {
        public BlogPostUrlCategory()
        {
            BlogPostUrl = new HashSet<BlogPostUrl>();
        }
        public string UrlTypeId { get; set; }
        [DataMember]
        public string UrlTypeName { get; set; }

        public virtual ICollection<BlogPostUrl> BlogPostUrl { get; set; }
    }
}
