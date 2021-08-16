using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.ExternalServices;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IPhotoService, PhotoService>(c =>
            {
                c.BaseAddress = new Uri("http://jsonplaceholder.typicode.com");
            });
        }
    }
}
