using MoveIT.Common.Contracts;
using MoveIT.Common.Contracts.Models;

namespace MoveIT.Common
{
    public class TokenManager : ITokenManager
    {
        private readonly object _lock;
        private JwtToken? _token;

        public TokenManager()
        {
            _lock = new object();
        }

        public JwtToken? Token
        {
            get { return _token; }
            set
            {
                lock (_lock)
                {
                    _token = value;
                }
            }
        }

        public void SetToken(string accessToken, string refreshToken, int expiresIn)
        {
            Token = new JwtToken(accessToken, refreshToken, expiresIn);
        }

        public void DeleteToken()
        {
            Token = null;
        }
    }
}
