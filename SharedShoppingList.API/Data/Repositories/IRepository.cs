using System.Linq.Expressions;

namespace SharedShoppingList.API.Data.Repositories
{
    public interface IRepository<TEntity>
    {
        void Insert(TEntity entity);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        Task<TEntity> GetByIdAsync(object id);

        void Update(TEntity entityToUpdate);

        void Delete(object id);

        void Delete(TEntity entityToDelete);
    }
}