using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using MoveIT.Common;
using MoveIT.Common.Contracts;
using MoveIT.Gateways;
using MoveIT.Gateways.Contracts;
using MoveIT.Gateways.Contracts.Models;
using MoveIT.Helpers.Authorization;
using MoveIT.HostedServices;
using MoveIT.Services;
using MoveIT.Services.Contracts;
using static MoveIT.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IMoveITGateway, MoveITGateway>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddSingleton<ITokenManager, TokenManager>();
builder.Services.AddSingleton<IAuthorizationHandler, TokenAuthorizationHandler>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>(svc =>
{
    var gateway = svc.GetRequiredService<IMoveITGateway>();
    var tokenManager = svc.GetRequiredService<ITokenManager>();

    return new AuthenticationService(gateway, (tokenData) =>
    {
        tokenManager.SetToken(tokenData.AccessToken, tokenData.RefreshToken, tokenData.ExpiresIn);
    });
});

builder.Services.Configure<MoveITOptions>(builder.Configuration.GetSection("MoveIT"));

builder.Services.AddHostedService<TokenRefresherService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(HAS_TOKEN_POLICY, policy =>
        policy.Requirements.Add(new HasTokenRequirement()));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.LoginPath = new PathString($"/{AUTHENTICATION_CONTROLLER}/{LOGIN_ACTION}");
            options.AccessDeniedPath = new PathString($"/{INDEX_ACTION}");
        });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
