using PokemonCardz.Models;

namespace PokemonCardz.Repositories
{
    public interface IPokemonDataRepository
    {
        /// <summary>
        /// Gets the pokemon details for a given array of pokemon names with string value
        /// </summary>
        /// <returns>A list of Pokemon</returns>
        Task<List<Pokemon>> GetPokemonByNamesAsync(string[] names);
    }
}