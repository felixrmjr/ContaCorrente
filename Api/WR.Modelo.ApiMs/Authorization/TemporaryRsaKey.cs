using System.Security.Cryptography;

namespace WR.Modelo.ApiMs.Authorization
{
    public class TemporaryRsaKey
    {
        public string KeyId { get; set; }
        public RSAParameters Parameters { get; set; }
    }
}
