using Microsoft.EntityFrameworkCore;
using RMISDemo.API.Models.Dto;
using RMISDemo.API.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMISDemo.API.Models.DataManager
{
    public class BlogPostDataManager : IDataRepository<BlogPost, BlogPostDto>
    {
        readonly RmisDbContext _dbContext;

        public BlogPostDataManager(RmisDbContext dbc)
        {
            _dbContext = dbc;
        }

        public IEnumerable<BlogPost> GetAll()
        {
            _dbContext.ChangeTracker.LazyLoadingEnabled = false;

            return _dbContext.BlogPost
                .Include(a => a.BlogPostUrl)
                .ThenInclude(b => b.UrlType)
                .ToList();

        }

        public IEnumerable<BlogPost> GetAllForKeyword(string keyword)
        {
            _dbContext.ChangeTracker.LazyLoadingEnabled = false;

            return _dbContext.BlogPost
                .Include(a => a.BlogPostUrl)
                .ThenInclude(b => b.UrlType)
                .Where(c=>c.Title.Contains(keyword))
                .ToList();

        }


        public void Add(BlogPost entity)
        {
            _dbContext.BlogPost.Add(entity);
            _dbContext.SaveChanges();
        }





    }
}
