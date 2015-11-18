using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace NoSQL_Final
{
    class MongoDBClient
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;

        public MongoDBClient()
        {
            Console.WriteLine("Nicolas");
        }

        public string InsertData(string uri, Stockobject obj)
        {
            var connectingString = uri;

            try
            {
                var _client = new MongoClient(connectingString);
                _database = _client.GetDatabase("stockExchange");
                var collection = _database.GetCollection<Stockobject>("stockCollection");
                collection.InsertOneAsync(obj);
                return "element ajouté";
            }
            catch(Exception e)
            {
                return e.ToString();
            }
        }

        public string InsertDataFromJson(string path)
        {
            var json = System.IO.File.ReadAllText(path);
        //    var objects = JArray.Parse(json);
            Stockobject obj = new Stockobject();

            return " ";
            
        }




        public class Stockobject
        {
        public _Id _id { get; set; }
        public string Ticker { get; set; }
        public float ProfitMargin { get; set; }
        public float InstitutionalOwnership { get; set; }
        public float EPSgrowthpast5years { get; set; }
        public float TotalDebtEquity { get; set; }
        public int CurrentRatio { get; set; }
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
        public int SharesOutstanding { get; set; }
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
        public int Gap { get; set; }
        public float RelativeVolume { get; set; }
        public float VolatilityMonth { get; set; }
        public float MarketCap { get; set; }
        public int Volume { get; set; }
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
