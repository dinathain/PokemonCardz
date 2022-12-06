using PokemonCardz.Models;

namespace PokemonCardz.Services
{
    public interface IPokemonDataService
    {
        /// <summary>
        /// Gets the pokemon details for the user's inventory
        /// </summary>
        /// <returns>A list of Pokemon</returns>
        Task<List<Pokemon>> GetUsersPokemonAsync();
    }
}