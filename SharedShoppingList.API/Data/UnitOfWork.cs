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
    }
}