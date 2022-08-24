using AuthenticatorApp.AF;

namespace AuthenticatorAppTests
{
    public class AuthenticatorAppSettingsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Get_ShouldSucceed()
        {
            // Act
            var settings = AuthenticatorAppSettings.Get();

            // Assert
            Assert.IsNotNull(settings);
            Assert.IsNotNull(settings.AesSettings);
        }
    }
}