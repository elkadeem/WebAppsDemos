using Microsoft.AspNetCore.Authentication.Cookies;

namespace SimpleCookieAuth
{
    public class Program
    {
        public const string SecondCookieSchema = "AnyNameSchemaCookie";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = SecondCookieSchema;
            })
                .AddCookie(options =>
                {
                    options.Cookie.Name = "SimpleCookieAuth";
                    options.LoginPath = "/Login";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.Events.OnValidatePrincipal = context =>
                    {
                        if (context.Principal.HasClaim("UserAgent", context.Request.Headers["User-Agent"]))
                        {
                            return Task.CompletedTask;
                        }
                        context.RejectPrincipal();
                        return Task.CompletedTask;
                    };
                })
                .AddCookie(SecondCookieSchema, options =>
                {
                    options.Cookie.Name = "SimpleCookieAuth.AnyNameSchemaCookie";
                    options.LoginPath = "/Login2";
                    options.AccessDeniedPath = "/AccessDenied";
                });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
