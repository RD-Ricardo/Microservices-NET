using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly MySqlContext _context;

        private readonly UserManager<ApplicationUser> _user;

        private readonly RoleManager<IdentityRole> _role;

        public DBInitializer(UserManager<ApplicationUser> user, RoleManager<IdentityRole> role, MySqlContext context)
        {
            _user = user;
            _role = role;
            _context = context;
        }

        public void Initialize()
        {
            if(_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) { return; }
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult(); 
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
               UserName = "ricardo-Admin",
               Email =  "ricardodias69@gmail.com",
               EmailConfirmed = true,
               PhoneNumber = "+55 (13) 98801-7249",
               FirstName = "Ricardo",
               LastName = "Admin"
            };

            _user.CreateAsync(admin, "Rica0408#").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin,IdentityConfiguration.Admin).GetAwaiter().GetResult();


            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,$"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
            }).Result;


            ApplicationUser client = new ApplicationUser()
            {
                UserName = "ricardo-client",
                Email = "ricardodias69@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (13) 98801-7249",
                FirstName = "Ricardo",
                LastName = "client"
            };

            _user.CreateAsync(client, "Rica0408#").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();


            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,$"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
            }).Result;
        }
    }
}
