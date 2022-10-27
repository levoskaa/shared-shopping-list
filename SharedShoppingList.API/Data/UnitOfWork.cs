using Microsoft.EntityFrameworkCore.Storage;

namespace SharedShoppingList.API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SharedShoppingListContext dbContext;

        public UnitOfWork(SharedShoppingListContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}