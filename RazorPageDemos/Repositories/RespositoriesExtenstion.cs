namespace RazorPageDemos.Repositories
{
    public static class RespositoriesExtenstion
    {
        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDepartmentRepository
                , DepartmentRepository>();
            return serviceCollection;
        }
    }
}
