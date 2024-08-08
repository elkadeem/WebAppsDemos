using Microsoft.EntityFrameworkCore;
using RazorPageDemos.Model;
using RazorPageDemos.Options;
using RazorPageDemos.Repositories;

namespace RazorPageDemos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // Configure Logging
            builder.Logging.AddConsole();

            if(builder.Environment.IsDevelopment())
            {
                VacationsRequestsOption vacationsRequestsOption = new VacationsRequestsOption();
                builder.Configuration.GetSection(VacationsRequestsOption.SectionKey)
                    .Bind(vacationsRequestsOption);

                Console.WriteLine($"{vacationsRequestsOption.ApiUrl}-{vacationsRequestsOption.ApiUrl}");

                // Option Pattern with Get
                var vacationsRequestsOptionGet = builder.Configuration
.GetSection(VacationsRequestsOption.SectionKey).Get<VacationsRequestsOption>(); 
                
                // Option Pattern Services
                builder.Services.Configure<VacationsRequestsOption>(builder.Configuration.GetSection(VacationsRequestsOption.SectionKey));

                // Register development services
            }

            builder.Services.AddDbContext<AdventureworksDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorks2019ConnectionString"));
                //options.LogTo(System.Console.WriteLine);
            });
          

            // Add services to the container.
            builder.Services.AddRazorPages();

            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            builder.Services.AddRepositories();

            

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
