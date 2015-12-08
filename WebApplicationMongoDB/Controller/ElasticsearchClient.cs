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
using PlainElastic.Net;
using System.Net;

namespace WebApplicationMongoDB.Controller
{
    public class ElasticsearchClient
    {
        private Uri connexion;
        private ConnectionSettings settings;
        private ElasticClient client;
        public ElasticsearchClient()
        {
            this.connexion = new Uri("http://localhost:9200");
            this.settings = new ConnectionSettings(connexion, defaultIndex: "tp");
            this.client = new ElasticClient(settings);
            client.CreateIndex("tp");
            var mapResult = client.Map<Stockobject>(c => c.MapFromAttributes().IgnoreConflicts().Type("stocks").Indices("tp"));
        }

        public void Indexing(Stockobject obj, string idx, string id, string type)
        {
            try
            {
                var index = client.Raw.Index<Stockobject>(idx, type, id, obj);
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
                    .AllTypes()
                    .Size(10)
                    );
                return searchResults.Documents.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public void LoadLocalData(String _path)
        {
            try
            {
                int i = 1;
                StreamReader file = new StreamReader(_path);
                while (file.ReadLine() != null)
                {
                    try
                    {
                        var _connection = new ElasticConnection("localhost", 9200);
                        string command = Commands.Index("stockexchange", "stocks", i.ToString());
                        string line1 = file.ReadLine();
                        string line2 = file.ReadLine();

                        //  string json = line1 + line2;
                        OperationResult response = _connection.Put(command, line2);
                        i++;
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
                var searchResults = client.Search<Stockobject>(s => s
                    .AllIndices()
                    .AllTypes()
                    .Size(10)
                    .Query(qs => qs.QueryString(q => q.Query(txt))));

                return searchResults.Documents.ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }


        [ElasticType(Name = "stocks")]
        [JsonObject(MemberSerialization.OptIn)]
        public class Stockobject
        {
            [ElasticProperty(Name = "EPS growth next 5 year")]
            [JsonProperty("EPS growth next 5 year")]
            public float EPSgrowthnext5years { get; set; }
            [ElasticProperty(Name = "Company")]
            [JsonProperty("Company")]
            public string Company { get; set; }
            [ElasticProperty(Name = "Current Ratio")]
            [JsonProperty("Current Ratio")]
            public float CurrentRatio { get; set; }
            [ElasticProperty(Name = "Payout Ratio")]
            [JsonProperty("Payout Ratio")]
            public float PayoutRatio { get; set; }
            [ElasticProperty(Name = "Operating Margin")]
            [JsonProperty("Operating Margin")]
            public float OperatingMargin { get; set; }
            [ElasticProperty(Name = "Shares Float")]
            [JsonProperty("Shares Float")]
            public float SharesFloat { get; set; }
            [ElasticProperty(Name = "P/Cash")]
            [JsonProperty("P/Cash")]
            public float PCash { get; set; }
            [ElasticProperty(Name = "Institutional Transactions")]
            [JsonProperty("Institutional Transactions")]
            public float InstitutionalTransactions { get; set; }
            [ElasticProperty(Name = "Average True Range")]
            [JsonProperty("Average True Range")]
            public float AverageTrueRange { get; set; }
            [ElasticProperty(Name = "Performance (Half Year)")]
            [JsonProperty("Performance (Half Year)")]
            public float PerformanceHalfYear { get; set; }
            [ElasticProperty(Name = "Beta")]
            [JsonProperty("Beta")]
            public float Beta { get; set; }
            [ElasticProperty(Name = "Industry")]
            [JsonProperty("Industry")]
            public string Industry { get; set; }
            [ElasticProperty(Name = "Average Volume")]
            [JsonProperty("Average Volume")]
            public float AverageVolume { get; set; }
            [ElasticProperty(Name = "Performance (Year)")]
            [JsonProperty("Performance (Year)")]
            public float PerformanceYear { get; set; }
            [ElasticProperty(Name = "200-Day Simple Moving Average")]
            [JsonProperty("200-Day Simple Moving Average")]
            public float _200DaySimpleMovingAverage { get; set; }
            [ElasticProperty(Name = "Total Debt/Equity")]
            [JsonProperty("Total Debt/Equity")]
            public float TotalDebtEquity { get; set; }
            [ElasticProperty(Name = "EPS (ttm)")]
            [JsonProperty("EPS (ttm)")]
            public float EPSttm { get; set; }
            [ElasticProperty(Name = "Forward P/E")]
            [JsonProperty("Forward P/E")]
            public float ForwardPE { get; set; }
            [ElasticProperty(Name = "Volatility (Week)")]
            [JsonProperty("Volatility (Week)")]
            public float VolatilityWeek { get; set; }
            [ElasticProperty(Name = "Change from Open")]
            [JsonProperty("Change from Open")]
            public float ChangefromOpen { get; set; }
            [ElasticProperty(Name = "Sales growth quarter over quarter")]
            [JsonProperty("Sales growth quarter over quarter")]
            public float Salesgrowthquarteroverquarter { get; set; }
            [ElasticProperty(Name = "Short Ratio")]
            [JsonProperty("Short Ratio")]
            public float ShortRatio { get; set; }
            [ElasticProperty(Name = "Price")]
            [JsonProperty("Price")]
            public float Price { get; set; }
            [ElasticProperty(Name = "Volume")]
            [JsonProperty("Volume")]
            public float Volume { get; set; }
            [ElasticProperty(Name = "Relative Volume")]
            [JsonProperty("Relative Volume")]
            public float RelativeVolume { get; set; }
            [ElasticProperty(Name = "20-Day Simple Moving Average")]
            [JsonProperty("20-Day Simple Moving Average")]
            public float _20DaySimpleMovingAverage { get; set; }
            [ElasticProperty(Name = "Gap")]
            [JsonProperty("Gap")]
            public float Gap { get; set; }
            [ElasticProperty(Name = "Country")]
            [JsonProperty("Country")]
            public string Country { get; set; }
            [ElasticProperty(Name = "52-Week Low")]
            [JsonProperty("52-Week Low")]
            public float _52WeekLow { get; set; }
            [ElasticProperty(Name = "_id")]
            [JsonProperty("_id")]
            public float _id { get; set; }
            [ElasticProperty(Name = "Market Cap")]
            [JsonProperty("Market Cap")]
            public float MarketCap { get; set; }
            [ElasticProperty(Name = "EPS growth past 5 years")]
            [JsonProperty("EPS growth past 5 years")]
            public float EPSgrowthpast5years { get; set; }
            [ElasticProperty(Name = "50-Day Simple Moving Average")]
            [JsonProperty("50-Day Simple Moving Average")]
            public float _50DaySimpleMovingAverage { get; set; }
            [ElasticProperty(Name = "Quick Ratio")]
            [JsonProperty("Quick Ratio")]
            public float QuickRatio { get; set; }
            [ElasticProperty(Name = "P/B")]
            [JsonProperty("P/B")]
            public float PB { get; set; }
            [ElasticProperty(Name = "EPS growth next year")]
            [JsonProperty("EPS growth next year")]
            public float EPSgrowthnextyear { get; set; }
            [ElasticProperty(Name = "Profit Margin")]
            [JsonProperty("Profit Margin")]
            public float ProfitMargin { get; set; }
            [ElasticProperty(Name = "Performance (YTD)")]
            [JsonProperty("Performance (YTD)")]
            public float PerformanceYTD { get; set; }
            [ElasticProperty(Name = "Sales growth past 5 years")]
            [JsonProperty("Sales growth past 5 years")]
            public float Salesgrowthpast5years { get; set; }
            [ElasticProperty(Name = "P/E")]
            [JsonProperty("P/E")]
            public float PE { get; set; }
            [ElasticProperty(Name = "50-Day High")]
            [JsonProperty("50-Day High")]
            public float _50DayHigh { get; set; }
            [ElasticProperty(Name = "Relative Strength Index (14)")]
            [JsonProperty("Relative Strength Index (14)")]
            public float RelativeStrengthIndex14 { get; set; }
            [ElasticProperty(Name = "Float Short")]
            [JsonProperty("Float Short")]
            public float FloatShort { get; set; }
            [ElasticProperty(Name = "Analyst Recom")]
            [JsonProperty("Analyst Recom")]
            public float AnalystRecom { get; set; }
            [ElasticProperty(Name = "Return on Investment")]
            [JsonProperty("Return on Investment")]
            public float ReturnonInvestment { get; set; }
            [ElasticProperty(Name = "Return on Assets")]
            [JsonProperty("Return on Assets")]
            public float ReturnonAssets { get; set; }
            [ElasticProperty(Name = "Shares Outstanding")]
            [JsonProperty("Shares Outstanding")]
            public float SharesOutstanding { get; set; }
            [ElasticProperty(Name = "Volatility (Month)")]
            [JsonProperty("Volatility (Month)")]
            public float VolatilityMonth { get; set; }
            [ElasticProperty(Name = "Performance (Week)")]
            [JsonProperty("Performance (Week)")]
            public float PerformanceWeek { get; set; }
            [ElasticProperty(Name = "P/S")]
            [JsonProperty("P/S")]
            public float PS { get; set; }
            [ElasticProperty(Name = "Gross Margin")]
            [JsonProperty("Gross Margin")]
            public float GrossMargin { get; set; }
            [ElasticProperty(Name = "P/Free Cash Flow")]
            [JsonProperty("P/Free Cash Flow")]
            public float PFreeCashFlow { get; set; }
            [ElasticProperty(Name = "LT Debt/Equity")]
            [JsonProperty("LT Debt/Equity")]
            public float LTDebtEquity { get; set; }
            [ElasticProperty(Name = "52-Week High")]
            [JsonProperty("52-Week High")]
            public float _52WeekHigh { get; set; }
            [ElasticProperty(Name = "50-Day Low")]
            [JsonProperty("50-Day Low")]
            public float _50DayLow { get; set; }
            [ElasticProperty(Name = "Earnings Date")]
            [JsonProperty("Earnings Date")]
            public EarningsDate EarningsDate { get; set; }
            [ElasticProperty(Name = "Sector")]
            [JsonProperty("Sector")]
            public string Sector { get; set; }
            [ElasticProperty(Name = "Ticker")]
            [JsonProperty("Ticker")]
            public string Ticker { get; set; }
            [ElasticProperty(Name = "Institutional Ownership")]
            [JsonProperty("Institutional Ownership")]
            public float InstitutionalOwnership { get; set; }
            [ElasticProperty(Name = "EPS growth this year")]
            [JsonProperty("EPS growth this year")]
            public float EPSgrowththisyear { get; set; }
            [ElasticProperty(Name = "EPS growth quarter over quarter")]
            [JsonProperty("EPS growth quarter over quarter")]
            public float EPSgrowthquarteroverquarter { get; set; }
            [ElasticProperty(Name = "Return on Equity")]
            [JsonProperty("Return on Equity")]
            public float ReturnonEquity { get; set; }
            [ElasticProperty(Name = "Change")]
            [JsonProperty("Change")]
            public float Change { get; set; }
            [ElasticProperty(Name = "Performance (Month)")]
            [JsonProperty("Performance (Month)")]
            public float PerformanceMonth { get; set; }
            [ElasticProperty(Name = "PEG")]
            [JsonProperty("PEG")]
            public float PEG { get; set; }
            [ElasticProperty(Name = "Dividend Yield")]
            [JsonProperty("Dividend Yield")]
            public float DividendYield { get; set; }
            [ElasticProperty(Name = "Insider Ownership")]
            [JsonProperty("Insider Ownership")]
            public float InsiderOwnership { get; set; }
            [ElasticProperty(Name = "Performance (Quarter)")]
            [JsonProperty("Performance (Quarter)")]
            public float PerformanceQuarter { get; set; }
            [ElasticProperty(Name = "Insider Transactions")]
            [JsonProperty("Insider Transactions")]
            public float InsiderTransactions { get; set; }
        }

        public class EarningsDate
        {
            [ElasticProperty(Name = "$date")]
            [JsonProperty("$date")]
            public long date { get; set; }
        }


    }
}