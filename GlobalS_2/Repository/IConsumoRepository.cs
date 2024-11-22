using GlobalS_2.Models;
using StackExchange.Redis;

namespace GlobalS_2.Repository
{
    public interface IConsumoRepository
    {
        IConnectionMultiplexer Redis { get; }

        Task<Consumo> AddConsumoAsync(Consumo consumo);
        Task<List<Consumo>> GetConsumosAsync(int skip, int pageSize);
    }
}

