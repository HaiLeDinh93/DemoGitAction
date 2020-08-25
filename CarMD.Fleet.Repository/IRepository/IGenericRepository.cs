using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CarMD.Fleet.Repository.IRepository
{
    public interface IGenericRepository<TEntity>
    {
        /// <summary>
        /// Get all entity from db
        /// </summary>
        /// <param name="entity"></param>
        IList<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get single entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity Get(Guid id);

        TEntity Get(Guid id, params Expression<Func<TEntity, object>>[] includes);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Add entity to db
        /// </summary>
        /// <param name="entity"></param>
        TEntity Create(TEntity entity);

        /// <summary>
        /// Edit entity in db
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete entity from db
        /// </summary>
        /// <param name="id"></param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Remove entity from db
        /// </summary>
        /// <param name="id"></param>
        void Remove(TEntity entityToDelete);

        /// <summary>
        /// Save Changes
        /// </summary>
        void SaveChanges();
    }
}
