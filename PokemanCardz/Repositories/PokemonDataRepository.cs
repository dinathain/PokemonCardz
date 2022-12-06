using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PokemonCardz.Models;
using System.Net.Http.Headers;

namespace PokemonCardz.Repositories
{
    public class PokemonDataRepository : IPokemonDataRepository
    {
        private const string BaseUrl = "https://pokeapi.co/api/v2/pokemon/";
        private const int CacheExpirationMinutes = 60;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;

        public PokemonDataRepository(ILogger<PokemonDataRepository> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        private static int PaginationLimit => 500;

        private string UrlParameters
        {
            get
            {
                return $"?limit={PaginationLimit}";
            }
        }

        public async Task<List<Pokemon>> GetPokemonByNamesAsync(string[] names)
        {
            var result = new List<Pokemon>();

            // Get all pokemon resources (in order to get pokemon IDs for quicker API calls)
            var allPokemonResources = (await GetAllPokemonAsync()).ToDictionary(x => x.Name);
            if (allPokemonResources == null)
            {
                throw new Exception($"Error occured whilst attempting to get the full resource list from the API.");
            }

            // Get pokemon details
            foreach (var name in names)
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogInformation($"{nameof(GetPokemonByNamesAsync)} - empty name found, ignored.");
                    continue;
                }

                var cacheKeyName = $"{CacheKeys.Pokemon}-{name.ToLower()}";

                // Check if exists in cached data
                if (_memoryCache.TryGetValue(cacheKeyName, out Pokemon cachedPokemon))
                {
                    result.Add(cachedPokemon);
                    continue;
                }

                // If not, get from API
                var cleanName = CleanPokemonName(name);
                if (!allPokemonResources.ContainsKey(cleanName))
                {
                    _logger.LogInformation($"{nameof(GetPokemonByNamesAsync)} - name ({cleanName}) not found in API, ignored.");
                    continue;
                }
                var pokemon = await GetPokemonFromUrlAsync(allPokemonResources[cleanName].Url);
                if (pokemon == null)
                {
                    throw new Exception($"Error occured whilst attempting to get data from {allPokemonResources[cleanName].Url} from the API.");
                }
                result.Add(pokemon);

                // Add to cache
                AddToPokemonCache(cacheKeyName, pokemon);
            }
            return result;
        }

        private static string CleanPokemonName(string name)
        {
            return name.Trim().ToLower();
        }

        private void AddToPokemonCache(string cacheKeyName, Pokemon pokemon)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(60));
            _memoryCache.Set(cacheKeyName, pokemon, cacheEntryOptions);
        }

        private void AddToResourceListCache(string cacheKeyName, List<PokemonResource> pokemonResourceList)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(CacheExpirationMinutes));
            _memoryCache.Set(cacheKeyName, pokemonResourceList, cacheEntryOptions);
        }

        private async Task<Pokemon?> GetPokemonFromUrlAsync(string url)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Response from {url} unsuccessful - {response.StatusCode} - {response.ReasonPhrase}");
                }
                else
                {
                    if (response.Content == null)
                    {
                        throw new Exception($"Response from {url} is invalid - Content is null.");
                    }
                    else
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var pokemonDetails = JsonConvert.DeserializeObject<PokemonDetails>(responseBody);

                        if (pokemonDetails == null)
                        {
                            throw new Exception($"Response from {url} is empty following deserialisation.");
                        }
                        else
                        {
                            // Validate results
                            if (pokemonDetails.Id <= 0)
                            {
                                throw new Exception($"Response from {url} is unexpected - Id is less than or equal to 0.");
                            }

                            if (string.IsNullOrEmpty(pokemonDetails.Name))
                            {
                                throw new Exception($"Response from {url} is unexpected - Name is empty or null.");
                            }

                            if (pokemonDetails.BaseExperience <= 0)
                            {
                                throw new Exception($"Response from {url} is unexpected - Base Experience is less than or equal to 0.");
                            }

                            return new Pokemon()
                            {
                                Id = pokemonDetails.Id,
                                Name = pokemonDetails.Name,
                                BaseExperience = pokemonDetails.BaseExperience
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        private async Task<List<PokemonResource>?> GetAllPokemonAsync()
        {
            try
            {
                if (!_memoryCache.TryGetValue(CacheKeys.FullResourceList, out List<PokemonResource> cachedResourceList))
                {
                    var url = BaseUrl + UrlParameters;
                    var result = new List<PokemonResource>();
                    while (url != null)
                    {
                        var client = new HttpClient();
                        client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                        var response = await client.GetAsync(url);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Response from API for resource list unsuccessful - {response.StatusCode} - {response.ReasonPhrase}");
                        }
                        else
                        {
                            if (response.Content == null)
                            {
                                throw new Exception($"Response from API for resource list is invalid - Content is null.");
                            }
                            else
                            {
                                var responseBody = await response.Content.ReadAsStringAsync();
                                var pokemonResourceList = JsonConvert.DeserializeObject<PokemonResourceList>(responseBody);
                                if (pokemonResourceList == null || pokemonResourceList.Results == null)
                                {
                                    throw new Exception($"Response from API for resource list is unexpected - following deserialisatino, no content or results found.");
                                }

                                url = pokemonResourceList.NextUrl;
                                result.AddRange(pokemonResourceList.Results);
                            }
                        }
                    }
                    AddToResourceListCache(CacheKeys.FullResourceList, result);
                    return result;
                }
                return cachedResourceList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}