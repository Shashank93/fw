namespace CustomerAPIServices
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System.Text;

    public class Program
    {       
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            
        }

    

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>().UseUrls("http://localhost:9002");                
    }
}
