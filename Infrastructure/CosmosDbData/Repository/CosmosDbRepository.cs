using Ardalis.Specification;
using Core.Entities.Base;
using Core.Interfaces;
using Core.Interfaces.Persistence;
using Core.Specifications.Base;
using Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Infrastructure.CosmosDbData.Repository
{
    public abstract class CosmosDbRepository<T> : IRepository<T>, IContainerContext<T> where T : BaseEntity
    {
        /// <summary>
        ///     Name of the CosmosDB container
        /// </summary>
        public abstract string ContainerName { get; }

        /// <summary>
        ///     Generate id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract string GenerateId(T entity);

        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly Microsoft.Azure.Cosmos.Container _container;
        public CosmosDbRepository(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        public async Task<string> AddItemAsync(T item)
        {
            item.Id = GenerateId(item);
            await _container.CreateItemAsync<T>(item, null);
            return item.Id;
        }

        //public async Task<T> AddOrUpdateAsync(T item, RequestOptions requestOptions = null)
        //{
        //    T upsertedEntity;

        //    PartitionKey partitionKey;
        //    requestOptions.Properties.TryGetValue("PartitionKey", out partitionKey);

        //    var upsertedDoc = await _container.UpsertItemAsync(item, );
        //    upsertedEntity = JsonConvert.DeserializeObject<T>(upsertedDoc.Resource.ToString());

        //    return upsertedEntity;
        //}

        public async Task DeleteItemAsync(Guid id)
        {
            await this._container.DeleteItemAsync<T>(id.ToString(), PartitionKey.None);
        }

        public async Task<T> GetItemAsync(Guid id)
        {
            try
            {
                ItemResponse<T> response = await _container.ReadItemAsync<T>(id.ToString(), PartitionKey.None);
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // Search data using SQL query string
        // This shows how to use SQL string to read data from Cosmos DB for demonstration purpose.
        // For production, try to use safer alternatives like Parameterized Query and LINQ if possible.
        // Using string can expose SQL Injection vulnerability, e.g. select * from c where c.id=1 OR 1=1. 
        // String can also be hard to work with due to special characters and spaces when advanced querying like search and pagination is required.
        public async Task<IEnumerable<T>> GetItemsAsync(string queryString)
        {
            var resultSetIterator = _container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (resultSetIterator.HasMoreResults)
            {
                var response = await resultSetIterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        /// <inheritdoc cref="IRepository{T}.GetItemsAsync(Ardalis.Specification.ISpecification{T})"/>
        public async Task<IEnumerable<T>> GetItemsAsync(ISpecification<T> specification)
        {
            var queryable = ApplySpecification(specification);
            var iterator = queryable.ToFeedIterator<T>();

            List<T> results = new List<T>();
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        /// <summary>
        ///     Evaluate specification and return IQueryable
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            var evaluator = new CosmosDbSpecificationEvaluator<T>();
            return evaluator.GetQuery(_container.GetItemLinqQueryable<T>(), specification);
        }

        public async Task UpdateItemAsync(Guid id, T item)
        {
            await this._container.UpsertItemAsync<T>(item, null);
        }


    }
}
