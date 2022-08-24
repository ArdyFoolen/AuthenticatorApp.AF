using ProtoBuf;

namespace AuthenticatorApp.AF
{
    [Serializable]
    [ProtoContract(SkipConstructor = true)]
    [CacheKey(KeyPrefix = "AuthenticatorAppConfiguration", Version = "1")]
    public class AuthenticatorAppConfiguration
    {
        [ProtoMember(1)]
        public int UserId { get; private set; }

        [ProtoMember(3)]
        public string EncryptedSecret { get; private set; }

        //internal AuthenticatorAppConfiguration(SafeDataReader reader)
        //{
        //    UserId = reader.GetInt32("UserId");
        //    EncryptedSecret = reader.GetString("EncryptedSecret");
        //}

        public AuthenticatorAppConfiguration(int userId, string encryptedSecret)
        {
            UserId = userId;
            EncryptedSecret = encryptedSecret;
        }
    }
}
