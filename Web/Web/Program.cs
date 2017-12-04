using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Web.Data;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //try
                //{
                DbInitializer.Initialize(services.GetRequiredService<MovieContext>());
                DbInitializer.Initialize(services.GetRequiredService<SongContext>());
                //}
                //catch (Exception ex)
                //{
                //    var logger = services.GetRequiredService<ILogger<Program>>();
                //    logger.LogError(new EventId(1),ex,"An error occurred while seeding the database.");
                //}
            }

            host.Run();
        }
    }
}
