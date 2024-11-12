using LdapAuthorizationService;
using System.Reflection;

internal class Program
{
	public static AssemblyName ServiceName = Assembly.GetExecutingAssembly().GetName();

	public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}