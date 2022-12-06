namespace PokemonCardz.Config
{
    public static class AuthConfig
    {
        /// <summary>
        /// Webapp authentication setup
        /// </summary>
        /// <param name="webApp"></param>
        public static WebApplication SetupAuth(this WebApplication webApp)
        {
            webApp.UseAuthorization();
            return webApp;
        }
    }
}