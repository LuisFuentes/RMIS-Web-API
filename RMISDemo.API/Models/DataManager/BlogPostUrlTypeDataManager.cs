using Microsoft.EntityFrameworkCore;
using RMISDemo.API.Models.Dto;
using RMISDemo.API.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMISDemo.API.Models.DataManager
{
    public class BlogPostUrlTypeDataManager : IDataRepository<BlogPostUrlCategory, BlogPostUrlTypeDto>
    {
        readonly RmisDbContext _dbContext;

        public BlogPostUrlTypeDataManager(RmisDbContext dbc)
        {
            _dbContext = dbc;
        }

        public IEnumerable<BlogPostUrlCategory> GetAll()
        {
            return _dbContext.BlogPostUrlCategory.ToList();
        }

        public IEnumerable<BlogPostUrlCategory> GetAllForKeyword(string keyword)
        {
            return _dbContext.BlogPostUrlCategory
                .Where(a => a.UrlTypeName == keyword)
                .ToList();
        }


        public void Add(BlogPostUrlCategory entity)
        {
            _dbContext.BlogPostUrlCategory.Add(entity);
            _dbContext.SaveChanges();
        }
    }
}
