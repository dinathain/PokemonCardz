namespace PokemonCardz.Config
{
    public static class AppConfig
    {
        public static IServiceCollection SetupApp(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMemoryCache();
            services.AddLogging();
            return services;
        }

        public static WebApplication SetupApp(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Cards/Error");
                app.UseHsts(); // The default HSTS value is 30 days
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            return app;
        }
    }
}