using Aes.AF.Factories;
using AuthenticatorApp.AF;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticatorAppTests
{
    public class AuthenticatorAppServiceTests
    {
        private IAuthenticatorAppService _authenticatorAppService;
        private Mock<IDateService> _dateServiceMock;
        private IEncryptorFactory _encryptor;

        [SetUp]
        public void Setup()
        {
            _dateServiceMock = new Mock<IDateService>();
            ITimeBasedOneTimePassword timeBasedOneTimePassword = new TimeBasedOneTimePassword(_dateServiceMock.Object);
            IBase32Encoder base32Encoder = new Base32Encoder();
            IAuthenticatorAppSettings settings = AuthenticatorAppSettings.Get();
            IAesFactory aesFactory = new AesFactory();
            _encryptor = aesFactory.CreateFactory(settings.AesSettings);
            _authenticatorAppService = new AuthenticatorAppService(timeBasedOneTimePassword, base32Encoder, _encryptor);
        }

        static object[] PinCases =
        {
            new object[] { "mzqw2zdnovgwu4bq", new DateTime(2022, 07, 20, 13, 28, 10), "111010" },
            new object[] { "mzqw2zdnovgwu4bq", new DateTime(2022, 07, 20, 13, 32, 31), "812153" }
        };

        [TestCaseSource(nameof(PinCases))]
        public async Task Verify_PinIsCorrect(string secret, DateTime utc, string pin)
        {
            // Arrange
            await Task.Yield();
            _dateServiceMock
                .Setup(x => x.GetUtcNow())
                .Returns(utc);
            var encryptedSecret = _encryptor.Encrypt(secret);
            var configuration = new AuthenticatorAppConfiguration(0, encryptedSecret);

            // Act
            var result = _authenticatorAppService.Verify(configuration, pin);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
