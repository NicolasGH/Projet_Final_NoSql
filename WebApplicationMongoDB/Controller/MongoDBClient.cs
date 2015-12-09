using System;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Web.Script.Serialization;
using System.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
                while ((line = streamReader.ReadLine()) != null)
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
        public Dictionary<string, int> mapReduceMarketCapCountry()
        {
            Dictionary<string, int> list = new Dictionary<string, int>();
            List<BsonDocument> lis2 = new List<BsonDocument>();
            string map = @"function(){emit(this.Country, this['Market Cap']);}";
            string reduce = @"function(key, values){ return Array.sum(values);}";
            var results = collection.MapReduceAsync<BsonDocument>(map, reduce).Result.ForEachAsync(document => list.Add(document.GetValue("_id").ToString(), Convert.ToInt32(document.GetValue("value"))));

            return list;
        }

        public Dictionary<string, int> mapReduceIndustryCountry(string _country)
        {
            Dictionary<string, int> list = new Dictionary<string, int>();
            List<BsonDocument> lis2 = new List<BsonDocument>();
            string map = @"function(){if (this.Country == '"+_country+"') emit(this.Industry, 1);}";
            string reduce = @"function(key, values){return Array.sum(values);}";
            var results = collection.MapReduceAsync<BsonDocument>(map, reduce).Result.ForEachAsync(document => list.Add(document.GetValue("_id").ToString(), Convert.ToInt32(document.GetValue("value"))));
            return list;
        }

        public void aggregateCountrySorted(string _country, string _sortBy, bool _asc)
        {
            int i = 0;
            if (_asc) i = 1;
            else i = -1;
            List<Stockobject> list = new List<Stockobject>();
            List<BsonDocument> list2 = new List<BsonDocument>();

            var aggregate = collection.Aggregate()
                                      .Match(new BsonDocument { { "Country", "France" } })
                                      .Sort(new BsonDocument { { _sortBy, 1 } });


            var jsonSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            System.Diagnostics.Debug.WriteLine(aggregate.ToJson());// (document => list2.Add(document));
             //System.Diagnostics.Debug.WriteLine(results);

         //   return Task;
            //int i = 0;
            //if (_asc) i = 1;
            //else i = -1;
            //List<Stockobject> list = new List<Stockobject>();
            //List<BsonDocument> list2 = new List<BsonDocument>();

            //var aggregate = collection.Aggregate()
            //                          .Match(new BsonDocument { { "Country", "France" } })
            //                          .Sort(new BsonDocument { { _sortBy, 1 } });

            //var jsonSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict };
            //System.Diagnostics.Debug.WriteLine("Here1");

            //var results = await aggregate.ToListAsync<Stockobject>(;
            //System.Diagnostics.Debug.WriteLine("Here2");
            //document => list.Add(BsonSerializer.Deserialize<Stockobject>(document.ToJson(jsonSettings)))

        }

    }
}