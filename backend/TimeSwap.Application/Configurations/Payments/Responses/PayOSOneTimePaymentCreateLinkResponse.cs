namespace TimeSwap.Application.Configurations.Payments.Responses
{
    public class PayOSOneTimePaymentCreateLinkResponse
    {
        public string status { get; set; } = string.Empty;
        public string code { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string orderCode { get; set; } = string.Empty;
        public bool cancel { get; set; }
    }
}
