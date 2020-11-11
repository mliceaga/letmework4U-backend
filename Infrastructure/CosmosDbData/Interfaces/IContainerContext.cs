using Core.Entities.Base;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.CosmosDbData.Interfaces
{
    /// <summary>
    ///  Defines the container level context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainerContext<T> where T : BaseEntity
    {
        string ContainerName { get; }
        string GenerateId(T entity);
    }
}
