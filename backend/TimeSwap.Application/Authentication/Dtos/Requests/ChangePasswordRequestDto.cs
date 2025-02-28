﻿namespace TimeSwap.Application.Authentication.Dtos.Requests
{
    public class ChangePasswordRequestDto
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
