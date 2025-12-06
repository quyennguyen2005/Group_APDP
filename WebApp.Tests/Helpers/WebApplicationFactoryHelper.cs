using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp;
using WebApp.Data;
using WebApp.Services.Auth;

namespace WebApp.Tests.Helpers;

public class WebApplicationFactoryHelper : WebApplicationFactory<IProgramMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the real database
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString());
            });

            // Ensure AuthService is singleton
            var authServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(AuthService));
            
            if (authServiceDescriptor != null && authServiceDescriptor.Lifetime != ServiceLifetime.Singleton)
            {
                services.Remove(authServiceDescriptor);
                services.AddSingleton<AuthService>();
            }
        });
    }
}
