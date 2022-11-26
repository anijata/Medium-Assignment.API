using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Medium_Assignment.API.Models;

namespace Medium_Assignment.API.Repo
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public virtual ApplicationDbContext DbContext { get; set; }
        public GenericRepository(ApplicationDbContext _dbContext)
        {
            DbContext = _dbContext;
        }
        public virtual void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public virtual TEntity Get(int id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        public virtual IEnumerable<TEntity> List()
        {
            return DbContext.Set<TEntity>().ToList();
        }
        public virtual  IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate).ToList();
        }
        public virtual void Remove(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
        }
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().AddRange(entities);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
        }
    }
}