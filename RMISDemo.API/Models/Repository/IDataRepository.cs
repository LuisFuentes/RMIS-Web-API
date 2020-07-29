using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMISDemo.API.Models.Repository
{
    public interface IDataRepository<TEntity, TDto>
    {
        // Create an interface to implement a simple data repository 
        // using the repository pattern

        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetAllForKeyword(string keyword);


        //TEntity Get(long id);
        //TDto GetDto(long id);
        void Add(TEntity entity);
        //void Update(TEntity entityToUpdate, TEntity entity);
        //void Delete(TEntity entity);
    }
}
