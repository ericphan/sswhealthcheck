﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="SSW">
//   All rights reserved
// </copyright>
// <summary>
//   Repository interface that follows Repository pattern. Exposes basic CRUD
//   operations for specified entity object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SSW.HealthCheck.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Repository interface that follows Repository pattern. Exposes basic CRUD
    /// operations for specified entity object.
    /// </summary>
    /// <typeparam name="TEntity">Entity to be used for CRUD operation.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Retrieve Methods
        /// <summary>
        /// Gets the specified entities filtered by specified filter expression.
        /// </summary>
        /// <param name="filter">The filter to be used in Where clause.</param>
        /// <returns>Result set.</returns>
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Gets the specified entities filtered by specified filter expression and
        /// loaded with specified dependent objects.
        /// </summary>
        /// <param name="filter">The filter to be used in Where clause.</param>
        /// <param name="includes">The related objects to be loaded.</param>
        /// <returns>Materialized result set.</returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>Result set</returns>
        IQueryable<TEntity> Get();

        /// <summary>
        /// Finds the entity by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Found entity</returns>
        TEntity Find(object id);

        /// <summary>
        /// Loads specified entity with related collection.
        /// </summary>
        /// <typeparam name="TElement">The type of the collection element.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="expression">The collection expression.</param>
        void LoadCollection<TElement>(TEntity entity, Expression<Func<TEntity, ICollection<TElement>>> expression)
            where TElement : class;

        /// <summary>
        /// Loads specified entity with related object.
        /// </summary>
        /// <typeparam name="TProperty">The type of the related object.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="expression">The object expression.</param>
        void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> expression)
            where TProperty : class;

        #endregion

        #region Modification Methods
        
        /// <summary>
        /// Adds the specified entity to database context.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Deletes the specified entity by id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        void Delete(object id);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        void Delete(TEntity entityToDelete);

        // SAVE is not implelented in the repository
        //  because we might want to commit changes to the database from 
        //  multiple repositories, save is called from the EfUnitOfWork
        #endregion
    }
}
