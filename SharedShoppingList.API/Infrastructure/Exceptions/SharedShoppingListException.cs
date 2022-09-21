namespace SharedShoppingList.API.Infrastructure.Exceptions
{
    public abstract class SharedShoppingListException : Exception
    {
        public IEnumerable<string> ErrorCodes { get; set; } = Array.Empty<string>();

        public SharedShoppingListException()
        {        
        }

        public SharedShoppingListException(string message)
            : base(message)
        {
        }

        public SharedShoppingListException(string message, params string[] errorCodes)
            : base(message)
        {
            ErrorCodes = errorCodes;
        }
    }
}
