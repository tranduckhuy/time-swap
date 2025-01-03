﻿namespace TimeSwap.Application.Authentication.Dtos.Responses
{
    public record AuthenticationResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
}
