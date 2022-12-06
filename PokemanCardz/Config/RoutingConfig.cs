namespace PokemonCardz.Config
{
    public static class RoutingConfig
    {
        public static WebApplication SetupRouting(this WebApplication app)
        {
            app.UseRouting();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Cards}/{action=Index}");

            return app;
        }
    }
}