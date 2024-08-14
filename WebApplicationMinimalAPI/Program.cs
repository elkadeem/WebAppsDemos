
using Microsoft.EntityFrameworkCore;
using WebApplicationMinimalAPI.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using WebApplicationMinimalAPI;
using WebApplicationMinimalAPI.Entities;

namespace WebApplicationMinimalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AdventureworksDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureworksDbContext")));

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.MapDepartmentEndpoints();

            app.Run();
        }
    }


    public static class DepartmentEndpoints
    {
        public static void MapDepartmentEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Departments").WithTags(nameof(Department));

            group.MapGet("/", async (AdventureworksDbContext db) =>
            {
                return await db.Departments.AsNoTracking()
                .Select(c => new DepartmentDto(c.DepartmentId, c.Name, c.GroupName))
                .ToListAsync();
            })
            .WithName("GetAllDepartments")
            .WithOpenApi();

            group.MapGet("/{id}", async Task<Results<Ok<DepartmentDto>, NotFound>> (short departmentid, AdventureworksDbContext db) =>
            {
                return await db.Departments.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.DepartmentId == departmentid)
                    is Department model
                        ? TypedResults.Ok(new DepartmentDto(model.DepartmentId
                        , model.Name, model.GroupName))
                        : TypedResults.NotFound();
            })
            .WithName("GetDepartmentById")
            .WithOpenApi();

            group.MapPut("/{id}", async Task<Results<Ok, BadRequest, NotFound>> (short departmentid, DepartmentDto departmentDto, AdventureworksDbContext db) =>
            {
                if (departmentid != departmentDto.DepartmentId)
                {
                    return TypedResults.BadRequest();
                }

                var department = await db.Departments.FindAsync(departmentid);
                if (department == null)
                {
                    return TypedResults.NotFound();
                }

                department.Name = departmentDto.Name;
                department.GroupName = departmentDto.GroupName;
                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithName("UpdateDepartment")
            .WithOpenApi();

            group.MapPost("/", async (Department department, AdventureworksDbContext db) =>
            {
                db.Departments.Add(department);
                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/Departments/{department.DepartmentId}", department);
            })
            .WithName("CreateDepartment")
            .WithOpenApi();

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (short departmentid, AdventureworksDbContext db) =>
            {
                var affected = await db.Departments
                    .Where(model => model.DepartmentId == departmentid)
                    .ExecuteDeleteAsync();
                return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
            })
            .WithName("DeleteDepartment")
            .WithOpenApi();
        }
    }

    public record DepartmentDto(int DepartmentId, string Name, string GroupName);
}
