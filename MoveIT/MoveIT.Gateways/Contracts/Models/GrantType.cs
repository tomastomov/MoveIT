using System.ComponentModel;

namespace MoveIT.Gateways.Contracts.Models
{
    public enum GrantType
    {
        [Description("password")]
        Password = 1,

        [Description("refresh_token")]
        RefreshToken = 2,
    }
}
