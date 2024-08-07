using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DIDemosConsoleApp.DISample
{
    public interface ISampleService
    {
        string Name { get; }
    }

    public interface ITransientSerivce : ISampleService
    {
    }

    public interface IScopedService : ISampleService
    {
    }

    public class SampleService : ISampleService, ITransientSerivce
        , IScopedService
    {
        public SampleService()
        {
            Name = Guid.NewGuid().ToString();
        }

        public string Name { get; }
    }

    public class SampleService2 : ISampleService
    {
        public SampleService2()
        {
            Name = $"Service 2: {Guid.NewGuid().ToString()}";
        }
        public string Name { get; }
    }

    public class UpperService
    {
        private readonly ISampleService _sampleService;

        public UpperService(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        public override string ToString()
        {
            return $"Hi I am using {_sampleService.Name}";
        }
    }

    public class UpperServiceWithScoped
    {
        private readonly IScopedService _sampleService;

        public UpperServiceWithScoped(IScopedService sampleService)
        {
            _sampleService = sampleService;
        }

        public override string ToString()
        {
            return $"Hi I am using {_sampleService.Name}";
        }
    }

    public class UpperServiceWithTransient
    {
        private readonly ITransientSerivce _sampleService;

        public UpperServiceWithTransient(ITransientSerivce sampleService)
        {
            _sampleService = sampleService;
        }

        public override string ToString()
        {
            return $"Hi I am using {_sampleService.Name}";
        }
    }

    public static class DIConsumer
    {
        public static void Consume()
        {
            SampleService sampleService = new SampleService();
            SampleService sampleService2 = new SampleService();

            Console.WriteLine(sampleService.Name);
            Console.WriteLine(sampleService2.Name);
            Console.WriteLine(sampleService.Name.Equals(sampleService2.Name));

            ServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ISampleService, SampleService>();

            serviceCollection.AddSingleton<ISampleService, SampleService2>();

            serviceCollection.AddSingleton<SampleService>();

            serviceCollection.AddTransient<ITransientSerivce, SampleService>();

            serviceCollection.AddScoped<IScopedService, SampleService>();

            // Register dependant service
            serviceCollection.AddSingleton<UpperService>();

            serviceCollection.AddTransient<UpperServiceWithScoped>();

            serviceCollection
                .AddTransient<UpperServiceWithTransient>();
            // Build the service provider
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            // Consume the service
            ISampleService sampleService3 = serviceProvider.GetRequiredService<ISampleService>();
            ISampleService sampleService4 = serviceProvider.GetRequiredService<ISampleService>();

            Console.WriteLine("Show singleton Serivce");
            Console.WriteLine(sampleService3.Name);
            Console.WriteLine(sampleService4.Name);
            Console.WriteLine(sampleService3.Name.Equals(sampleService4.Name));

            Console.WriteLine("Create another scope");
            using var scope = serviceProvider.CreateScope();
            ISampleService sampleService5 = scope.ServiceProvider.GetRequiredService<ISampleService>();
            ISampleService sampleService6 = scope.ServiceProvider.GetRequiredService<ISampleService>();

            Console.WriteLine(sampleService5.Name);
            Console.WriteLine(sampleService6.Name);
            Console.WriteLine(sampleService5.Name.Equals(sampleService6.Name));



            Console.WriteLine("Show Transient Service");
            ITransientSerivce transientSerivce1 = serviceProvider.GetRequiredService<ITransientSerivce>();
            ITransientSerivce transientSerivce2 = serviceProvider.GetRequiredService<ITransientSerivce>();

            ITransientSerivce transientSerivce3 = scope.ServiceProvider.GetRequiredService<ITransientSerivce>();

            Console.WriteLine(transientSerivce1.Name);
            Console.WriteLine(transientSerivce2.Name);
            Console.WriteLine(transientSerivce3.Name);

            Console.WriteLine("Show Scoped Service");

            IScopedService scopedService1 = serviceProvider.GetRequiredService<IScopedService>();
            IScopedService scopedService2 = serviceProvider.GetRequiredService<IScopedService>();

            IScopedService scopedService3 = scope.ServiceProvider.GetRequiredService<IScopedService>();
            IScopedService scopedService4 = scope.ServiceProvider.GetRequiredService<IScopedService>();

            Console.WriteLine(scopedService1.Name);
            Console.WriteLine(scopedService2.Name);
            Console.WriteLine(scopedService1.Name.Equals(scopedService2.Name));


            Console.WriteLine(scopedService3.Name);
            Console.WriteLine(scopedService4.Name);
            Console.WriteLine(scopedService3.Name.Equals(scopedService4.Name));

            SampleService service = serviceProvider
                .GetRequiredService<SampleService>();

            // Get all ISampleService
            Console.WriteLine("Get List of services");
            IEnumerable<ISampleService> sampleServices = serviceProvider.GetServices<ISampleService>();

            foreach (var item in sampleServices)
            {
                Console.WriteLine(item.Name);
            }

            // Call other services
            ConsumeDepndantService(serviceProvider);

        }

        public static void ConsumeDepndantService(ServiceProvider serviceProvider)
        {
            var upperService = serviceProvider.GetRequiredService<UpperService>();

            Console.WriteLine(upperService.ToString());

            Console.WriteLine("Show Scoped Service");
            var upperServiceWithScoped = serviceProvider.GetRequiredService<UpperServiceWithScoped>();
            Console.WriteLine(upperServiceWithScoped.ToString());

            // create a new scope
            using var scope = serviceProvider.CreateScope();
            upperServiceWithScoped = scope.ServiceProvider.GetRequiredService<UpperServiceWithScoped>();
            Console.WriteLine(upperServiceWithScoped.ToString());

            Console.WriteLine("Show Transient Service");
            var upperServiceWithTransient = serviceProvider.GetRequiredService<UpperServiceWithTransient>();
            Console.WriteLine(upperServiceWithTransient.ToString());

            upperServiceWithTransient = serviceProvider.GetRequiredService<UpperServiceWithTransient>();
            Console.WriteLine(upperServiceWithTransient.ToString());

        }
    }
}
