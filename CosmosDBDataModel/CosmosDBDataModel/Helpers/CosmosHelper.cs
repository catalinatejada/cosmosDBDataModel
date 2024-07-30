using Azure.Identity;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using CosmosDBDataModel.Config;
using IDataObject = CosmosDBDataModel.Model.IDataObject;



namespace CosmosDBDataModel.Helpers
{
    public class CosmosHelpers
    {

        public static async Task<ItemResponse<T>> InsertItem<T>(string databaseName, string containerName, T Element)
        {
            using (CosmosClient client = new(
                    accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                    authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
            ))
            {
                var db = client.GetDatabase(databaseName);
                var container = db.GetContainer(containerName);
                try
                {
                    var response = await container.CreateItemAsync<T>(Element);
                    return response;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public static async Task<TransactionalBatchOperationResult<T>> InsertTransacctionalBatch<T>(string databaseName, string containerName, List<T> Elements) where T : IDataObject
        {
            using CosmosClient client = new(
                    accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                    authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
            );

            var db = client.GetDatabase(databaseName);
            var container = db.GetContainer(containerName);
            PartitionKey partitionKey = new PartitionKey(Elements[0].partitionKey);
            TransactionalBatch batch = container.CreateTransactionalBatch(partitionKey);
            foreach (var item in Elements)
                batch.CreateItem<T>(item);
            using TransactionalBatchResponse response = await batch.ExecuteAsync();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Inserted");
                TransactionalBatchOperationResult<T> productResponse;
                return response.GetOperationResultAtIndex<T>(0);
            }
            else
            {
                Console.WriteLine($"ERROR: {response.StatusCode} {response.ErrorMessage}");
            }

            return null;
        }

        public static async Task CreateContainer(string databaseName, string containerName, string partitionKeyPath)
        {
            using CosmosClient client = new(
                    accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                    authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
            );

            var db = client.GetDatabase(databaseName);
            var containercreated = await db.CreateContainerAsync(
                                            id: containerName,
                                            partitionKeyPath: partitionKeyPath);
        }

        public static async Task DeleteContainer(string databaseName, string containerName)
        {
            using CosmosClient client = new(
                     accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                     authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
             );
            var db = client.GetDatabase(databaseName);
            var container = db.GetContainer(containerName);

            await container.DeleteContainerAsync();

        }
        public static async Task<CosmosTestRun<T>> QueryItems<T>(string databaseName, string containerName, QueryDefinition query)
        {
            List<T> collection = new List<T>();
            double consumedRUs = 0;
            using CosmosClient client = new(
                    accountEndpoint: Environment.GetEnvironmentVariable("COSMOS_ENDPOINT")!,
                    authKeyOrResourceToken: Environment.GetEnvironmentVariable("COSMOS_KEY")!
            );

            var db = client.GetDatabase(databaseName);
            var container = db.GetContainer(containerName);
            using FeedIterator<T> feed = container.GetItemQueryIterator<T>(query);

            while (feed.HasMoreResults)
            {
                try
                {
                    FeedResponse<T> response = await feed.ReadNextAsync();
                    consumedRUs += response.RequestCharge;
                    foreach (T item in response)
                    {
                        collection.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return CosmosTestRun<T>.SaveInTestRun(collection, consumedRUs);
        }


    }

}

