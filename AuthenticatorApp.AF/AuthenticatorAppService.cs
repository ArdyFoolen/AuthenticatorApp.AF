using Aes.AF;
using Aes.AF.Factories;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticatorApp.AF
{
    public interface IAuthenticatorAppService
    {
        Task<AuthenticatorAppConfiguration> GetAuthenticatorAppConfigurationAsync(int userId);
        string GenerateSecret();
        Task<AuthenticatorAppConfiguration> RegisterAuthenticatorAppAsync(int userId, string url, string secret);
        bool Verify(AuthenticatorAppConfiguration configuration, string token);
    }

    public class AuthenticatorAppService : IAuthenticatorAppService
    {
        private readonly IBase32Encoder _base32Encoder;
        private readonly ITimeBasedOneTimePassword _timeBasedOneTimePassword;
        private readonly IEncryptorFactory _aesFactory;

        public AuthenticatorAppService(
            ITimeBasedOneTimePassword timeBasedOneTimePassword,
            IBase32Encoder base32Encoder,
            IEncryptorFactory aesFactory)
        {
            _base32Encoder = base32Encoder;
            _timeBasedOneTimePassword = timeBasedOneTimePassword;
            _aesFactory = aesFactory;
        }

        public async Task<AuthenticatorAppConfiguration> GetAuthenticatorAppConfigurationAsync(int userId)
        {
            await Task.Yield();
            //ToDo
            return null;
        }

        public string GenerateSecret()
        {
            byte[] buffer = new byte[9];
            using (RandomNumberGenerator rng = RNGCryptoServiceProvider.Create())
            {
                rng.GetBytes(buffer);
            }

            var secret = Convert.ToBase64String(buffer).Substring(0, 10).Replace('/', '0').Replace('+', '1');

            return _base32Encoder.Encode(Encoding.UTF8.GetBytes(secret));
        }

        public async Task<AuthenticatorAppConfiguration> RegisterAuthenticatorAppAsync(int userId, string url, string secret)
        {
            var configuration = CreateConfiguration(userId, secret);
            await SaveAsync(configuration);
            return configuration;
        }

        public bool Verify(AuthenticatorAppConfiguration configuration, string token)
        {
            var current = GeneratePin(configuration);
            return current.Equals(token);
        }

        private AuthenticatorAppConfiguration CreateConfiguration(int userId, string secret)
        {
            var encryptedSecret = _aesFactory.Encrypt(secret);
            return new AuthenticatorAppConfiguration(userId, encryptedSecret);
        }

        private async Task SaveAsync(AuthenticatorAppConfiguration configuration)
        {
            //ToDo
        }

        private string GeneratePin(AuthenticatorAppConfiguration configuration)
        {
            var decryptedSecret = _aesFactory.Decrypt(configuration.EncryptedSecret);
            return _timeBasedOneTimePassword.GetPin(Encoding.UTF8.GetString(_base32Encoder.Decode(decryptedSecret)));
        }
    }
}
