﻿namespace TimeSwap.Application.Authentication.Dtos.Requests
{
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string ClientUrl { get; set; } = string.Empty;
    }
}
