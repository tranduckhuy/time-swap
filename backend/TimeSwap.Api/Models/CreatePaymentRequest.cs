﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TimeSwap.Api.Models
{
    public class CreatePaymentRequest
    {
        [Required(ErrorMessage = "PaymentContent is required.")]
        public string PaymentContent { get; set; } = string.Empty;

        [JsonRequired]
        public decimal Amount { get; set; }

        [JsonRequired]
        public int PaymentMethodId { get; set; }

    }
}
