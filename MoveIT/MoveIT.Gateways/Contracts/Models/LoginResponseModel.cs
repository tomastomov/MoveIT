using Newtonsoft.Json;
using static MoveIT.Common.Constants;

namespace MoveIT.Gateways.Contracts.Models
{
    public class LoginResponseModel
    {
        [JsonProperty(ACCESS_TOKEN)]
        public string AccessToken { get; set; }

        [JsonProperty(REFRESH_TOKEN)]
        public string RefreshToken { get; set; }
    }
}
