using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DataStuff
{
    public class Repository
    {
        private readonly IMongoCollection<Ressource> Ressources;
        private readonly IMongoCollection<Log> Logs;

        public Repository()
        {
#if DEBUG
            var client = new MongoClient("mongodb://admin:Sup3rP455W0RD@localhost:27017");
#else
            var client = new MongoClient("mongodb://admin:Sup3rP455W0RD@mongo:27017");
#endif
            var database = client.GetDatabase("MarketDb");

            Ressources = database.GetCollection<Ressource>("Ressources");
            Logs = database.GetCollection<Log>("Logs");
        }

        public List<Ressource> GetAll()
        {
            return Ressources.Find(r => true).ToList();
        }

        public Ressource Get(string name)
        {
            return Ressources.Find(r => r.Name == name).FirstOrDefault();
        }

        public void Insert(Ressource r)
        {
            Ressources.InsertOne(r);
        }

        public void Insert(Log l)
        {
            Logs.InsertOne(l);
        }

        public void RemoveRessource(string name)
        {
            Ressources.DeleteOne(r => r.Name == name);
        }
    }
}
