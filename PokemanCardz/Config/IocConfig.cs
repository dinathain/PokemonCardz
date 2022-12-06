using PokemonCardz.Repositories;
using PokemonCardz.Services;

namespace PokemonCardz.Config
{
    public static class IocConfig
    {
        public static IServiceCollection SetupIoc(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IPokemonDataRepository, PokemonDataRepository>();
            services.AddScoped<IUserInventoryRepository, UserInventoryRepository>();

            // Register services
            services.AddScoped<IPokemonDataService, PokemonDataService>();

            return services;
        }
    }
}