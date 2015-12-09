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
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;

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

        public Panel createPopUp(Stockobject obj)
        {
          
            try
            {
                Panel pnlP = new Panel();
                pnlP.Attributes["class"] = "poping";
                Panel pnlS = new Panel();
                pnlS.ID = "Panel" + obj._id.ToString();
                pnlS.Attributes["class"] = "content";
                HtmlGenericControl h1 = new HtmlGenericControl("h1");
                h1.InnerHtml = "Details";
                HtmlGenericControl div = new HtmlGenericControl("div");
                div.ID = "content" + obj._id.ToString();
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.ID = "modalclosebutton" + obj._id.ToString();
                div.Controls.Add(h1);
                div.Controls.Add(a);
                div.Controls.Add(createInputs(obj));
                pnlS.Controls.Add(div);

                AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
                modalPop.ID = "popUp" + obj._id.ToString();
                modalPop.PopupControlID = "Panel" + obj._id.ToString();
                modalPop.TargetControlID = "btn" + obj._id.ToString();
                modalPop.DropShadow = true;
                modalPop.BackgroundCssClass = "modalBackground";
                modalPop.CancelControlID = "modalclosebutton" + obj._id.ToString();

              
                pnlP.Controls.Add(pnlS);
                pnlP.Controls.Add(modalPop);

                return pnlP;
            }
            catch(Exception e)
            {
                e.ToString();
                return null;
            }
              

                 

           

        }

        public Table createInputs(Stockobject  obj)
        {
            

            Label lbl1 = new Label();
            lbl1.Attributes["class"] = "lblDetails";
            lbl1.Text = "EPS growth next 5 year";
            HtmlGenericControl inpt1 = new HtmlGenericControl("input");
            inpt1.Attributes["type"] = "text";
            inpt1.Attributes["placeholder"] = "EPS growth next 5 year";
            inpt1.ID = "inpt1";
            inpt1.Attributes["class"] = "input";
            inpt1.Attributes["value"] = obj.EPSgrowthnext5years.ToString();

            Label lbl2 = new Label();
            lbl2.Attributes["class"] = "lblDetails";
            lbl2.Text = "Company";
            HtmlGenericControl inpt2 = new HtmlGenericControl("input");
            inpt2.Attributes["type"] = "text";
            inpt2.Attributes["placeholder"] = "Company";
            inpt2.ID = "inpt2";
            inpt2.Attributes["class"] = "input";
            inpt2.Attributes["value"] = obj.Company;

            Label lbl3 = new Label();
            lbl3.Attributes["class"] = "lblDetails";
            lbl3.Text = "Current Ratio";
            HtmlGenericControl inpt3 = new HtmlGenericControl("input");
            inpt3.Attributes["type"] = "text";
            inpt3.Attributes["placeholder"] = "Current Ratio";
            inpt3.ID = "inpt3";
            inpt3.Attributes["class"] = "input";
            inpt3.Attributes["value"] = obj.CurrentRatio.ToString();

            Label lbl4 = new Label();
            lbl4.Attributes["class"] = "lblDetails";
            lbl4.Text = "Payout Ratio";
            HtmlGenericControl inpt4 = new HtmlGenericControl("input");
            inpt4.Attributes["type"] = "text";
            inpt4.Attributes["placeholder"] = "Payout Ratio";
            inpt4.ID = "inpt4";
            inpt4.Attributes["class"] = "input";
            inpt4.Attributes["value"] = obj.PayoutRatio.ToString();

            Label lbl5 = new Label();
            lbl5.Attributes["class"] = "lblDetails";
            lbl5.Text = "Operating Margin";
            HtmlGenericControl inpt5 = new HtmlGenericControl("input");
            inpt5.Attributes["type"] = "text";
            inpt5.Attributes["placeholder"] = "Operating Margin";
            inpt5.ID = "inpt5";
            inpt5.Attributes["class"] = "input";
            inpt5.Attributes["value"] = obj.OperatingMargin.ToString();

            Label lbl6 = new Label();
            lbl6.Attributes["class"] = "lblDetails";
            lbl6.Text = "Shares Float";
            HtmlGenericControl inpt6 = new HtmlGenericControl("input");
            inpt6.Attributes["type"] = "text";
            inpt6.Attributes["placeholder"] = "Shares Float";
            inpt6.ID = "inpt6";
            inpt6.Attributes["class"] = "input";
            inpt6.Attributes["value"] = obj.SharesFloat.ToString();

            //Label lbl7 = new Label();
            //lbl7.Attributes["class"] = "lblDetails";
            //lbl7.Text = "P/Cash";
            //HtmlGenericControl inpt7 = new HtmlGenericControl("input");
            //inpt7.Attributes["type"] = "text";
            //inpt7.Attributes["placeholder"] = "P/Cash";
            //inpt7.ID = "inpt7";
            //inpt7.Attributes["class"] = "input";
            //inpt7.Attributes["value"] = obj.PCash.ToString();

            //Label lbl8 = new Label();
            //lbl8.Attributes["class"] = "lblDetails";
            //lbl8.Text = "Institutional Transactions";
            //HtmlGenericControl inpt8 = new HtmlGenericControl("input");
            //inpt8.Attributes["type"] = "text";
            //inpt8.Attributes["placeholder"] = "Institutional Transactions";
            //inpt8.ID = "inpt8";
            //inpt8.Attributes["class"] = "input";
            //inpt8.Attributes["value"] = obj.InstitutionalTransactions.ToString();

            //Label lbl9 = new Label();
            //lbl9.Attributes["class"] = "lblDetails";
            //lbl9.Text = "Average True Range";
            //HtmlGenericControl inpt9 = new HtmlGenericControl("input");
            //inpt9.Attributes["type"] = "text";
            //inpt9.Attributes["placeholder"] = "Average True Range";
            //inpt9.ID = "inpt9";
            //inpt9.Attributes["class"] = "input";
            //inpt9.Attributes["value"] = obj.AverageTrueRange.ToString();

            //Label lbl10 = new Label();
            //lbl10.Attributes["class"] = "lblDetails";
            //lbl10.Text = "Performance (Half Year)";
            //HtmlGenericControl inpt10 = new HtmlGenericControl("input");
            //inpt10.Attributes["type"] = "text";
            //inpt10.Attributes["placeholder"] = "Performance (Half Year)";
            //inpt10.ID = "inpt10";
            //inpt10.Attributes["class"] = "input";
            //inpt10.Attributes["value"] = obj.PerformanceHalfYear.ToString();

            Table tbl = new Table();
            TableRow row1 = new TableRow();
            TableCell cell11 = new TableCell();
            cell11.Controls.Add(lbl1);
            row1.Controls.Add(cell11);
            TableCell cell12 = new TableCell();
            cell12.Controls.Add(lbl2);
            row1.Controls.Add(cell12);
            TableCell cell13 = new TableCell();
            cell13.Controls.Add(lbl3);
            row1.Controls.Add(cell13);
            TableCell cell14 = new TableCell();
            cell14.Controls.Add(lbl4);
            row1.Controls.Add(cell14);
            TableCell cell15 = new TableCell();
            cell15.Controls.Add(lbl5);
            row1.Controls.Add(cell15);
            TableCell cell16 = new TableCell();
            cell16.Controls.Add(lbl6);
            row1.Controls.Add(cell16);
            TableCell cell17 = new TableCell();
            //cell17.Controls.Add(lbl7);
            //row1.Controls.Add(cell17);
            //TableCell cell18 = new TableCell();
            //cell18.Controls.Add(lbl8);
            //row1.Controls.Add(cell18);
            //TableCell cell19 = new TableCell();
            //cell19.Controls.Add(lbl9);
            //row1.Controls.Add(cell19);
            //TableCell cell110 = new TableCell();
            //cell110.Controls.Add(lbl10);
            //row1.Controls.Add(cell110);


            tbl.Controls.Add(row1);

            TableRow row2 = new TableRow();
            TableCell cell21 = new TableCell();
            cell21.Controls.Add(inpt1);
            row2.Controls.Add(cell21);
            TableCell cell22 = new TableCell();
            cell22.Controls.Add(inpt2);
            row2.Controls.Add(cell22);
            TableCell cell23 = new TableCell();
            cell23.Controls.Add(inpt3);
            row2.Controls.Add(cell23);
            TableCell cell24 = new TableCell();
            cell24.Controls.Add(inpt4);
            row2.Controls.Add(cell24);
            TableCell cell25 = new TableCell();
            cell25.Controls.Add(inpt5);
            row2.Controls.Add(cell25);
            TableCell cell26 = new TableCell();
            cell26.Controls.Add(inpt6);
            row2.Controls.Add(cell26);
            //TableCell cell27 = new TableCell();
            //cell27.Controls.Add(inpt7);
            //row2.Controls.Add(cell27);
            //TableCell cell28 = new TableCell();
            //cell28.Controls.Add(inpt8);
            //row2.Controls.Add(cell28);
            //TableCell cell29 = new TableCell();
            //cell29.Controls.Add(inpt9);
            //row2.Controls.Add(cell29);
            //TableCell cell210 = new TableCell();
            //cell210.Controls.Add(inpt10);
            //row2.Controls.Add(cell210);

            tbl.Controls.Add(row2);
            //for (int i = 0; i <= 0; i++)
            //{
            //    TableRow row = new TableRow();
            //    for (int y = 0; i <= 9; i++)
            //    {
            //        TableCell cell = new TableCell();
            //        HtmlElement htmlelement = this.FindControl("Name");
            //       // cell.Controls.Add();
            //        cell.ID="("+i+","+y+")";
            //        row.Cells.Add(cell);
            //    }
            //    tbl.Rows.Add(row);
            //}

            

            

            


                return tbl;
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