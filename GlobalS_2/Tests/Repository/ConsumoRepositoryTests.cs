using GlobalS_2.Models;
using GlobalS_2.Repository;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;
using Xunit;

namespace GlobalS_2.Tests.Repository
{
    public class ConsumoRepositoryTests
    {
        private readonly Mock<IConnectionMultiplexer> _mockRedis;
        private readonly Mock<IDatabase> _mockDatabase;
        private readonly Mock<IMongoCollection<Consumo>> _mockMongoCollection;
        private readonly ConsumoRepository _repository;

        public ConsumoRepositoryTests()
        {
            _mockRedis = new Mock<IConnectionMultiplexer>();
            _mockDatabase = new Mock<IDatabase>();
            _mockMongoCollection = new Mock<IMongoCollection<Consumo>>();

            _mockRedis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_mockDatabase.Object);

            var mockMongoDatabase = new Mock<IMongoDatabase>();
            mockMongoDatabase.Setup(m => m.GetCollection<Consumo>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                             .Returns(_mockMongoCollection.Object);

            _repository = new ConsumoRepository(mockMongoDatabase.Object, _mockRedis.Object);
        }

        [Fact]
        public async Task AddConsumoAsync_ShouldReturnConsumo_WhenValidDataIsProvided()
        {
            var consumo = new Consumo
            {
                Data = "2024-11-21",
                Valor = 150.5
            };

            _mockMongoCollection.Setup(m => m.InsertOneAsync(consumo, null, default)).Returns(Task.CompletedTask);

            var result = await _repository.AddConsumoAsync(consumo);

            object value = Assert.Equals(consumo.Data, result.Data);
            Assert.ReferenceEquals(consumo.Valor, result.Valor);
        }

        [Fact]
        public async Task GetConsumosAsync_ShouldReturnCachedData_WhenCacheIsAvailable()
        {
            var cachedData = "[{\"Id\": \"1\", \"Data\": \"2024-11-21\", \"Valor\": 150.5}]";
            _mockDatabase.Setup(d => d.StringGetAsync(It.IsAny<string>(), It.IsAny<CommandFlags>())).ReturnsAsync(cachedData);

            var result = await _repository.GetConsumosAsync(0, 10);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("2024-11-21", result[0].Data);
            Assert.Equal(150.5, result[0].Valor);
        }

        [Fact]
        public async Task GetConsumosAsync_ShouldReturnMongoData_WhenCacheIsEmpty()
        {
            var consumosMongo = new List<Consumo>
            {
                new Consumo { Data = "2024-11-21", Valor = 150.5 }
            };

            _mockDatabase.Setup(d => d.StringGetAsync(It.IsAny<string>(), It.IsAny<CommandFlags>())).ReturnsAsync((string)null);
            _mockMongoCollection.Setup(m => m.Find(It.IsAny<FilterDefinition<Consumo>>(), It.IsAny<FineOptions>())
                                          .ToListAsync()).ReturnsAsync(consumosMongo);

            var result = await _repository.GetConsumosAsync(0, 10);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("2024-11-21", result[0].Data);
            Assert.Equal(150.5, result[0].Valor);
        }
    }
}

