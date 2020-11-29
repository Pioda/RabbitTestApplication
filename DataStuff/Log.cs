using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStuff
{
    public class Log
    {
        public Log(string message)
        {
            MessageTime = DateTime.Now;
            Message = message;
        }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string _id { get; set; }
        public DateTime MessageTime { get; set; }
        public string Message { get; set; }
    }
}
