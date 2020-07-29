using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMISDemo.API.Models.Dto
{
    public class BlogPostDto
    {
        public BlogPostDto() 
        {
        
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<BlogPostUrlDto> Urls { get; set; }



    }
}
