using GlobalS_2.Models;
using MongoDB.Driver;
using StackExchange.Redis;

namespace GlobalS_2.Repository
{
    public class ConsumoRepository : IConsumoRepository
    {
        private readonly IMongoCollection<Consumo> _consumos;

        public ConsumoRepository()
        {
        }

        public ConsumoRepository(IMongoDatabase database, IConnectionMultiplexer @object)
        {
            _consumos = database.GetCollection<Consumo>("Consumos");
        }

        // Adiciona um novo consumo no MongoDB
        public async Task<Consumo> AddConsumoAsync(Consumo consumo)
        {
            await _consumos.InsertOneAsync(consumo); // Insere o consumo no MongoDB
            return consumo; // Retorna o consumo adicionado
        }

        // Recupera todos os consumos do MongoDB
        public async Task<List<Consumo>> GetConsumosAsync()
        {
            return await _consumos.Find(consumo => true).ToListAsync(); // Retorna todos os consumos
        }

        public Task<List<Consumo>> GetConsumosAsync(int skip, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IConnectionMultiplexer Redis => throw new NotImplementedException();
    }
}

