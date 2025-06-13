using System.Security.Cryptography;
using System.Text;

namespace FormsAPI.Services
{
    public class EncryptionService
    {
        private readonly string _key;

        public EncryptionService(IConfiguration configuration)
        {
            _key = configuration["PasswordKey"]!;
        }

        public byte[] HashPassword(string password)
        {
            var key = Encoding.UTF8.GetBytes(_key);

            using (var hmac = new HMACSHA3_256(key))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
