namespace RazorPageDemos.Options
{
    public class VacationsRequestsOption
    {
        public const string SectionKey = "VacationRequests";
        public string ApiKey { get; set; }

        public string ApiUrl { get; set; }

        public string[] Scopes { get; set; }

        public Tenant[] Tenants { get; set; }
    }

    public class Tenant
    {
       public string Name { get; set; }
        public string ClientId { get; set; }
    }
}
