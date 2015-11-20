using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using System.Web.Script.Serialization;
using System.IO;
using System.Runtime.Serialization;
using Nest;
using Newtonsoft.Json;

namespace NoSQL_Final.Controller
{
    class ElasticsearchClient
    {
        public ElasticsearchClient()
        {
            
        }

        public void Indexing(Stockobject obj)
        {
            try
            {
                var node = new Uri("http://localhost:9200");
                var settings = new ConnectionSettings(node, defaultIndex: "stockexchange");
                var client = new ElasticClient(settings);
                var index = client.Index <Stockobject>(obj);
                

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
                JavaScriptSerializer ser = new JavaScriptSerializer();
                while (file.ReadLine() != null)
                {
                    try
                    {
                        string line = file.ReadLine();
                        JObject json = JObject.Parse(line);
                        Stockobject obj = JsonConvert.DeserializeObject<Stockobject>(line);
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


       [JsonObject(MemberSerialization.OptIn)]
        public class Stockobject
        {
            [JsonProperty("_id")]
            public _Id _id { get; set; }
            [JsonProperty("Ticker")]
            public string Ticker { get; set; }
            [JsonProperty("Profit Margin")]
            public float ProfitMargin { get; set; }
            [JsonProperty("Institutional Ownership")]
            public float InstitutionalOwnership { get; set; }
            [JsonProperty("EPS growth past 5 years")]
            public float EPSgrowthpast5years { get; set; }
            [JsonProperty("Total Debt/Equity")]
            public float TotalDebtEquity { get; set; }
            [JsonProperty("Current Ratio")]
            public float CurrentRatio { get; set; }
            [JsonProperty("Return on Assets")]
            public float ReturnonAssets { get; set; }
            [JsonProperty("ector")]
            public string Sector { get; set; }
            [JsonProperty("P/S")]
            public float PS { get; set; }
            [JsonProperty("Change from Open")]
            public float ChangefromOpen { get; set; }
            [JsonProperty("Performance (YTD)")]
            public float PerformanceYTD { get; set; }
            [JsonProperty("Performance (Week)")]
            public float PerformanceWeek { get; set; }
            [JsonProperty("Quick Ratio")]
            public float QuickRatio { get; set; }
            [JsonProperty("Insider Transactions")]
            public float InsiderTransactions { get; set; }
            [JsonProperty("P/B")]
            public float PB { get; set; }
            [JsonProperty("EPS growth quarter over quarter")]
            public float EPSgrowthquarteroverquarter { get; set; }
            [JsonProperty("Payout Ratio")]
            public float PayoutRatio { get; set; }
            [JsonProperty("Performance (Quarter)")]
            public float PerformanceQuarter { get; set; }
            [JsonProperty("Forward P/E")]
            public float ForwardPE { get; set; }
            [JsonProperty("P/E")]
            public float PE { get; set; }
            [JsonProperty("200-Day Simple Moving Average")]
            public float _200DaySimpleMovingAverage { get; set; }
            [JsonProperty("Shares Outstanding")]
            public float SharesOutstanding { get; set; }
            [JsonProperty("Earnings Date")]
            public EarningsDate EarningsDate { get; set; }
            [JsonProperty("52-Week High")]
            public float _52WeekHigh { get; set; }
            [JsonProperty("P/Cash")]
            public float PCash { get; set; }
            [JsonProperty("Change")]
            public float Change { get; set; }
            [JsonProperty("Analyst Recom")]
            public float AnalystRecom { get; set; }
            [JsonProperty("Volatility (Week)")]
            public float VolatilityWeek { get; set; }
            [JsonProperty("Country")]
            public string Country { get; set; }
            [JsonProperty("Return on Equity")]
            public float ReturnonEquity { get; set; }
            [JsonProperty("50-Day Low")]
            public float _50DayLow { get; set; }
            [JsonProperty("Price")]
            public float Price { get; set; }
            [JsonProperty("50-Day High")]
            public float _50DayHigh { get; set; }
            [JsonProperty("Return on Investment")]
            public float ReturnonInvestment { get; set; }
            [JsonProperty("Shares Float")]
            public float SharesFloat { get; set; }
            [JsonProperty("Dividend Yield")]
            public float DividendYield { get; set; }
            [JsonProperty("EPS growth next 5 years")]
            public float EPSgrowthnext5years { get; set; }
            [JsonProperty("Industry")]
            public string Industry { get; set; }
            [JsonProperty("Beta")]
            public float Beta { get; set; }
            [JsonProperty("Sales growth quarter over quarter")]
            public float Salesgrowthquarteroverquarter { get; set; }
            [JsonProperty("Operating Margin")]
            public float OperatingMargin { get; set; }
            [JsonProperty("EPS (ttm)")]
            public float EPSttm { get; set; }
            [JsonProperty("PEG")]
            public float PEG { get; set; }
            [JsonProperty("Float Short")]
            public float FloatShort { get; set; }
            [JsonProperty("52-Week Low")]
            public float _52WeekLow { get; set; }
            [JsonProperty("Average True Range")]
            public float AverageTrueRange { get; set; }
            [JsonProperty("EPS growth next year")]
            public float EPSgrowthnextyear { get; set; }
            [JsonProperty("Sales growth past 5 years")]
            public float Salesgrowthpast5years { get; set; }
            [JsonProperty("Company")]
            public string Company { get; set; }
            [JsonProperty("Gap")]
            public float Gap { get; set; }
            [JsonProperty("Relative Volume")]
            public float RelativeVolume { get; set; }
            [JsonProperty("Volatility (Month)")]
            public float VolatilityMonth { get; set; }
            [JsonProperty("Market Cap")]
            public float MarketCap { get; set; }
            [JsonProperty("Volume")]
            public float Volume { get; set; }
            [JsonProperty("Gross Margin")]
            public float GrossMargin { get; set; }
            [JsonProperty("Short Ratio")]
            public float ShortRatio { get; set; }
            [JsonProperty("Performance (Half Year)")]
            public float PerformanceHalfYear { get; set; }
            [JsonProperty("Relative Strength Index (14)")]
            public float RelativeStrengthIndex14 { get; set; }
            [JsonProperty("Insider Ownership")]
            public float InsiderOwnership { get; set; }
            [JsonProperty("20-Day Simple Moving Average")]
            public float _20DaySimpleMovingAverage { get; set; }
            [JsonProperty("Performance (Month)")]
            public float PerformanceMonth { get; set; }
            [JsonProperty("P/Free Cash Flow")]
            public float PFreeCashFlow { get; set; }
            [JsonProperty("Institutional Transactions")]
            public float InstitutionalTransactions { get; set; }
            [JsonProperty("Performance (Year)")]
            public float PerformanceYear { get; set; }
            [JsonProperty("LT Debt/Equity")]
            public float LTDebtEquity { get; set; }
            [JsonProperty("Average Volume")]
            public float AverageVolume { get; set; }
            [JsonProperty("EPS growth this year")]
            public float EPSgrowththisyear { get; set; }
            [JsonProperty("50-Day Simple Moving Average")]
            public float _50DaySimpleMovingAverage { get; set; }


        }

        public class _Id
        {
            [JsonProperty("$oid")]
            public string oid { get; set; }
        }

        public class EarningsDate
        {
            [JsonProperty("$date")]
            public long date { get; set; }
        }

    }

    }
}
