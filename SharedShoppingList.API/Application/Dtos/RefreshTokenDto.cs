﻿namespace SharedShoppingList.API.Application.Dtos
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}