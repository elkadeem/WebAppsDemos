﻿using DIDemosConsoleApp.DISample;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DIDemosConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configuration system
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            // Add Json file
            configurationBuilder.AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            var configuration = configurationBuilder.Build();
            
            Console.WriteLine($"DefaultConnection: {configuration.GetConnectionString("DefaultConnection")}");

            Console.WriteLine($"DefaultConnection: {configuration["ConnectionStrings:DefaultConnection"]}");

            Console.WriteLine($"VacationApiUrl: {configuration["Vacations:VacationApiUrl"]}");

            Console.WriteLine($"VacationApiUrl: {configuration.GetSection("Vacations")["VacationApiUrl"]}");

            Console.WriteLine($"VacationApiUrl: {configuration.GetSection("Vacations")["VacationApiScope"]}");

            Console.ReadLine();

            DIConsumer.Consume();
            Console.ReadLine();

            //SampleUsingNew();

            // Create DI container and register types into it
            // Create Service collection
            var serviceCollection = new ServiceCollection();

            // Register Services
            serviceCollection.AddTransient<DepartmentsService>();
            serviceCollection.AddTransient<IDepartmentRepository, DepartmentRespository>();

            // Register Servicex
            serviceCollection.AddSingleton<ISingletonService, ServiceX>();
            serviceCollection.AddScoped<IScopedService, ServiceX>();
            serviceCollection.AddTransient<ITransientSerivce, ServiceX>();

            serviceCollection.AddKeyedSingleton<ISingletonService
                , ServiceX>("SeriveY");
            // Creating Service Provider 
            var serviceProvider = serviceCollection.BuildServiceProvider();

            using var scope1 = serviceProvider.CreateScope();

            // Singleton Sample
            ISingletonService singletonService1 = serviceProvider.GetRequiredService<ISingletonService>();
            ISingletonService singletonService2 = scope1.ServiceProvider
                .GetRequiredService<ISingletonService>();

            var serivcey = serviceProvider
                .GetRequiredKeyedService<ISingletonService>("SeriveY");

            Console.WriteLine(singletonService1.Name);
            Console.WriteLine(singletonService2.Name);
            Console.WriteLine(singletonService1.Name.Equals(singletonService2
                .Name));

            Console.WriteLine($"Servicey: {serivcey.Name}");

            // Transient 
            ITransientSerivce transientSerivce1 = serviceProvider
                .GetRequiredService<ITransientSerivce>();
            ITransientSerivce transientSerivce2 = serviceProvider
                .GetRequiredService<ITransientSerivce>();

            ITransientSerivce transientSerivce3 = scope1.ServiceProvider
                .GetRequiredService<ITransientSerivce>();

            Console.WriteLine (transientSerivce1.Name);
            Console.WriteLine(transientSerivce2.Name);
            Console.WriteLine(transientSerivce3.Name);
            Console.WriteLine("End of transient Sample");

            // Add Scoped Service
            IScopedService scopedService1 = serviceProvider.GetRequiredService<IScopedService>();
            IScopedService scopedService2 = serviceProvider.GetRequiredService<IScopedService>();

            Console.WriteLine("********** Scoped Samples *************");
            Console.WriteLine(scopedService1.Name);
            Console.WriteLine(scopedService2.Name);


            IScopedService scopedService3 = scope1.ServiceProvider.GetRequiredService<IScopedService>();
            IScopedService scopedService4 = scope1. ServiceProvider.GetRequiredService<IScopedService>();

            Console.WriteLine(scopedService3.Name);
            Console.WriteLine(scopedService4.Name);

            Console.ReadLine();

            // Resolve types
            var departmentService = serviceProvider.GetRequiredService<DepartmentsService>();

            using var scope = serviceProvider.CreateScope();
            var departmentService2 = scope.ServiceProvider
                .GetRequiredService<DepartmentsService>();

            Console.WriteLine("Hello, World!");
        }

        private static void SampleUsingNew()
        {
            //DepartmentRespository departmentRespository = new DepartmentRespository("ConnectionString");
            OracleDepartmentRepository departmentRespository
                 = new OracleDepartmentRepository("oracleConnectionstring");
            DepartmentsService departmentsService
                = new DepartmentsService(departmentRespository);
        }
    }

    public interface IServiceX
    {
        string Name { get; }
    }
    public interface ISingletonService : IServiceX
    {
    }

    public interface IScopedService : IServiceX
    {

    }

    public interface ITransientSerivce : IServiceX
    {

    }

    public class ServiceX: ISingletonService, IScopedService, ITransientSerivce, IServiceX
    {
        public ServiceX()
        {
            Name = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
    }

}
