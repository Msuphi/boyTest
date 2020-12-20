using System.Threading.Tasks;
using MongoDB.Driver;

namespace Persistence.MongoDB
{
    public interface IMongoDbSeeder
    {
        Task SeedAsync(IMongoDatabase database);
    }
}