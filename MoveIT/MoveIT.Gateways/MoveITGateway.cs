﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MoveIT.Gateways.Contracts;
using MoveIT.Gateways.Contracts.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using static MoveIT.Common.Constants;

namespace MoveIT.Gateways
{
    public class MoveITGateway : IMoveITGateway
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IOptions<MoveITOptions> _options;

        public MoveITGateway(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IOptions<MoveITOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
            _options = options;
        }

        public async Task<string> Login(string username)
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

        public async Task UploadFileToDirectory(byte[] file, string fileName, string directory)
        {
            var url = string.Format(_options.Value.UPLOAD_FILE_TO_DIRECTORY_URL, directory);

            var client = _httpClientFactory.CreateClient();

            using var request = new MultipartFormDataContent();

            request.Add(new ByteArrayContent(file), "file", fileName);

            var token = _contextAccessor.HttpContext.Session.GetString(JWT);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER, token);

            var response = await client.PostAsync(url, request);

            if (!response.IsSuccessStatusCode)
            {
                return;
            }
        }

        public async Task CreateUser(string username, string password)
        {

        }
    }
}
