using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;

namespace WsFed_Demo
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
                options.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddWsFederation(options =>
                {
                    options.MetadataAddress = "https://login.microsoftonline.com/fb45ee2f-4833-45c5-811f-6b09cdac3578/federationmetadata/2007-06/federationmetadata.xml";
                    
                    options.Wtrealm = "spn:0fffa819-e50e-4f8c-ac54-42fcadbc883d";
                    options.CallbackPath = "/signin-wsfed";
                    //options.TokenValidationParameters.ValidIssuer = "https://sts.windows.net/fb45ee2f-4833-45c5-811f-6b09cdac3578/";
                    //options.TokenValidationParameters.ValidAudience = "";
                    
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

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
