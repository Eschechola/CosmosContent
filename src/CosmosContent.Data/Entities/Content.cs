using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CosmosContent.Data.Entities
{
    public class Content
    {
        [BsonElement("id")]
        public string Id { get; private set; }

        [BsonElement("briefing")]
        public object Briefing { get; private set; }

        public Content(object briefing)
        {
            Id = Guid.NewGuid().ToString();
            Briefing = briefing;
        }

        public void UpdateBriefing(object briefing)
        {
            if (briefing != null)
                Briefing = briefing;
        }

        public string GetBriefingProperty(string propertyName)
        {
            var serializedBriefing = JsonConvert.SerializeObject(Briefing);
            var briefingDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(serializedBriefing);

            return briefingDictionary.Where(x => x.Key.ToLower() == propertyName.ToLower())
                .FirstOrDefault()
                .Value;
        }
    }
}
