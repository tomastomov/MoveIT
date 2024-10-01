using Microsoft.Extensions.Options;
using MoveIT.Common.Contracts;
using MoveIT.Common.Extensions;
using MoveIT.Common.Helpers;
using MoveIT.Gateways.Contracts;
using MoveIT.Gateways.Contracts.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static MoveIT.Common.Constants;

namespace MoveIT.Gateways
{
    public class MoveITGateway : IMoveITGateway
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenManager _tokenManager;
        private readonly IOptions<MoveITOptions> _options;

        public MoveITGateway(IHttpClientFactory httpClientFactory, ITokenManager tokenManager, IOptions<MoveITOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _tokenManager = tokenManager;
            _options = options;
        }

        public async Task<Result<AuthenticationResponseModel>> Authenticate()
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(GRANT_TYPE, GrantType.Password.ToSnakeCaseString()),
                new KeyValuePair<string, string>(USERNAME, _options.Value.API_USER),
                new KeyValuePair<string, string>(PASSWORD, _options.Value.API_PASSWORD),
            };

            return await AuthenticateImpl(formData);
        }

        public async Task<Result<AuthenticationResponseModel>> ReAuthenticate(string refreshToken)
        {
            var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(GRANT_TYPE, GrantType.RefreshToken.ToSnakeCaseString()),
                new KeyValuePair<string, string>(REFRESH_TOKEN, refreshToken)
            };

            return await AuthenticateImpl(formData);
        }

        public async Task<Result> UploadFileToDirectory(byte[] file, string fileName, int directoryId)
        {
            var url = string.Format(_options.Value.UPLOAD_FILE_TO_DIRECTORY_URL, directoryId);

            var client = _httpClientFactory.CreateClient();

            using var request = new MultipartFormDataContent();

            request.Add(new ByteArrayContent(file), FILE_REQUEST_PARAM_NAME, fileName);

            var token = _tokenManager.Token;

            if (token is not null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER, token.AccessToken);
            }

            var response = await client.PostAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return Result.ToVoidError(response.StatusCode.ToMessage());
            }

            return Result.ToEmptyResult();
        }

        private async Task<Result<AuthenticationResponseModel>> AuthenticateImpl(List<KeyValuePair<string, string>> formData)
        {
            var client = _httpClientFactory.CreateClient();

            var content = new FormUrlEncodedContent(formData);

            var response = await client.PostAsync(_options.Value.AUTH_URL, content);

            if (!response.IsSuccessStatusCode)
            {
                return Result<AuthenticationResponseModel>.ToError(response.StatusCode.ToMessage());
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<AuthenticationResponseModel>(responseContent);

            return Result<AuthenticationResponseModel>.ToResult(result);
        }
    }
}
