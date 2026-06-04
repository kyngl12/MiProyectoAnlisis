using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Jwt;
using MiPrimeraSolucion.UI;
using MiPrimeraSolucion.UI.Models;
using Owin;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Microsoft.Owin.Security;

[assembly: OwinStartup(typeof(GestionPubRock.UI.Startup))]

namespace GestionPubRock.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            CrearRolesYAdministrador();

            var issuer = "miAplicacion";
            var audience = "miAplicacion";
            var secret = Encoding.UTF8.GetBytes("clave_super_secreta_123");

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true
                }
            });
        }

        private void CrearRolesYAdministrador()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

                var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

                if (!roleManager.RoleExists("Administrador"))
                {
                    roleManager.Create(new IdentityRole("Administrador"));
                }

                if (!roleManager.RoleExists("Empleado"))
                {
                    roleManager.Create(new IdentityRole("Empleado"));
                }

                var admin = userManager.FindByEmail("admin@pubrock.com");

                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin@pubrock.com",
                        Email = "admin@pubrock.com"
                    };

                    userManager.Create(admin, "Admin123");

                    userManager.AddToRole(admin.Id, "Administrador");
                }
            }
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}