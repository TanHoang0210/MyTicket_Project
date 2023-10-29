using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MYTICKET.BASE.IdentityServer.Services
{
    public class Worker<TDbContext> : IHostedService where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await context.Database.EnsureCreatedAsync();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("client-angular", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "client-angular",
                    ClientSecret = "52F4A9A45C1F21B53B62F56DA52F7",
                    DisplayName = "Web Angular",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Revocation,

                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        //Permissions.GrantTypes.ClientCredentials,
                    }
                }, cancellationToken);
            }

            if (await manager.FindByClientIdAsync("client-app", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "client-app",
                    ClientSecret = "B5F7FADE886FBEF68C9B94ADC9F68",
                    DisplayName = "App mobile",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Revocation,

                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.RefreshToken,
                        //Permissions.GrantTypes.ClientCredentials,
                    }
                }, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
