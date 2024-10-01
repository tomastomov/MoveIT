using MoveIT.Common.Contracts;
using MoveIT.Services.Contracts;

namespace MoveIT.HostedServices
{
    public class TokenRefresherService : IHostedService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenManager _tokenManager;
        private Timer _timer;

        public TokenRefresherService(IAuthenticationService authenticationService, ITokenManager tokenManager)
        {
            _authenticationService = authenticationService;
            _tokenManager = tokenManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async _ => await RefreshToken(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            await _timer.DisposeAsync();
        }

        private async Task RefreshToken()
        {
            var token = _tokenManager.Token;

            if (token is null)
            {
                return;
            }

            if (DateTime.UtcNow > token.ExpirationDate)
            {
                await _authenticationService.ReAuthenticate(token.RefreshToken);
            }
        }
    }
}
