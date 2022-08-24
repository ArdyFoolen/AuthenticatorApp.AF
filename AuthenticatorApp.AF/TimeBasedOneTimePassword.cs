using System.Security.Cryptography;
using System.Text;

namespace AuthenticatorApp.AF
{
    public interface ITimeBasedOneTimePassword
    {
        string GetPin(string secret, int digits = 6);
    }

    public class TimeBasedOneTimePassword : ITimeBasedOneTimePassword
    {
        private const int TimeStep = 30;
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private IDateService _dateService;

        public TimeBasedOneTimePassword(IDateService dateService)
        {
            _dateService = dateService;
        }

        public string GetPin(string secret, int digits = 6)
        {
            return GeneratePin(secret, GetCurrentCounter(), digits);
        }

        private long GetCurrentCounter()
            => (long)(_dateService.GetUtcNow() - UNIX_EPOCH).TotalSeconds / TimeStep;

        private string GeneratePin(string secret, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(counter);

            byte[] key = Encoding.ASCII.GetBytes(secret);

            HMACSHA1 hmac = new HMACSHA1(key, true);
            byte[] hash = hmac.ComputeHash(counter);
            int offset = hash[hash.Length - 1] & 0xf;

            // Convert the 4 bytes into an integer, ignoring the sign.
            int binary =
                ((hash[offset] & 0x7f) << 24)
                | (hash[offset + 1] << 16)
                | (hash[offset + 2] << 8)
                | (hash[offset + 3]);

            // Limit the number of digits
            int pin = binary % (int)Math.Pow(10, digits);

            // Pad to required digits
            return pin.ToString(new string('0', digits));
        }
    }
}
