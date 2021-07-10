using CosmosContent.Data.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace CosmosContent.Data.Interfaces
{
    public interface ICosmosService
    {
        void Create(Content content);
        void Update(Content content);
        void Remove(Content content);
        Content Get(string id);
        IList<Content> GetAll();
        IList<Content> Search(FilterDefinition<Content> filter);
    }
}
