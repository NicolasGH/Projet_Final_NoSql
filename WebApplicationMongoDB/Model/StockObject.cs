using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationMongoDB
{
    [ElasticType(Name = "stocks")]
    [JsonObject(MemberSerialization.OptIn)]
    [BsonIgnoreExtraElements]
    public class Stockobject
    {
        [ElasticProperty(Name = "EPS growth next 5 year")]
        [JsonProperty("EPS growth next 5 year")]
        public double EPSgrowthnext5years { get; set; }
        [ElasticProperty(Name = "Company")]
        [JsonProperty("Company")]
        public string Company { get; set; }
        [ElasticProperty(Name = "Current Ratio")]
        [JsonProperty("Current Ratio")]
        public double CurrentRatio { get; set; }
        [ElasticProperty(Name = "Payout Ratio")]
        [JsonProperty("Payout Ratio")]
        public double PayoutRatio { get; set; }
        [ElasticProperty(Name = "Operating Margin")]
        [JsonProperty("Operating Margin")]
        public double OperatingMargin { get; set; }
        [ElasticProperty(Name = "Shares double")]
        [JsonProperty("Shares double")]
        public double Sharesdouble { get; set; }
        [ElasticProperty(Name = "P/Cash")]
        [JsonProperty("P/Cash")]
        public double PCash { get; set; }
        [ElasticProperty(Name = "Institutional Transactions")]
        [JsonProperty("Institutional Transactions")]
        public double InstitutionalTransactions { get; set; }
        [ElasticProperty(Name = "Average True Range")]
        [JsonProperty("Average True Range")]
        public double AverageTrueRange { get; set; }
        [ElasticProperty(Name = "Performance (Half Year)")]
        [JsonProperty("Performance (Half Year)")]
        public double PerformanceHalfYear { get; set; }
        [ElasticProperty(Name = "Beta")]
        [JsonProperty("Beta")]
        public double Beta { get; set; }
        [ElasticProperty(Name = "Industry")]
        [JsonProperty("Industry")]
        public string Industry { get; set; }
        [ElasticProperty(Name = "Average Volume")]
        [JsonProperty("Average Volume")]
        public double AverageVolume { get; set; }
        [ElasticProperty(Name = "Performance (Year)")]
        [JsonProperty("Performance (Year)")]
        public double PerformanceYear { get; set; }
        [ElasticProperty(Name = "200-Day Simple Moving Average")]
        [JsonProperty("200-Day Simple Moving Average")]
        public double _200DaySimpleMovingAverage { get; set; }
        [ElasticProperty(Name = "Total Debt/Equity")]
        [JsonProperty("Total Debt/Equity")]
        public double TotalDebtEquity { get; set; }
        [ElasticProperty(Name = "EPS (ttm)")]
        [JsonProperty("EPS (ttm)")]
        public double EPSttm { get; set; }
        [ElasticProperty(Name = "Forward P/E")]
        [JsonProperty("Forward P/E")]
        public double ForwardPE { get; set; }
        [ElasticProperty(Name = "Volatility (Week)")]
        [JsonProperty("Volatility (Week)")]
        public double VolatilityWeek { get; set; }
        [ElasticProperty(Name = "Change from Open")]
        [JsonProperty("Change from Open")]
        public double ChangefromOpen { get; set; }
        [ElasticProperty(Name = "Sales growth quarter over quarter")]
        [JsonProperty("Sales growth quarter over quarter")]
        public double Salesgrowthquarteroverquarter { get; set; }
        [ElasticProperty(Name = "Short Ratio")]
        [JsonProperty("Short Ratio")]
        public double ShortRatio { get; set; }
        [ElasticProperty(Name = "Price")]
        [JsonProperty("Price")]
        public double Price { get; set; }
        [ElasticProperty(Name = "Volume")]
        [JsonProperty("Volume")]
        public double Volume { get; set; }
        [ElasticProperty(Name = "Relative Volume")]
        [JsonProperty("Relative Volume")]
        public double RelativeVolume { get; set; }
        [ElasticProperty(Name = "20-Day Simple Moving Average")]
        [JsonProperty("20-Day Simple Moving Average")]
        public double _20DaySimpleMovingAverage { get; set; }
        [ElasticProperty(Name = "Gap")]
        [JsonProperty("Gap")]
        public double Gap { get; set; }
        [ElasticProperty(Name = "Country")]
        [JsonProperty("Country")]
        public string Country { get; set; }
        [ElasticProperty(Name = "52-Week Low")]
        [JsonProperty("52-Week Low")]
        public double _52WeekLow { get; set; }
        [ElasticProperty(Name = "_id")]
        [JsonProperty("_id")]
        public Object _id { get; set; }
        [ElasticProperty(Name = "Market Cap")]
        [JsonProperty("Market Cap")]
        public double MarketCap { get; set; }
        [ElasticProperty(Name = "EPS growth past 5 years")]
        [JsonProperty("EPS growth past 5 years")]
        public double EPSgrowthpast5years { get; set; }
        [ElasticProperty(Name = "50-Day Simple Moving Average")]
        [JsonProperty("50-Day Simple Moving Average")]
        public double _50DaySimpleMovingAverage { get; set; }
        [ElasticProperty(Name = "Quick Ratio")]
        [JsonProperty("Quick Ratio")]
        public double QuickRatio { get; set; }
        [ElasticProperty(Name = "P/B")]
        [JsonProperty("P/B")]
        public double PB { get; set; }
        [ElasticProperty(Name = "EPS growth next year")]
        [JsonProperty("EPS growth next year")]
        public double EPSgrowthnextyear { get; set; }
        [ElasticProperty(Name = "Profit Margin")]
        [JsonProperty("Profit Margin")]
        public double ProfitMargin { get; set; }
        [ElasticProperty(Name = "Performance (YTD)")]
        [JsonProperty("Performance (YTD)")]
        public double PerformanceYTD { get; set; }
        [ElasticProperty(Name = "Sales growth past 5 years")]
        [JsonProperty("Sales growth past 5 years")]
        public double Salesgrowthpast5years { get; set; }
        [ElasticProperty(Name = "P/E")]
        [JsonProperty("P/E")]
        public double PE { get; set; }
        [ElasticProperty(Name = "50-Day High")]
        [JsonProperty("50-Day High")]
        public double _50DayHigh { get; set; }
        [ElasticProperty(Name = "Relative Strength Index (14)")]
        [JsonProperty("Relative Strength Index (14)")]
        public double RelativeStrengthIndex14 { get; set; }
        [ElasticProperty(Name = "double Short")]
        [JsonProperty("double Short")]
        public double doubleShort { get; set; }
        [ElasticProperty(Name = "Analyst Recom")]
        [JsonProperty("Analyst Recom")]
        public double AnalystRecom { get; set; }
        [ElasticProperty(Name = "Return on Investment")]
        [JsonProperty("Return on Investment")]
        public double ReturnonInvestment { get; set; }
        [ElasticProperty(Name = "Return on Assets")]
        [JsonProperty("Return on Assets")]
        public double ReturnonAssets { get; set; }
        [ElasticProperty(Name = "Shares Outstanding")]
        [JsonProperty("Shares Outstanding")]
        public double SharesOutstanding { get; set; }
        [ElasticProperty(Name = "Volatility (Month)")]
        [JsonProperty("Volatility (Month)")]
        public double VolatilityMonth { get; set; }
        [ElasticProperty(Name = "Performance (Week)")]
        [JsonProperty("Performance (Week)")]
        public double PerformanceWeek { get; set; }
        [ElasticProperty(Name = "P/S")]
        [JsonProperty("P/S")]
        public double PS { get; set; }
        [ElasticProperty(Name = "Gross Margin")]
        [JsonProperty("Gross Margin")]
        public double GrossMargin { get; set; }
        [ElasticProperty(Name = "P/Free Cash Flow")]
        [JsonProperty("P/Free Cash Flow")]
        public double PFreeCashFlow { get; set; }
        [ElasticProperty(Name = "LT Debt/Equity")]
        [JsonProperty("LT Debt/Equity")]
        public double LTDebtEquity { get; set; }
        [ElasticProperty(Name = "52-Week High")]
        [JsonProperty("52-Week High")]
        public double _52WeekHigh { get; set; }
        [ElasticProperty(Name = "50-Day Low")]
        [JsonProperty("50-Day Low")]
        public double _50DayLow { get; set; }
        //[ElasticProperty(Name = "Earnings Date")]
        //[JsonProperty("Earnings Date")]
        //public string EarningsDate { get; set; }

        [ElasticProperty(Name = "Sector")]
        [JsonProperty("Sector")]
        public string Sector { get; set; }
        [ElasticProperty(Name = "Ticker")]
        [JsonProperty("Ticker")]
        public string Ticker { get; set; }
        [ElasticProperty(Name = "Institutional Ownership")]
        [JsonProperty("Institutional Ownership")]
        public double InstitutionalOwnership { get; set; }
        [ElasticProperty(Name = "EPS growth this year")]
        [JsonProperty("EPS growth this year")]
        public double EPSgrowththisyear { get; set; }
        [ElasticProperty(Name = "EPS growth quarter over quarter")]
        [JsonProperty("EPS growth quarter over quarter")]
        public double EPSgrowthquarteroverquarter { get; set; }
        [ElasticProperty(Name = "Return on Equity")]
        [JsonProperty("Return on Equity")]
        public double ReturnonEquity { get; set; }
        [ElasticProperty(Name = "Change")]
        [JsonProperty("Change")]
        public double Change { get; set; }
        [ElasticProperty(Name = "Performance (Month)")]
        [JsonProperty("Performance (Month)")]
        public double PerformanceMonth { get; set; }
        [ElasticProperty(Name = "PEG")]
        [JsonProperty("PEG")]
        public double PEG { get; set; }
        [ElasticProperty(Name = "Dividend Yield")]
        [JsonProperty("Dividend Yield")]
        public double DividendYield { get; set; }
        [ElasticProperty(Name = "Insider Ownership")]
        [JsonProperty("Insider Ownership")]
        public double InsiderOwnership { get; set; }
        [ElasticProperty(Name = "Performance (Quarter)")]
        [JsonProperty("Performance (Quarter)")]
        public double PerformanceQuarter { get; set; }
        [ElasticProperty(Name = "Insider Transactions")]
        [JsonProperty("Insider Transactions")]
        public double InsiderTransactions { get; set; }
    }

    public class EarningsDate
    {
        [ElasticProperty(Name = "$date")]
        [JsonProperty("$date")]
        public long date { get; set; }
    }
}