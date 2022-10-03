using System.Linq.Expressions;

namespace SharedShoppingList.API.Data.Repositories
{
    public interface IRepository<TEntity>
    {
        void Insert(TEntity entity);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetByID(object id);

        void Update(TEntity entityToUpdate);

        void Delete(object id);

        void Delete(TEntity entityToDelete);
    }
}