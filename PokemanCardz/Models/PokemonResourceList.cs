using Newtonsoft.Json;

namespace PokemonCardz.Repositories
{
    public class PokemonResourceList
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("next")]
        public string? NextUrl { get; set; }

        [JsonProperty("previous")]
        public string? PreviousUrl { get; set; }

        [JsonProperty("results")]
        public List<PokemonResource> Results { get; set; }
    }

    public class PokemonResource
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}