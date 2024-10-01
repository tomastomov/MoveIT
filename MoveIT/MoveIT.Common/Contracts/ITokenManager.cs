using MoveIT.Common.Contracts.Models;

namespace MoveIT.Common.Contracts
{
    public interface ITokenManager
    {
        JwtToken? Token { get; }

        void SetToken(string accessToken, string refreshToken, int expiresIn);

        void DeleteToken();
    }
}
