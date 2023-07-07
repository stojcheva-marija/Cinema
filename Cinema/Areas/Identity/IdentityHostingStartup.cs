using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Cinema.Web.Areas.Identity.IdentityHostingStartup))]
namespace Cinema.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}