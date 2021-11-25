using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WookieBooks.DataContext;
using WookieBooks.Models;
using Microsoft.Extensions.Logging;

namespace WookieBooks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //getting the IWebHost that will host this application
            var host = CreateHostBuilder(args).Build();

            //finding the service layer within the scope
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //create sample data
                BooksDataGenerator.Initialize(services);
            }

            //continue to run the application
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
