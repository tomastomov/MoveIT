using Newtonsoft.Json;
using static MoveIT.Common.Constants;

namespace MoveIT.Gateways.Contracts.Models
{
    public class AuthenticationResponseModel
    {
        [JsonProperty(ACCESS_TOKEN)]
        public string AccessToken { get; set; }

        [JsonProperty(REFRESH_TOKEN)]
        public string RefreshToken { get; set; }

        [JsonProperty(EXPIRES_IN)]
        public int ExpiresIn { get; set; }
    }
}
