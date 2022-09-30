namespace SharedShoppingList.API.Infrastructure.ErrorHandling
{
    public static class ValidationErrors
    {
        public static readonly string UsernameRequired = "username_required";
        public static readonly string UsernameTaken = "username_taken";
        public static readonly string PasswordRequired = "password_required";
        public static readonly string MinPasswordLength = "min_password_length_is_8";
        public static readonly string PasswordDigitRequired = "password_must_contain_at_least_1_digit";
        public static readonly string PasswordLowercaseRequired = "password_must_contain_at_least_1_lowercase_letter";
        public static readonly string PasswordUppercaseRequired = "password_must_contain_at_least_1_uppercase_letter";
        public static readonly string SignInCredentialsInvalid = "sign_in_credentials_invalid";
        public static readonly string UserIdRequired = "user_id_required";
        public static readonly string RefreshTokenRequired = "refresh_token_required";
        public static readonly string RefreshTokenInvalid = "refresh_token_invalid";
        public static readonly string AccessTokenInvalid = "access_token_invalid";
        public static readonly string AccessTokenVerificatonFailed = "access_token_verification_failed";
    }
}