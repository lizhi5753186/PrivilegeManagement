using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LH.Common;
using LH.Domain.Interface;
using LH.Domain.Repositories;
using LH.Repository.DBContext;

namespace LH.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity :class , IEntity
    {
        private readonly ThreadLocal<UserManagerDBContext> _localCtx = new ThreadLocal<UserManagerDBContext>(() => new UserManagerDBContext());

        public UserManagerDBContext DbContext { get { return _localCtx.Value; } }

        public TEntity FindSingle(Expression<Func<TEntity, bool>> exp = null)
        {
            return DbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(exp);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp = null)
        {
            return Filter(exp);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "pageNumber must great than or equal to 1.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "pageSize must great than or equal to 1.");

            var query = DbContext.Set<TEntity>().Where(expression);
            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;
            if (sortPredicate == null)
                throw new InvalidOperationException("Based on the paging query must specify sorting fields and sort order.");

            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    var pagedAscending = query.SortBy(sortPredicate).Skip(skip).Take(take);

                    return pagedAscending;
                case SortOrder.Descending:
                    var pagedDescending = query.SortByDescending(sortPredicate).Skip(skip).Take(take);
                    return pagedDescending;
            }

            throw new InvalidOperationException("Based on the paging query must specify sorting fields and sort order.");
        }

        public int GetCount(Expression<Func<TEntity, bool>> exp = null)
        {
            return Filter(exp).Count();
        }

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Deleted;
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void Delete(ICollection<TEntity> entityCollection)
        {
            if(entityCollection.Count ==0)
                return;

            DbContext.Set<TEntity>().Attach(entityCollection.First());
            DbContext.Set<TEntity>().RemoveRange(entityCollection);
        }

        private IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> exp)
        {
            var dbSet = DbContext.Set<TEntity>().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}
