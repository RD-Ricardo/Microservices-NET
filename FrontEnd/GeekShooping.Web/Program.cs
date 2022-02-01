using GeekShooping.Web.Services;
using GeekShooping.Web.Services.IServices;
using GeekShopping.Web.Services;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var isoTeste = builder.Configuration["ServiceUrls:ProductAPI"];
var servicesUrlCart = builder.Configuration["ServiceUrls:CartAPI"];

builder.Services.AddHttpClient<IProductService, ProductService>(c=> c.BaseAddress =  new Uri(isoTeste));
builder.Services.AddHttpClient<ICartService, CartService>(c=> c.BaseAddress =  new Uri(servicesUrlCart));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(c=>
{
    c.DefaultScheme = "Cookies";
    c.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", c=> c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc",c =>
    {
        c.Authority = builder.Configuration["ServiceUrls:IdentityServer"];
        c.GetClaimsFromUserInfoEndpoint = true;
        c.ClientId = "geek_shopping";
        c.ClientSecret = "my_super_secret";
        c.ResponseType = "code";
        c.ClaimActions.MapJsonKey("role", "role", "role");
        c.ClaimActions.MapJsonKey("sub", "sub", "sub");
        c.TokenValidationParameters.NameClaimType = "name";
        c.TokenValidationParameters.RoleClaimType = "role";
        c.Scope.Add("geek_shopping");
        c.SaveTokens = true;
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
