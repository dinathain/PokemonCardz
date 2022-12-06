using PokemonCardz.Models;
using PokemonCardz.Repositories;

namespace PokemonCardz.Services
{
    public class PokemonDataService : IPokemonDataService
    {
        private readonly IUserInventoryRepository _userInventoryRepository;
        private readonly IPokemonDataRepository _pokemonDataRepository;
        private readonly ILogger<PokemonDataService> _logger;

        public PokemonDataService(
            ILogger<PokemonDataService> logger,
            IUserInventoryRepository userInventoryRepository,
            IPokemonDataRepository pokemonDataRepository)
        {
            _userInventoryRepository = userInventoryRepository;
            _pokemonDataRepository = pokemonDataRepository;
            _logger = logger;
        }

        public async Task<List<Pokemon>> GetUsersPokemonAsync()
        {
            var pokemonList = new List<Pokemon>();
            try
            {
                // Get pokemon names from user inventory
                var pokemonNames = _userInventoryRepository.GetAll();
                if (pokemonNames.Length > 0)
                {
                    // Remove duplicates from names
                    var cleanedNames = pokemonNames.Distinct().ToArray();

                    // Get pokemon details
                    pokemonList =
                        (await _pokemonDataRepository.GetPokemonByNamesAsync(cleanedNames))
                        .OrderByDescending(x => x.BaseExperience)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return pokemonList;
        }
    }
}