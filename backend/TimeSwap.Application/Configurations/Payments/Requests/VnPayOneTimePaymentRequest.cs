using System.Net;
using TimeSwap.Application.Configurations.Payments.Utils.Helpers;

namespace TimeSwap.Application.Configurations.Payments.Requests
{
    public class VnPayOneTimePaymentRequest
    {
        private readonly SortedList<string, string> requestData = new SortedList<string, string>(new VnPayCompare());

        public VnPayOneTimePaymentRequest() { }

        public string vnp_Version { get; set; } = string.Empty;
        public string vnp_Command { get; set; } = "pay";
        public string vnp_TmnCode { get; set; } = string.Empty;
        public decimal vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; } = string.Empty;
        public string vnp_CreateDate { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss");
        public string vnp_CurrCode { get; set; } = "VND";
        public string vnp_Locale { get; set; } = "vn";
        public string vnp_OrderInfo { get; set; } = string.Empty;
        public string vnp_OrderType { get; set; } = string.Empty;
        public string vnp_SecureHash { get; set; } = string.Empty;
        public string vnp_ReturnUrl { get; set; } = string.Empty;
        public string vnp_ExpireDate { get; set; } = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss");
        public string vnp_TxnRef { get; set; } = string.Empty;
        public string vnp_IpAddr { get; set; } = string.Empty;

        private void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                requestData[key] = value;
            }
        }

        private void MakeRequestData()
        {
            AddRequestData("vnp_Version", vnp_Version);
            AddRequestData("vnp_Command", vnp_Command);
            AddRequestData("vnp_TmnCode", vnp_TmnCode);
            AddRequestData("vnp_Amount", vnp_Amount.ToString());
            AddRequestData("vnp_BankCode", vnp_BankCode);
            AddRequestData("vnp_CreateDate", vnp_CreateDate);
            AddRequestData("vnp_CurrCode", vnp_CurrCode);
            AddRequestData("vnp_Locale", vnp_Locale);
            AddRequestData("vnp_OrderInfo", vnp_OrderInfo);
            AddRequestData("vnp_OrderType", vnp_OrderType);
            AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            AddRequestData("vnp_ExpireDate", vnp_ExpireDate);
            AddRequestData("vnp_TxnRef", vnp_TxnRef);
            AddRequestData("vnp_IpAddr", vnp_IpAddr);
        }

        public string GetLink(string baseUrl, string secretKey)
        {
            MakeRequestData();

            var queryParams = string.Join("&", requestData
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));

            var secureHash = HashHelper.HmacSHA512(queryParams, secretKey);
            return $"{baseUrl}?{queryParams}&vnp_SecureHash={secureHash}";
        }
    }
}
