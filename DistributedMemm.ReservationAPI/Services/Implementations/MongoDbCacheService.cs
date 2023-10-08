using DistributedMemm.ReservationAPI.Services.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace DistributedMemm.ReservationAPI.Services.Implementations
{
    public class MongoDbCacheService : ICacheService
    {
        private readonly IMongoCollection<KeyValuePair> _collection;

        public MongoDbCacheService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<KeyValuePair>(collectionName);
        }

        public async Task SaveToCacheKeyValueAsync(string key, object value)
        {
            var pair = new KeyValuePair
            {
                Key = key,
                Value = JsonSerializer.Serialize(value)
            };

            await _collection.InsertOneAsync(pair);
        }

        public async Task<List<KeyValuePair>> GetKeyValuesAsync(int page, int pageSize)
        {
            return await _collection.Find(_ => true)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }
    }

    public class KeyValuePair
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
    }
}