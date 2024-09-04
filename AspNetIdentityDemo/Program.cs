global using AspNetIdentityDemo.Entities.Identity;
using AspNetIdentityDemo.Data;
using AspNetIdentityDemo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace AspNetIdentityDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;                
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole>()
                ;

            builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
            builder.Services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

            builder.Services.AddRazorPages();

            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                })
                .AddMicrosoftAccount(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
                    options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                    options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
                });

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
