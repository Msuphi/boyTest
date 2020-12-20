using System.Threading.Tasks;
using MongoDB.Driver;

namespace Persistence.MongoDB
{
    public interface IMongoSessionFactory
    {
        Task<IClientSessionHandle> CreateAsync();
    }
}