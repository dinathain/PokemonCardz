using Newtonsoft.Json;

namespace PokemonCardz.Repositories
{
    public class PokemonDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("base_experience")]
        public int BaseExperience { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }

        [JsonProperty("abilities")]
        public object? Abilities { get; set; }

        [JsonProperty("forms")]
        public object? Forms { get; set; }

        [JsonProperty("game_indices")]
        public object? GameIndices { get; set; }

        [JsonProperty("held_items")]
        public object? HeldItems { get; set; }

        [JsonProperty("location_area_encounters")]
        public string? LocationAreaEncounters { get; set; }

        [JsonProperty("moves")]
        public object? Moves { get; set; }

        [JsonProperty("species")]
        public object? Species { get; set; }

        [JsonProperty("sprites")]
        public object? Sprites { get; set; }

        [JsonProperty("stats")]
        public object? Stats { get; set; }

        [JsonProperty("types")]
        public object? Types { get; set; }

        [JsonProperty("past_types")]
        public object? PastTypes { get; set; }
    }
}