using Microsoft.AspNetCore.Authentication.Cookies;
using WOWFocus.Application.Interfaces;
using WOWFocus.Application.Services;
using WOWFocus.Infrastructure.Repositories;
using WOWFocus.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var identityFile = builder.Configuration["JsonStorage:IdentityFile"]!;
builder.Services.AddScoped<IIdentityRepository>(_ => new JsonIdentityRepository(identityFile));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IdentitySeeder>();
builder.Services.AddScoped<IdentityAdminService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        var hours = builder.Configuration.GetValue<int>("Auth:SessionHours");
        opt.LoginPath = "/Account/Login";
        opt.ExpireTimeSpan = TimeSpan.FromHours(hours);
        opt.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("perm:admin.manage",
        policy => policy.RequireClaim("perm", "admin.manage"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IdentitySeeder>();
    await seeder.SeedAsync();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.Run();