namespace SharedShoppingList.API.Infrastructure.Exceptions
{
    public class EntityNotFoundException : SharedShoppingListException
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, params string[] errorCodes)
            : base(message, errorCodes)
        {
        }
    }
}
