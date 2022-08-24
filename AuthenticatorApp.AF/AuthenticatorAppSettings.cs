//I generated the IV and Key using Visual Studio C# interactive window:

//using System.Security.Cryptography;
//AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
//var IV = Convert.ToBase64String(AES.IV);
//var Key = Convert.ToBase64String(AES.Key);
//IV
//Copy IV
//Key
//Copy Key

using Aes.AF;
using Aes.AF.Extensions;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.Security.Cryptography;

namespace AuthenticatorApp.AF
{
    public interface IAuthenticatorAppSettings
    {
        AesSettings AesSettings { get; }
        int QRPixelsPerModule { get; }
    }

    public class AuthenticatorAppSettings : IAuthenticatorAppSettings
    {
        public const string EnvPath = "AuthenticatorAppSettingsEnvPath";

        public AesSettings AesSettings { get; private set; }

        public int QRPixelsPerModule { get; private set; } = 8;

        public static IAuthenticatorAppSettings Get()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(GetEnvPath, optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();

            var section = configuration.GetSection("AuthenticatorAppSettings");
            int qrPixelsPerModule = 8;
            if (int.TryParse(section["QRPixelsPerModule"], out qrPixelsPerModule)) { }


            return new AuthenticatorAppSettings()
            {
                AesSettings = GetSettings(section),
                QRPixelsPerModule = qrPixelsPerModule
            };
        }

        private static AesSettings GetSettings(IConfigurationSection section)
        {
            var aesSection = section.GetSection("AesSettings");
            return AesSettings.Create(aesSection);
        }

        private static string GetEnvPath { get => Environment.GetEnvironmentVariable(EnvPath) ?? "Configs\\AuthenticatorAppSettings.json"; }
    }
}
