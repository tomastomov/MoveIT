using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MoveIT.Tests.GatewaysTests
{
    public class MockDelegatingHandler : DelegatingHandler
    {
        private readonly HttpResponseMessage _fakeResponse;

        public MockDelegatingHandler(HttpResponseMessage responseMessage)
        {
            _fakeResponse = responseMessage;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_fakeResponse);
        }
    }
}
