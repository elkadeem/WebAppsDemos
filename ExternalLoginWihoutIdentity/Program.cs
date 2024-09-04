using ExternalLoginWihoutIdentity.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExternalLoginWihoutIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
            }
                )

                .AddCookie()
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
                    options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                    options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
                    options.Events.OnTicketReceived = context =>
                    {
                        var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                        // Add custom claims here
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth
                            , new DateTime(1980, 1, 1).ToString()));
                        claimsIdentity.AddClaim(new Claim("user-agent", context.Request.Headers["User-Agent"].ToString() ?? "unknown"));
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "MicrosoftEmployee"));
                        return Task.CompletedTask;
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    options.Events.OnTicketReceived = context =>
                    {
                        var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                        // Add custom claims here

                        claimsIdentity.AddClaim(new Claim("user-agent", context.Request.Headers["User-Agent"].ToString() ?? "unknown"));
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "GoogleEmployee"));
                        return Task.CompletedTask;
                    };
                });

            builder.Services.AddAuthorization(options =>
            {

                options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());

                options.AddPolicy("MicrosoftEmployee", policy =>
                policy.RequireRole("MicrosoftEmployee"));

                options.AddPolicy("GoogleEmployee", policy =>
                 policy.RequireRole("GoogleEmployee"));

                options.AddPolicy("AllowedFamilies", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Surname, "Smith", "Johnson", "Williams", "Jones", "Brown");
                });

                options.AddPolicy("MinimumAge18", policy =>
                {
                    policy.AddRequirements(new MinimumAgeRequirement(18));
                });

                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim(ClaimTypes.Role)
                    .AddRequirements(new ChromeRequirmenet(), new BrowserVersionRequirmenet())
                    .Build();

            });

            builder.Services.AddTransient<IAuthorizationHandler, MinimumAgeAuhorizationHandler>();
            builder.Services.AddTransient<IAuthorizationHandler, BrowserAuthorizationHandler>();

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
