using DistributedMemm.ReservationAPI.Services.Interfaces;
using MongoDB.Driver;

namespace DistributedMemm.ReservationAPI.Services.Implementations;

public class MongoDbCacheService : ICacheService
{
    private readonly IMongoCollection<KeyValuePair> _collection;

    public MongoDbCacheService(string connectionString, string databaseName, string collectionName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<KeyValuePair>(collectionName);
    }

    public async Task SaveKeyValueAsync(string key, string value)
    {
        var pair = new KeyValuePair
        {
            Key = key,
            Value = value
        };

        await _collection.InsertOneAsync(pair);
    }
}

public class KeyValuePair
{
    public string Key { get; set; }
    public string Value { get; set; }
}