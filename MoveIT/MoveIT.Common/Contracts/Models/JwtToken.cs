namespace MoveIT.Common.Contracts.Models
{
    public class JwtToken
    {
        public JwtToken(string accessToken, string refreshToken, int expiresIn)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpirationDate = DateTime.UtcNow.AddSeconds(expiresIn);
        }

        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime ExpirationDate { get; private set; }
    }
}
