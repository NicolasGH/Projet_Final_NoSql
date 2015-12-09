using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Web.Script.Serialization;
using System.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using System.Collections.Generic;

namespace WebApplicationMongoDB.Controller
{
    public class MongoDBClient
    {
        protected IMongoClient client;
        protected string connectionString;
        protected IMongoDatabase database;
        protected IMongoCollection<BsonDocument> collection;

        public MongoDBClient(string _collection)
        {
            this.connectionString = "mongodb://groupeNoSql3:groupeNoSql3@ds055564.mongolab.com:55564/stockexchange";
            //mongo ds055564.mongolab.com:55564/stockexchange -u groupeNoSql3 -p groupeNoSql3
            client = new MongoClient(this.connectionString);
            database = client.GetDatabase("stockexchange");
            this.collection = database.GetCollection<BsonDocument>(_collection);
            if (this.collection == null) database.CreateCollectionAsync(_collection);

        }

        public void insertData(Stockobject obj)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                string str = ser.Serialize(obj);
                BsonDocument bson = BsonDocument.Parse(str);
                collection.InsertOneAsync(bson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public void updateData(Stockobject obj, string _collection)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var collection = database.GetCollection<BsonDocument>(_collection);
                var filter = Builders<BsonDocument>.Filter.Eq("Ticker", obj.Ticker);
                string str = ser.Serialize(obj);
                BsonDocument bson = BsonDocument.Parse(str);
                collection.UpdateOneAsync(filter, str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void deleteData(Stockobject obj)
        {
            try
            {
                var collection = database.GetCollection<BsonDocument>("stockCollection");
                var filter = Builders<BsonDocument>.Filter.Eq("Ticker", obj.Ticker);
                collection.DeleteOneAsync(filter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }



        public void LoadLocalData(string _path)
        {
            using (var streamReader = new StreamReader(_path))
            {
                string line;
                while ((line =  streamReader.ReadLine()) != null)
                {
                    using (var jsonReader = new JsonReader(line))
                    {
                        var context = BsonDeserializationContext.CreateRoot(jsonReader);
                        var document = collection.DocumentSerializer.Deserialize(context);
                        collection.InsertOneAsync(document).Wait();
                    }
                }
            }
        }

        public List<Stockobject> mapReduceMarketCapCountry()
        {
            List<Stockobject> list = new List<Stockobject>();
            //string map = @"function(){emit(this.Country, this['Market Cap'])}";
            //string reduce = @"function(key, values){ return Array.sum(values)}";
            //var result = collection.MapReduceAsync<BsonDocument>(map, reduce);
            //foreach (var reslt in result.ToJson())
            //{
            //    System.Diagnostics.Debug.WriteLine(result.ToJson());
            //}
            var documents = collection.Find(new BsonDocument()).ToListAsync();
            collection.Find(new BsonDocument()).ForEachAsync(d => System.Diagnostics.Debug.WriteLine(d));
         
            return list;
        }



    }
}