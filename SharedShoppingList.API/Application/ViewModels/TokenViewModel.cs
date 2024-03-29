﻿namespace SharedShoppingList.API.Application.ViewModels
{
    public class TokenViewModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiryTime { get; set; }
    }
}
