using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using LH.Domain.Interface;

namespace LH.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity FindSingle(Expression<Func<TEntity, bool>> exp = null);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> exp = null);

        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, dynamic>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize);

        int GetCount(Expression<Func<TEntity, bool>> exp = null);

        void Add(TEntity entity);

        /// <summary>
        /// 更新一个实体的所有属性
        /// </summary>
        void Update(TEntity entity);

        void Delete(TEntity entity);

        /// <summary>
        /// 批量删除
        /// </summary>
        void Delete(ICollection<TEntity> entityCollection);
        void Commit();
    }
}