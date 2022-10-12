using Microsoft.EntityFrameworkCore.Storage;

namespace SharedShoppingList.API.Data
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}