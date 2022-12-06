using Microsoft.Extensions.Logging;
using Moq;
using PokemonCardz.Models;
using PokemonCardz.Repositories;
using PokemonCardz.Services;

namespace PokemonCardzTests
{
    public class PokemonDataServiceTests
    {
        [Fact]
        public async Task GetUsersPokemonAsync_ShouldReturnsTheResultFromRepository()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<PokemonDataService>>();
            var mockUserInventoryRepo = new Mock<IUserInventoryRepository>();
            var mockPokemonDataRepo = new Mock<IPokemonDataRepository>();

            string[] ownedPokemonNames = { "Bulbasaur", "Charmander", "Piplup", "Squirtle" };
            mockUserInventoryRepo
                .Setup(x => x.GetAll())
                .Returns(ownedPokemonNames);

            var pokemonList = new List<Pokemon>() {
                new Pokemon() { Id = 1, Name = "Bulbasaur", BaseExperience = 88 },
                new Pokemon() { Id = 2, Name = "Piplup", BaseExperience = 17 }
            };
            mockPokemonDataRepo
                .Setup(x => x.GetPokemonByNamesAsync(ownedPokemonNames))
                .ReturnsAsync(pokemonList);

            var sut = new PokemonDataService(mockLogger.Object, mockUserInventoryRepo.Object, mockPokemonDataRepo.Object);

            //Act
            var result = await sut.GetUsersPokemonAsync();

            //Assert
            Assert.Equal(pokemonList, result);
        }

        [Fact]
        public async Task GetUsersPokemonAsync_ShouldRemoveDuplicatesBeforeCallingRepository()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<PokemonDataService>>();
            var mockUserInventoryRepo = new Mock<IUserInventoryRepository>();
            var mockPokemonDataRepo = new Mock<IPokemonDataRepository>();

            string[] ownedPokemonNames = { "Bulbasaur", "Squirtle", "Bulbasaur" };
            mockUserInventoryRepo
                .Setup(x => x.GetAll())
                .Returns(ownedPokemonNames);

            var sut = new PokemonDataService(mockLogger.Object, mockUserInventoryRepo.Object, mockPokemonDataRepo.Object);

            //Act
            var result = await sut.GetUsersPokemonAsync();

            //Assert
            var expectedRepoArgs = new string[] { "Bulbasaur", "Squirtle" };
            mockPokemonDataRepo.Verify(
                _ => _.GetPokemonByNamesAsync(It.Is<string[]>(u => u.Length == expectedRepoArgs.Length)));
            mockPokemonDataRepo.Verify(
                _ => _.GetPokemonByNamesAsync(It.Is<string[]>(u => u[0] == expectedRepoArgs[0])));
            mockPokemonDataRepo.Verify(
                _ => _.GetPokemonByNamesAsync(It.Is<string[]>(u => u[1] == expectedRepoArgs[1])));
        }

        [Fact]
        public async Task GetUsersPokemonAsync_ShouldReturnEmptyListIfErrorThrownByUserInventoryRepo()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<PokemonDataService>>();
            var mockUserInventoryRepo = new Mock<IUserInventoryRepository>();
            var mockPokemonDataRepo = new Mock<IPokemonDataRepository>();

            mockUserInventoryRepo
                .Setup(x => x.GetAll())
                .Throws(new Exception("error occured"));

            var sut = new PokemonDataService(mockLogger.Object, mockUserInventoryRepo.Object, mockPokemonDataRepo.Object);

            //Act
            var result = await sut.GetUsersPokemonAsync();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersPokemonAsync_ShouldReturnEmptyListIfErrorThrownByPokemonDataRepo()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<PokemonDataService>>();
            var mockUserInventoryRepo = new Mock<IUserInventoryRepository>();
            var mockPokemonDataRepo = new Mock<IPokemonDataRepository>();

            mockPokemonDataRepo
                .Setup(x => x.GetPokemonByNamesAsync(It.IsAny<string[]>()))
                .Throws(new Exception("error occured"));

            var sut = new PokemonDataService(mockLogger.Object, mockUserInventoryRepo.Object, mockPokemonDataRepo.Object);

            //Act
            var result = await sut.GetUsersPokemonAsync();

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetUsersPokemonAsync_ShouldReturnEmptyListIfInventoryEmpty()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<PokemonDataService>>();
            var mockUserInventoryRepo = new Mock<IUserInventoryRepository>();
            var mockPokemonDataRepo = new Mock<IPokemonDataRepository>();

            mockUserInventoryRepo
                .Setup(x => x.GetAll())
                .Returns(new string[] { });

            mockPokemonDataRepo
                .Setup(x => x.GetPokemonByNamesAsync(It.IsAny<string[]>()))
                .ReturnsAsync(It.IsAny<List<Pokemon>>());

            var sut = new PokemonDataService(mockLogger.Object, mockUserInventoryRepo.Object, mockPokemonDataRepo.Object);

            //Act
            var result = await sut.GetUsersPokemonAsync();

            //Assert
            Assert.Empty(result);
        }
    }
}