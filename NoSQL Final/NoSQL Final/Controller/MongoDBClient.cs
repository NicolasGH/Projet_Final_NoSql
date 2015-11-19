using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.IO;

namespace NoSQL_Final
{
    class MongoDBClient
    {
        protected static IMongoClient _client;
        protected string connectionString;
        protected static IMongoDatabase _database;

        public MongoDBClient()
        {
            this.connectionString = "mongodb://groupeNoSql3:groupeNoSql3@ds055564.mongolab.com:55564/stockexchange";
            _client = new MongoClient(this.connectionString);
            _database = _client.GetDatabase("stockexchange");
        }

        public void insertData(Stockobject obj)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var collection = _database.GetCollection<BsonDocument>("stockCollection");
                string str = ser.Serialize(obj);
                BsonDocument bson = BsonDocument.Parse(str);
                collection.InsertOneAsync(bson);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        public void updateData(Stockobject obj)
        {
            try
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var collection = _database.GetCollection<BsonDocument>("stockCollection");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", obj._id);
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
                var collection = _database.GetCollection<BsonDocument>("stockCollection");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", obj._id);
                collection.DeleteOneAsync(filter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void LoadLocalData()
        {
            try
            {
                StreamReader file = new StreamReader("stocks.json");
                var collection = _database.GetCollection<BsonDocument>("stockCollection");
                while (file.ReadLine() != null)
                {
                    try
                    {
                        string line = file.ReadLine();
                        BsonDocument bson = BsonDocument.Parse(line);
                        collection.InsertOneAsync(bson);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }



        public class Stockobject
        {
            public _Id _id { get; set; }
            public string Ticker { get; set; }
            public float ProfitMargin { get; set; }
            public float InstitutionalOwnership { get; set; }
            public float EPSgrowthpast5years { get; set; }
            public float TotalDebtEquity { get; set; }
            public float CurrentRatio { get; set; }
            public float ReturnonAssets { get; set; }
            public string Sector { get; set; }
            public float PS { get; set; }
            public float ChangefromOpen { get; set; }
            public float PerformanceYTD { get; set; }
            public float PerformanceWeek { get; set; }
            public float QuickRatio { get; set; }
            public float InsiderTransactions { get; set; }
            public float PB { get; set; }
            public float EPSgrowthquarteroverquarter { get; set; }
            public float PayoutRatio { get; set; }
            public float PerformanceQuarter { get; set; }
            public float ForwardPE { get; set; }
            public float PE { get; set; }
            public float _200DaySimpleMovingAverage { get; set; }
            public float SharesOutstanding { get; set; }
            public EarningsDate EarningsDate { get; set; }
            public float _52WeekHigh { get; set; }
            public float PCash { get; set; }
            public float Change { get; set; }
            public float AnalystRecom { get; set; }
            public float VolatilityWeek { get; set; }
            public string Country { get; set; }
            public float ReturnonEquity { get; set; }
            public float _50DayLow { get; set; }
            public float Price { get; set; }
            public float _50DayHigh { get; set; }
            public float ReturnonInvestment { get; set; }
            public float SharesFloat { get; set; }
            public float DividendYield { get; set; }
            public float EPSgrowthnext5years { get; set; }
            public string Industry { get; set; }
            public float Beta { get; set; }
            public float Salesgrowthquarteroverquarter { get; set; }
            public float OperatingMargin { get; set; }
            public float EPSttm { get; set; }
            public float PEG { get; set; }
            public float FloatShort { get; set; }
            public float _52WeekLow { get; set; }
            public float AverageTrueRange { get; set; }
            public float EPSgrowthnextyear { get; set; }
            public float Salesgrowthpast5years { get; set; }
            public string Company { get; set; }
            public float Gap { get; set; }
            public float RelativeVolume { get; set; }
            public float VolatilityMonth { get; set; }
            public float MarketCap { get; set; }
            public float Volume { get; set; }
            public float GrossMargin { get; set; }
            public float ShortRatio { get; set; }
            public float PerformanceHalfYear { get; set; }
            public float RelativeStrengthIndex14 { get; set; }
            public float InsiderOwnership { get; set; }
            public float _20DaySimpleMovingAverage { get; set; }
            public float PerformanceMonth { get; set; }
            public float PFreeCashFlow { get; set; }
            public float InstitutionalTransactions { get; set; }
            public float PerformanceYear { get; set; }
            public float LTDebtEquity { get; set; }
            public float AverageVolume { get; set; }
            public float EPSgrowththisyear { get; set; }
            public float _50DaySimpleMovingAverage { get; set; }
        }

        public class _Id
        {
            public string oid { get; set; }
        }

        public class EarningsDate
        {
            public long date { get; set; }
        }


    }

}
