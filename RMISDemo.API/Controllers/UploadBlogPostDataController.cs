using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMISDemo.API.Models;
using RMISDemo.API.Models.Dto;
using RMISDemo.API.Models.Repository;

namespace RMISDemo.API.Controllers
{

    [Route("api/upload/blogs")]
    [ApiController]
    public class UploadBlogPostDataController : ControllerBase
    {
        private readonly IDataRepository<BlogPost, BlogPostDto> _repo;

        public UploadBlogPostDataController(IDataRepository<BlogPost, BlogPostDto> dataRepository)
        {
            _repo = dataRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            // NOTE : ONLY CALL THIS URL ONCE TO WEB SCRAPE THE
            // RMIS BLOG SITE AND STORE CONTENTS INTO DB.

            // Commented out after running one time ;)
            //await WebScrape();
            return Created("api/blogs", "");
        }


        private async Task WebScrape()
        {
            // Small script to fetch the data from RMIS blog web site.

            // Fetch the data from the RMIS Blog web site and parse out the
            // header text, image, content, and created date.

            string baseUrl = "http://blog.registrymonitoring.com/";

            //The 'using' will help to prevent memory leaks.
            //Create a new instance of HttpClient
            string response = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                {
                    using (HttpContent content = res.Content)
                    {
                        response = await content.ReadAsStringAsync();
                    }
                }
            }

            // First find each Article section
            List<ArticleSection> sections = new List<ArticleSection>();

            int articleIndex = response.IndexOf("<article");
            bool isFinished = false;

            while (!isFinished)
            {
                int nextIndex = response.IndexOf("<article", articleIndex + 1);
                if (nextIndex == -1)
                {
                    // No more articles to find
                    isFinished = true;
                    nextIndex = response.Length;
                }

                sections.Add(new ArticleSection() 
                { 
                    Contents = response.Substring(articleIndex, nextIndex - articleIndex)
                });

                articleIndex = nextIndex;

            }

            // Next, find each header
            foreach (ArticleSection section in sections)
            {
                int headerIndex = section.Contents.IndexOf("<header");
                int headerEndIndex = section.Contents.IndexOf("</header>");

                // Within each header tag, there's an <h2> tag and an <a> tag inside of it for the title of the article
                string headerSection = section.Contents.Substring(headerIndex, headerEndIndex - headerIndex);

                int titleIndex = headerSection.IndexOf("<a");
                int titleEndIndex = headerSection.IndexOf("</a>");

                // Format for the <a> section is something like "<a href=\"http://rmisblog.azurewebsites.net/2017/08/29/eld-deadline-approaching/\" rel=\"bookmark\">ELD Deadline Approaching"
                string titleSection = headerSection.Substring(titleIndex, titleEndIndex - titleIndex);

                // Find the closing of the <a href=... > tag
                int textIndex = titleSection.IndexOf('>') + 1;
                string title = titleSection.Substring(textIndex);

                //Console.WriteLine(title);
                section.Title = title;


                // There's usually a link in the <a> href
                int linkUrlIndex = titleSection.IndexOf("\"") + 1;//"href=\\");
                int linkUrlEndIndex = titleSection.IndexOf("\"", linkUrlIndex + 1);

                string linkUrl = titleSection.Substring(linkUrlIndex, linkUrlEndIndex - linkUrlIndex);
                section.PostUrl = linkUrl;

                // Next find the image of the post which is after the <header> tag section
                int divContentIndex = section.Contents.IndexOf("<div", headerEndIndex);
                int divContentEndIndex = section.Contents.IndexOf("</div>");

                string content = section.Contents.Substring(divContentIndex, divContentEndIndex - divContentIndex);

                string createdDate = section.Contents.Substring(divContentIndex, divContentEndIndex - divContentIndex);
                section.PostCreatedDate = DateTime.Now; // Convert.ToDateTime(createdDate);

                int imageIndex = content.IndexOf("src=\"");
                if (imageIndex != -1)
                {
                    // There's an image
                    imageIndex += 5;
                    int imageEndIndex = content.IndexOf("\"", imageIndex);
                    string imageUrl = content.Substring(imageIndex, imageEndIndex - imageIndex);
                    section.ImageUrl = imageUrl;

                }

                // Next find the content of the article's post in the <p> tags
                string contentBody = string.Empty;
                int index = 0;

                isFinished = false;
                while (!isFinished)
                {

                    int lastPIndex = content.IndexOf("<p>", index);
                    int lastPEndIndex = content.IndexOf("</p>", index + 1);

                    if (lastPEndIndex == -1 || lastPIndex == -1)
                        isFinished = true;
                    else
                    {
                        index = lastPEndIndex;

                        if (lastPEndIndex - lastPIndex > 0)
                        {
                            string p = content.Substring(lastPIndex, lastPEndIndex - lastPIndex);

                            if (p.Length > contentBody.Length)
                                contentBody = p;
                        }
                    }

                }
                section.PostContent = contentBody;

                // Finally, upload each post into the database
                BlogPost newPost = new BlogPost();
                newPost.Title = section.Title;
                newPost.PostContent = section.PostContent;
                newPost.CreatedDate = section.PostCreatedDate;

                _repo.Add(newPost);


            }
        }

        protected internal class ArticleSection
        {
            public string Contents { get; set; }

            public string Title { get; set; }

            public string PostUrl { get; set; }

            public string ImageUrl { get; set; }

            public string PostContent { get; set; }

            public DateTime PostCreatedDate { get; set; }

        }
    }

    
}
