using System.Security.Cryptography;
using System.Text;

namespace Clarifi.RoomMappingLogger
{
    public static class KeyGenerator
    {
        public static string GetMd5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                var sBuilder = new StringBuilder();

                foreach (var t in data)
                    sBuilder.Append(t.ToString("X2"));

                return sBuilder.ToString();
            }
        }
    }
}
