namespace DisconnectedGenericRepository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public enum OrderByType
    {
        Ascending,
        Descending
    }

    public class GenericRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public GenericRepository(DbContext _context)
        {
            this.context = _context;
            this.dbSet = this.context.Set<TEntity>();
        }

        public IEnumerable<TEntity> All()
        {
            return this.dbSet.AsNoTracking().ToList();
        }

        public IEnumerable<TEntity> AllInclude
                                   (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return this.GetAllIncluding(includeProperties).ToList();
        }

        public IQueryable<TEntity> AllInclude(
                                Expression<Func<TEntity, object>> keySelector,
                                OrderByType orderByType,
                                params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable =
            (orderByType == OrderByType.Ascending)
                ? this.GetAllIncluding(includeProperties).OrderBy(keySelector)
                : this.GetAllIncluding(includeProperties).OrderByDescending(keySelector);
            return queryable;
        }

        public IEnumerable<TEntity> FindByInclude(
                                    Expression<Func<TEntity, bool>> predicate,
                                    params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.GetAllIncluding(includeProperties);
            IEnumerable<TEntity> results = query.Where(predicate).ToList();
            return results;
        }

        public IEnumerable<TEntity> FindByInclude(
                                    Expression<Func<TEntity, object>> keySelector,
                                    OrderByType orderByType,
                                    Expression<Func<TEntity, bool>> predicate,
                                    params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = this.AllInclude(keySelector, orderByType, includeProperties);

            IEnumerable<TEntity> results = queryable.Where(predicate).ToList();
            return results;
        }       

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> results = this.dbSet.AsNoTracking()
              .Where(predicate).ToList();
            return results;
        }

        // public TEntity FindByKey(int id)
        // {
        //   Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
        //   return _dbSet.AsNoTracking().SingleOrDefault(lambda);
        // }

        // public TEntity FindByKey(int id)
        // {
        //    return _dbSet.AsNoTracking().SingleOrDefault(e => e.Id == id);
        // }

        public TEntity FindByKey(int id)
        {
            return this.dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            this.dbSet.Add(entity);
            this.context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            this.dbSet.Attach(entity);
            this.context.Entry(entity).State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = this.FindByKey(id);
            this.dbSet.Remove(entity);
            this.context.SaveChanges();
        }

        private IQueryable<TEntity> GetAllIncluding
                                  (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = this.dbSet.AsNoTracking();

            return includeProperties.Aggregate
              (queryable, (current, includeProperty) => current.Include(includeProperty));
        }
    }    
}