// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRepository.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Base repository implementation. Should be used as a base class for most of the repositories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using SSW.Framework.Data.EF;
    using SSW.HealthCheck.Data.Interfaces;

    /// <summary>
    /// Base repository implementation. Should be used as a base class for most of the repositories.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class. 
        /// </summary>
        /// <param name="contextManager">
        /// The context manager.
        /// </param>
        public BaseRepository(IDbContextManager contextManager)
        {
            if (contextManager == null)
            {
                throw new ArgumentNullException("contextManager");
            }

            this.ContextManager = contextManager;
        }

        /// <summary>
        /// Gets the context manager.
        /// </summary>
        /// <value>The context manager.</value>
        protected IDbContextManager ContextManager { get; private set; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        protected DbContext Context
        {
            get { return this.ContextManager.Context; }
        }

        /// <summary>
        /// Gets the database entity set.
        /// </summary>
        /// <value>The database set.</value>
        private IDbSet<TEntity> DbSet
        {
            get { return this.Context.Set<TEntity>(); } 
        }

        /// <summary>
        /// Gets the specified entities filtered by specified filter expression.
        /// </summary>
        /// <param name="filter">The filter to be used in Where clause.</param>
        /// <returns>Result set.</returns>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return this.Get().Where(filter);
        }

        /// <summary>
        /// Gets the specified entities filtered by specified filter expression and
        /// loaded with specified dependent objects.
        /// </summary>
        /// <param name="filter">The filter to be used in Where clause.</param>
        /// <param name="includes">The related objects to be loaded.</param>
        /// <returns>Materialized result set.</returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
        {
            var get = this.Get();
            foreach (var include in includes)
            {
                get.Include(include);
            }

            return get.Where(filter).ToList();
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>Result set</returns>
        public virtual IQueryable<TEntity> Get()
        {
            return this.DbSet;
        }

        /// <summary>
        /// Finds the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Found entity</returns>
        public virtual TEntity Find(object id)
        {
            return this.DbSet.Find(id);
        }

        /// <summary>
        /// Loads specified entity with related collection.
        /// </summary>
        /// <typeparam name="TElement">The type of the collection element.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="expression">The collection expression.</param>
        public virtual void LoadCollection<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> expression) where TElement : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads specified entity with related object.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related object.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="expression">The object expression.</param>
        public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression) where TProperty : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the specified entity to database context.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        public virtual void Add(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        public virtual void Update(TEntity entityToUpdate)
        {
            this.DbSet.Attach(entityToUpdate);
            this.Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the specified entity by id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = this.Find(id);
            this.Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.DbSet.Attach(entityToDelete);
            }

            this.DbSet.Remove(entityToDelete);
        }
    }
}
