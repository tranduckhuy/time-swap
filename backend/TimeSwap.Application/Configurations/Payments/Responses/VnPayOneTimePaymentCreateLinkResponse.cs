using System.Net;
using System.Text;
using TimeSwap.Application.Configurations.Payments.Utils.Helpers;

namespace TimeSwap.Application.Configurations.Payments.Responses
{
    public class VnPayOneTimePaymentCreateLinkResponse
    {
        private readonly SortedList<string, string> responseData = new SortedList<string, string>(new VnPayCompare());

        public string vnp_TmnCode { get; set; } = string.Empty;
        public decimal vnp_Amount { get; set; } = 0;
        public string vnp_BankCode { get; set; } = string.Empty;
        public string vnp_BankTranNo { get; set; } = string.Empty;
        public string vnp_CardType { get; set; } = string.Empty;
        public string vnp_PayDate { get; set; } = string.Empty;
        public string vnp_OrderInfo { get; set; } = string.Empty;
        public string vnp_TransactionNo { get; set; } = string.Empty;
        public string vnp_ResponseCode { get; set; } = string.Empty;
        public string vnp_TransactionStatus { get; set; } = string.Empty;
        public string vnp_TxnRef { get; set; } = string.Empty;
        public string vnp_SecureHash { get; set; } = string.Empty;


        public bool isValidSignature(string secretKey)
        {
            MakeResponseData();

            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in responseData)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string checkSum = HashHelper.HmacSHA512(data.ToString().Remove(data.Length - 1, 1), secretKey);
            return checkSum.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                responseData[key] = value;
            }
        }

        private void MakeResponseData()
        {
            AddResponseData("vnp_TmnCode", vnp_TmnCode);
            AddResponseData("vnp_Amount", vnp_Amount.ToString());
            AddResponseData("vnp_BankCode", vnp_BankCode);
            AddResponseData("vnp_CardType", vnp_CardType);
            AddResponseData("vnp_BankTranNo", vnp_BankTranNo);
            AddResponseData("vnp_PayDate", vnp_PayDate);
            AddResponseData("vnp_TransactionNo", vnp_TransactionNo);
            AddResponseData("vnp_ResponseCode", vnp_ResponseCode);
            AddResponseData("vnp_OrderInfo", vnp_OrderInfo);
            AddResponseData("vnp_TransactionStatus", vnp_TransactionStatus);
            AddResponseData("vnp_TxnRef", vnp_TxnRef);

        }

    }
}
