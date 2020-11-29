using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStuff
{
    public class Ressource
    {
        public Ressource(string name, int price)
        {
            Name = name;
            Price = price;
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id { get; protected set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
