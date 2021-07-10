using CosmosContent.Data.Entities;
using CosmosContent.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace CosmosContent.Data.Services
{
    public class CosmosService : ICosmosService
    {
        private readonly string _databaseName;
        private readonly string _collectionName;
        private readonly IMongoClient _mongoClient;
        private IMongoDatabase _database { 
            get => _mongoClient.GetDatabase(_databaseName);
        }

        public CosmosService(
            IMongoClient mongoClient,
            IConfiguration configuration)
        {
            _mongoClient = mongoClient;
            _databaseName = configuration["MongoDB:Database"];
            _collectionName = configuration["MongoDB:Collections:Content"];
        }

        public void Create(Content content)
        {
            var collection = GetCollection();
            collection.InsertOne(content);
        }

        public void Update(Content content)
        {
            var collection = GetCollection();

            var filter = Builders<Content>.Filter.Eq("_id", content.Id);
            var update = Builders<Content>.Update
                    .Set(x => x.Briefing, content.Briefing);

            collection.UpdateOne(filter, update);
        }

        public void Remove(Content content)
        {
            var collection = GetCollection();

            var filter = Builders<Content>.Filter.Eq("_id", content.Id);
            collection.DeleteOne(filter);
        }

        public Content Get(string id)
        {
            var collection = GetCollection();

            var filter = Builders<Content>.Filter.Eq("_id", id.ToString());
            return collection.Find(filter).FirstOrDefault();
        }

        public IList<Content> GetAll()
        {
            var collection = GetCollection();

            var filter = new BsonDocument();
            return collection.Find(filter).ToList();
        }

        public IList<Content> Search(FilterDefinition<Content> filter)
        {
            var collection = GetCollection();
            return collection.Find(filter).ToList();
        }

        private IMongoCollection<Content> GetCollection()
            => _database.GetCollection<Content>(_collectionName);
    }
}
