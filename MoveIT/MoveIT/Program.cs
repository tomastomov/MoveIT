using MoveIT.Common;
using MoveIT.Common.Contracts;
using MoveIT.Gateways;
using MoveIT.Gateways.Contracts;
using MoveIT.Gateways.Contracts.Models;
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
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddHostedService<TokenRefresherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

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
