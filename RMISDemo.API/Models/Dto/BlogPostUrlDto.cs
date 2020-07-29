using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMISDemo.API.Models.Dto
{
    public class BlogPostUrlDto
    {
        public BlogPostUrlDto()
        {

        }

        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public string UrlPath { get; set; }
        public string UrlTypeId { get; set; }

        public BlogPostUrlTypeDto UrlType { get; set; }


    }
}
