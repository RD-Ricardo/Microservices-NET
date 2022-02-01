using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using GeekShopping.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mysqlConnection = builder.Configuration.GetConnectionString("DefaultConnetion");

builder.Services.AddControllers();
builder.Services.AddDbContext<MySqlContext>(p => p.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MySqlContext>().AddDefaultTokenProviders();

var teste = builder.Services.AddIdentityServer(c =>
{
    c.Events.RaiseErrorEvents = true;
    c.Events.RaiseInformationEvents = true;
    c.Events.RaiseFailureEvents = true;
    c.Events.RaiseSuccessEvents = true;
    c.EmitStaticAudienceClaim = true;

}).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources).AddInMemoryApiScopes(IdentityConfiguration.ApiScopes).AddInMemoryClients(IdentityConfiguration.Clients).AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDBInitializer,DBInitializer>();
builder.Services.AddScoped<IProfileService, ProfileServices>();

teste.AddDeveloperSigningCredential();



builder.Services.AddControllersWithViews();

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
app.UseIdentityServer();
app.UseAuthorization();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var myDependency = services.GetRequiredService<IDBInitializer>();
    myDependency.Initialize();
}

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
