namespace SharedShoppingList.API.Infrastructure.ErrorHandling
{
    public static class ValidationErrors
    {
        public static readonly string UsernameRequired = "username_required";
        public static readonly string PasswordRequired = "password_required";
        public static readonly string SignInCredentialsInvalid = "sign_in_credentials_invalid";
    }
}
