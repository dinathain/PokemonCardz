using Microsoft.AspNetCore.Mvc;
using PokemonCardz.Models;
using PokemonCardz.Services;
using X.PagedList;

namespace PokemonCardz.Controllers
{
    public class CardsController : Controller
    {
        private const int PageSize = 10;
        private readonly ILogger<CardsController> _logger;
        private readonly IPokemonDataService _pokemonDataService;

        public CardsController(ILogger<CardsController> logger, IPokemonDataService pokemonDataService)
        {
            _logger = logger;
            _pokemonDataService = pokemonDataService;
        }

        public async Task<ViewResult> IndexAsync(int? page)
        {
            var pokemonList = new List<Pokemon>();
            try
            {
                pokemonList = await _pokemonDataService.GetUsersPokemonAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return View(pokemonList.ToPagedList(page ?? 1, PageSize));
        }
    }
}