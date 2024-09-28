using Microsoft.Extensions.Options;
using MoveIT.Common;
using MoveIT.Gateways.Contracts;
using MoveIT.Gateways.Contracts.Models;
using Newtonsoft.Json;
using System.Text;
using static MoveIT.Common.Constants;

namespace MoveIT.Gateways
{
    public class MoveITGateway : IMoveITGateway
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<MoveITOptions> _options;

        public MoveITGateway(IHttpClientFactory httpClientFactory, IOptions<MoveITOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options;
        }

        public async Task<string> LoginAsync(string username)
        {
            var client = _httpClientFactory.CreateClient();

            var credentials = new
            {
                username = _options.Value.API_USER,
                password = _options.Value.API_PASSWORD
            };

            var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, APPLICATION_JSON);

            var response = await client.PostAsync(_options.Value.AUTH_URL, content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<LoginResponseModel>(responseContent);

            return result?.AccessToken;
        }
    }
}
