﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SharedShoppingList.API.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        internal SharedShoppingListContext context;
        internal DbSet<TEntity> dbSet;

        public Repository(SharedShoppingListContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            CancellationToken cancellationToken = default,
            params string[] includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }
            else
            {
                return await query.ToListAsync(cancellationToken);
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(
            object id,
            CancellationToken cancellationToken = default,
            params string[] includeProperties)
        {
            var entity = await dbSet.FindAsync(id);
            if (entity != null)
            {
                await LoadRelatedEntitiesAsync(entity, cancellationToken, includeProperties);
            }
            return entity;
        }

        public virtual async Task LoadRelatedEntitiesAsync(
            TEntity entity,
            CancellationToken cancellationToken = default,
            params string[] includeProperties)
        {
            foreach (var includeProperty in includeProperties)
            {
                await context.Entry(entity)
                    .Navigation(includeProperty)
                    .LoadAsync(cancellationToken);
            }
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
    }
}