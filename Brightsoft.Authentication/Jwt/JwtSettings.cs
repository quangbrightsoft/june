namespace Brightsoft.Authentication.Jwt
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public ulong ExpireInSeconds { get; set; }
    }
}
