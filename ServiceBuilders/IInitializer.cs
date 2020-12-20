using System.Threading.Tasks;

namespace ServiceBuilders
{
    public interface IInitializer 
    {
        Task InitializeAsync();
    }
}