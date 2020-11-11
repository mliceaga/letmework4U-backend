using Infrastructure.CosmosDbData.Interfaces;
using System.Threading.Tasks;

namespace Core.Interfaces.Persistence
{
    public interface ICosmosDbContainerFactory
    {
        /// <summary>
        ///     Returns a CosmosDbContainer wrapper
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        ICosmosDbContainer GetContainer(string containerName);

        /// <summary>
        ///     Ensure the database is created
        /// </summary>
        /// <returns></returns>
        Task EnsureDbSetupAsync();
    }
}
