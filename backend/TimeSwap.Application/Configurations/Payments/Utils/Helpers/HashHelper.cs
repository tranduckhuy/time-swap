using System.Text;

namespace TimeSwap.Application.Configurations.Payments.Utils.Helpers
{
    public static class HashHelper
    {
        public static string HmacSHA512(string data, string key)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            using (var hmacSha512 = new System.Security.Cryptography.HMACSHA512(keyBytes))
            {
                byte[] hashBytes = hmacSha512.ComputeHash(dataBytes);
                foreach (var theByte in hashBytes)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }
}
