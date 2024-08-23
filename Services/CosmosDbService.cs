using Microsoft.Azure.Cosmos;

public class CosmosDbService
{
    private readonly Container _container;

    public CosmosDbService(CosmosClient cosmosClient, string databaseName, string containerName)
    {
        _container = cosmosClient.GetContainer(databaseName, containerName);
    }

    public async Task<IEnumerable<HardwareItem>> GetHardwareItemsAsync()
    {
        var query = _container.GetItemQueryIterator<HardwareItem>("SELECT * FROM c");
        List<HardwareItem> results = new List<HardwareItem>();
        
        while (query.HasMoreResults)
        {
            var response = await query.ReadNextAsync();
            results.AddRange(response.Resource);
        }
        
        return results;
    }
}
