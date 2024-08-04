using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Model;

namespace RazorPageDemos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AdventureworksDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorks2019ConnectionString"));
                options.LogTo(System.Console.WriteLine);
            });
          
            // Add services to the container.
            builder.Services.AddRazorPages();

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
