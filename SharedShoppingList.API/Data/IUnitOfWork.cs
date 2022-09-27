namespace SharedShoppingList.API.Data
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}