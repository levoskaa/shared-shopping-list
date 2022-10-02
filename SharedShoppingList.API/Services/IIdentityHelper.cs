namespace SharedShoppingList.API.Services
{
    public interface IIdentityHelper
    {
        string GetAuthenticatedUserId();
        string GetAuthenticatedUsername();
    }
}