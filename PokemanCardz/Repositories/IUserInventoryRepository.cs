namespace PokemonCardz.Repositories
{
    public interface IUserInventoryRepository
    {
        /// <summary>
        /// Gets the names of all pokemon in the user's inventory
        /// </summary>
        /// <returns>An array of string values for each name</returns>
        string[] GetAll();
    }
}