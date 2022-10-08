namespace SharedShoppingList.API.Application.Common
{
    public class AuthenticationResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiryTime { get; set; }
    }
}
