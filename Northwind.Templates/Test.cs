using Microsoft.Extensions.DependencyInjection;

namespace Northwind.Templates
{
    public sealed class Test
    {
        public void Run(IServiceCollection services)
        {
            services.AddScoped<IHello>();
        }
    }
}
