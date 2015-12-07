using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.IO;
using MongoDB.Bson;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace WebApplicationMongoDB.Controller
{
    public class ElasticsearchClient
    {
        private Uri connexion;
        private ConnectionSettings settings;
        private ElasticClient client;
        public ElasticsearchClient()
        {
            this.connexion = new Uri("http://172.21.153.221:9200");
             this.settings = new ConnectionSettings(connexion, defaultIndex: "stockexchange");
            this.client = new ElasticClient(settings);
            client.CreateIndex("ste", s => s.AddMapping<Stockobject>(m => m.MapFromAttributes()));
        }

        public void Indexing(Stockobject obj)
        {
            try
            {
                var index = client.Index<Stockobject>(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public List<Stockobject> findAll()
        {
              try
              {
                var searchResults = client.Search<Stockobject>(s => s
                    .AllIndices()
                    //.AllTypes()
                    .Size(10)
                    );
                return searchResults.Documents.ToList();
            }
            catch (JsonSerializationException e)
            {
                Debug.WriteLine(e.ToString());
                return null;
            }

        }

        public void LoadLocalData(String _path)
        {
            try
            {
                StreamReader file = new StreamReader(_path);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                while (file.ReadLine() != null)
                {
                    try
                    {
                        string line1 = file.ReadLine();
                        string line2 = file.ReadLine();
                        string str = line1 + line2;
                        JObject json = JObject.Parse(str);
                        Stockobject obj = JsonConvert.DeserializeObject<Stockobject>(str);
                        Indexing(obj);

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
                Console.ReadKey();
            }
        }


        public List<Stockobject> NavBar(String txt)
        {
            try
            {
                var searchResults =
            client.Search<Stockobject>(s => s.QueryString("{\"query\": {\"match\": { \"Sector\" : \"Technology\" }}}"));
                //string url_req = "http://localhost:9200/stockexchang/exchange/_search?pretty";
                //var _connection = new ElasticConnection("localhost", 9200);
                //string command = new SearchCommand("stockexchange", "exchange").WithParameter("exchange", "{\"query\": {\"match\": { \"Sector\" : \"Technology\" }}}")
                //                    .WithParameter("size", "100");
                //string result = _connection.Post(command, "{\"query\": {\"match\": { \"Sector\" : \"Technology\" }}}");
                //using (var client = new WebClient())
                //{
                //    Console.Write(client.UploadString(url_req, "POST", "{\"query\": {\"match\": { \"Sector\" : \"Technology\" }}}"));
                //}




                //var searchResults = client.Search(s => s
                //    .Size(10)
                //    .From(0)
                //    .Query(q=>q
                //    .Term(p=>p.Sector, "Technology")));

                //Console.WriteLine("aaa");
                foreach (Stockobject obj in searchResults.Documents.ToList())
                {
                    Console.WriteLine(obj.VolatilityWeek);

                }
                Console.ReadKey();
                return null;
                //return searchResults.Documents.ToList();
                // return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }



        [JsonObject(MemberSerialization.OptIn)]
        public class Stockobject
        {
            [JsonProperty("EPS growth next 5 year")]
            public float EPSgrowthnext5years { get; set; }
            [JsonProperty("Company")]
            public string Company { get; set; }
            [JsonProperty("Current Ratio")]
            public int CurrentRatio { get; set; }
            [JsonProperty("Payout Ratio")]
            public float PayoutRatio { get; set; }
            [JsonProperty("Operating Margin")]
            public float OperatingMargin { get; set; }
            [JsonProperty("Shares Float")]
            public float SharesFloat { get; set; }
            [JsonProperty("P/Cash")]
            public float PCash { get; set; }
            [JsonProperty("Institutional Transactions")]
            public float InstitutionalTransactions { get; set; }
            [JsonProperty("Average True Range")]
            public float AverageTrueRange { get; set; }
            [JsonProperty("Performance (Half Year)")]
            public float PerformanceHalfYear { get; set; }
            [JsonProperty("Beta")]
            public float Beta { get; set; }
            [JsonProperty("Industry")]
            public string Industry { get; set; }
            [JsonProperty("Average Volume")]
            public float AverageVolume { get; set; }
            [JsonProperty("Performance (Year)")]
            public float PerformanceYear { get; set; }
            [JsonProperty("200-Day Simple Moving Average")]
            public float _200DaySimpleMovingAverage { get; set; }
            [JsonProperty("Total Debt/Equity")]
            public float TotalDebtEquity { get; set; }
            [JsonProperty("EPS (ttm)")]
            public float EPSttm { get; set; }
            [JsonProperty("Forward P/E")]
            public float ForwardPE { get; set; }
            [JsonProperty("Volatility (Week)")]
            public float VolatilityWeek { get; set; }
            [JsonProperty("Change from Open")]
            public float ChangefromOpen { get; set; }
            [JsonProperty("Sales growth quarter over quarter")]
            public float Salesgrowthquarteroverquarter { get; set; }
            [JsonProperty("Short Ratio")]
            public float ShortRatio { get; set; }
            [JsonProperty("Price")]
            public float Price { get; set; }
            [JsonProperty("Volume")]
            public int Volume { get; set; }
            [JsonProperty("Relative Volume")]
            public float RelativeVolume { get; set; }
            [JsonProperty("20-Day Simple Moving Average")]
            public float _20DaySimpleMovingAverage { get; set; }
            [JsonProperty("Gap")]
            public int Gap { get; set; }
            [JsonProperty("Country")]
            public string Country { get; set; }
            [JsonProperty("52-Week Low")]
            public float _52WeekLow { get; set; }
            [JsonProperty("_id")]
            public int _id { get; set; }
            [JsonProperty("Market Cap")]
            public float MarketCap { get; set; }
            [JsonProperty("EPS growth past 5 years")]
            public float EPSgrowthpast5years { get; set; }
            [JsonProperty("50-Day Simple Moving Average")]
            public float _50DaySimpleMovingAverage { get; set; }
            [JsonProperty("Quick Ratio")]
            public float QuickRatio { get; set; }
            [JsonProperty("P/B")]
            public float PB { get; set; }
            [JsonProperty("EPS growth next year")]
            public float EPSgrowthnextyear { get; set; }
            [JsonProperty("Profit Margin")]
            public float ProfitMargin { get; set; }
            [JsonProperty("Performance (YTD)")]
            public float PerformanceYTD { get; set; }
            [JsonProperty("Sales growth past 5 years")]
            public float Salesgrowthpast5years { get; set; }
            [JsonProperty("P/E")]
            public float PE { get; set; }
            [JsonProperty("50-Day High")]
            public float _50DayHigh { get; set; }
            [JsonProperty("Relative Strength Index (14)")]
            public float RelativeStrengthIndex14 { get; set; }
            [JsonProperty("Float Short")]
            public float FloatShort { get; set; }
            [JsonProperty("Analyst Recom")]
            public float AnalystRecom { get; set; }
            [JsonProperty("Return on Investment")]
            public float ReturnonInvestment { get; set; }
            [JsonProperty("Return on Assets")]
            public float ReturnonAssets { get; set; }
            [JsonProperty("Shares Outstanding")]
            public int SharesOutstanding { get; set; }
            [JsonProperty("Volatility (Month)")]
            public float VolatilityMonth { get; set; }
            [JsonProperty("Performance (Week)")]
            public float PerformanceWeek { get; set; }
            [JsonProperty("P/S")]
            public float PS { get; set; }
            [JsonProperty("Gross Margin")]
            public float GrossMargin { get; set; }
            [JsonProperty("P/Free Cash Flow")]
            public float PFreeCashFlow { get; set; }
            [JsonProperty("LT Debt/Equity")]
            public float LTDebtEquity { get; set; }
            [JsonProperty("52-Week High")]
            public float _52WeekHigh { get; set; }
            [JsonProperty("50-Day Low")]
            public float _50DayLow { get; set; }
            [JsonProperty("Earnings Date")]
            public EarningsDate EarningsDate { get; set; }
            [JsonProperty("Sector")]
            public string Sector { get; set; }
            [JsonProperty("Ticker")]
            public string Ticker { get; set; }
            [JsonProperty("Institutional Ownership")]
            public float InstitutionalOwnership { get; set; }
            [JsonProperty("EPS growth this year")]
            public float EPSgrowththisyear { get; set; }
            [JsonProperty("EPS growth quarter over quarter")]
            public float EPSgrowthquarteroverquarter { get; set; }
            [JsonProperty("Return on Equity")]
            public float ReturnonEquity { get; set; }
            [JsonProperty("Change")]
            public float Change { get; set; }
            [JsonProperty("Performance (Month)")]
            public float PerformanceMonth { get; set; }
            [JsonProperty("PEG")]
            public float PEG { get; set; }
            [JsonProperty("Dividend Yield")]
            public float DividendYield { get; set; }
            [JsonProperty("Insider Ownership")]
            public float InsiderOwnership { get; set; }
            [JsonProperty("Performance (Quarter)")]
            public float PerformanceQuarter { get; set; }
            [JsonProperty("Insider Transactions")]
            public float InsiderTransactions { get; set; }
        }

        public class EarningsDate
        {
            [JsonProperty("$date")]
            public long date { get; set; }
        }



    }
}