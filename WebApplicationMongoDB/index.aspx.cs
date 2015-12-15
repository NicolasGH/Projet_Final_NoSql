using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplicationMongoDB.Controller;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
 

namespace WebApplicationMongoDB.View
{
    public partial class index : System.Web.UI.Page
    {
       
        protected WebApplicationMongoDB.Controller.ElasticsearchClient elastic = new Controller.ElasticsearchClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            MongoDBClient mdc = new MongoDBClient("stockcollection");

            Dictionary<int, HtmlGenericControl> dctPnl = new Dictionary<int, HtmlGenericControl>();
            Dictionary<int, AjaxControlToolkit.ModalPopupExtender> dctMdl = new Dictionary<int, AjaxControlToolkit.ModalPopupExtender>();
            dctPnl.Add(1,content);
            dctPnl.Add(2,content2);
            dctPnl.Add(3, content3);
            dctPnl.Add(4, content4);
            dctPnl.Add(5, content5);
            dctPnl.Add(6, content6);
            dctPnl.Add(7, content7);
            dctPnl.Add(8, content8);
            dctPnl.Add(9, content9);
            dctPnl.Add(10, content10);
            dctMdl.Add(1, ModalPopupExtender1);
            dctMdl.Add(2, ModalPopupExtender2);
            dctMdl.Add(3, ModalPopupExtender3);
            dctMdl.Add(4, ModalPopupExtender4);
            dctMdl.Add(5, ModalPopupExtender5);
            dctMdl.Add(6, ModalPopupExtender6);
            dctMdl.Add(7, ModalPopupExtender7);
            dctMdl.Add(8, ModalPopupExtender8);
            dctMdl.Add(9, ModalPopupExtender9);
            dctMdl.Add(10, ModalPopupExtender10);

            //mdc.LoadLocalData(@"Z:\Downloads\stocks-2.json");
            int i=1;
            Task t = Task.Run(() =>
            {
                for (i = 1; i <= 10; i++)
                {
                    try
                    {
                        dctPnl[i].Controls.Remove(createInputs(elastic.SearchRequestToES(srchIpt.Value).ToList()[i - 1]));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }
            });
            t.Wait();

            i = 1;
  
            foreach (Stockobject obj in elastic.SearchRequestToES(srchIpt.Value))
            {
                System.Web.UI.WebControls.Panel pnlC = new System.Web.UI.WebControls.Panel();
                pnlC.Attributes["class"] = "panel-body";
                System.Web.UI.WebControls.Button btn = new System.Web.UI.WebControls.Button();
                pnlC.ID = "btn" + i;
                System.Web.UI.WebControls.Panel pnlP = new System.Web.UI.WebControls.Panel();
                pnlP.Attributes["class"] = "panel panel-default panelStock";
                System.Web.UI.WebControls.Label lbl = new System.Web.UI.WebControls.Label();
                lbl.Text = obj.Company;
                pnlC.Controls.Add(lbl);
                pnlP.Controls.Add(pnlC);

                if (srchIpt.Value == "")
                {
                    containerStock.Controls.Add(pnlP);
                }
                i++;
            }
           
            for (i = 1; i <= 10; i++)
            {
                System.Web.UI.WebControls.Button btnB = new System.Web.UI.WebControls.Button();
                btnB.Attributes["class"] = "btn-popup";
                btnB.Text = "Update";
                System.Web.UI.WebControls.Button btnS = new System.Web.UI.WebControls.Button();
                btnS.Attributes["class"] = "btn-popup";
                btnS.Text = "Delete";
                try
                {
                  Stockobject ob = elastic.SearchRequestToES(srchIpt.Value).ToList()[i-1];
                  dctPnl[i].Controls.Add(createInputs(ob));
                  dctMdl[i].TargetControlID = "btn" + i;
                  btnS.Click += (s, ex) =>
                  {
                      mdc.deleteData(ob);
                  };
                  btnB.Click += (s, ex) =>
                  {
                      mdc.updateData(ob, "stockcollection");
                  };

                }
                catch (Exception ex)
                {
                    dctMdl[i].TargetControlID = "secour1";
                }
                dctPnl[i].Controls.Add(btnS);
                dctPnl[i].Controls.Add(btnB);
            }
            foreach(string str in mdc.searchCountries())
            {

                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes["href"] = "#";
                a.InnerText = str;
                li.Controls.Add(a);
                selectCountry.Controls.Add(li);
            }

            PieChart1.Attributes["PieLabelStyle"] = "Outside";
            //PieChart1.Label = "#PERCENT{P2}";
            foreach (KeyValuePair<string, int> entry in mdc.mapReduceIndustryCountry("Chile"))
            {
                PieChart1.PieChartValues.Add(new AjaxControlToolkit.PieChartValue
                {
                    Data = Convert.ToDecimal(entry.Value),
                    Category = entry.Key
                });
            }

            foreach (KeyValuePair<string, int> entry in mdc.mapReduceMarketCapCountry())
            {
                PieChart2.PieChartValues.Add(new AjaxControlToolkit.PieChartValue
                {
                    Data = Convert.ToDecimal(entry.Value),
                    Category = entry.Key
                });
            }

            
        }

        protected void searchfct(object sender, EventArgs e)
        {
            int i = 1;
            foreach (WebApplicationMongoDB.Stockobject obj in elastic.SearchRequestToES(srchIpt.Value))
            {
                System.Web.UI.WebControls.Panel pnlC = new System.Web.UI.WebControls.Panel();
                pnlC.Attributes["class"] = "panel-body";
                pnlC.ID = "btn" + i;
                System.Web.UI.WebControls.Panel pnlP = new System.Web.UI.WebControls.Panel();
                pnlP.Attributes["class"] = "panel panel-default panelStock";
                System.Web.UI.WebControls.Label lbl = new System.Web.UI.WebControls.Label();
                lbl.Text = obj.Company;
                pnlC.Controls.Add(lbl);
                pnlP.Controls.Add(pnlC);
                containerStock.Controls.Add(pnlP);
                i++;
            } 

            

        }

      

        public Table createInputs(Stockobject obj)
        {
            System.Web.UI.WebControls.Label lbl1 = new System.Web.UI.WebControls.Label();
            lbl1.Attributes["class"] = "lblDetails";
            lbl1.Text = "Company";
            HtmlGenericControl inpt1 = new HtmlGenericControl("input");
            inpt1.Attributes["type"] = "text";
            inpt1.Attributes["placeholder"] = "Company";
            inpt1.ID = "inpt1" + obj._id;
            inpt1.Attributes["class"] = "input";
            inpt1.Attributes["value"] = obj.Company.ToString();

            System.Web.UI.WebControls.Label lbl2 = new System.Web.UI.WebControls.Label();
            lbl2.Attributes["class"] = "lblDetails";
            lbl2.Text = "Industry";
            HtmlGenericControl inpt2 = new HtmlGenericControl("input");
            inpt2.Attributes["type"] = "text";
            inpt2.Attributes["placeholder"] = "Industry";
            inpt2.ID = "inpt2" + obj._id;
            inpt2.Attributes["class"] = "input";
            inpt2.Attributes["value"] = obj.Industry;

            System.Web.UI.WebControls.Label lbl3 = new System.Web.UI.WebControls.Label();
            lbl3.Attributes["class"] = "lblDetails";
            lbl3.Text = "Volume";
            HtmlGenericControl inpt3 = new HtmlGenericControl("input");
            inpt3.Attributes["type"] = "text";
            inpt3.Attributes["placeholder"] = "Volume";
            inpt3.ID = "inpt3" + obj._id;
            inpt3.Attributes["class"] = "input";
            inpt3.Attributes["value"] = obj.Volume.ToString();

            System.Web.UI.WebControls.Label lbl4 = new System.Web.UI.WebControls.Label();
            lbl4.Attributes["class"] = "lblDetails";
            lbl4.Text = "Country";
            HtmlGenericControl inpt4 = new HtmlGenericControl("input");
            inpt4.Attributes["type"] = "text";
            inpt4.Attributes["placeholder"] = "Country";
            inpt4.ID = "inpt4"+obj._id;
            inpt4.Attributes["class"] = "input";
            inpt4.Attributes["value"] = obj.Country.ToString();

            System.Web.UI.WebControls.Label lbl5 = new System.Web.UI.WebControls.Label();
            lbl5.Attributes["class"] = "lblDetails";
            lbl5.Text = "Sector";
            HtmlGenericControl inpt5 = new HtmlGenericControl("input");
            inpt5.Attributes["type"] = "text";
            inpt5.Attributes["placeholder"] = "Sector";
            inpt5.ID = "inpt5" + obj._id;
            inpt5.Attributes["class"] = "input";
            inpt5.Attributes["value"] = obj.Sector.ToString();

            System.Web.UI.WebControls.Label lbl6 = new System.Web.UI.WebControls.Label();
            lbl6.Attributes["class"] = "lblDetails";
            lbl6.Text = "Ticker";
            HtmlGenericControl inpt6 = new HtmlGenericControl("input");
            inpt6.Attributes["type"] = "text";
            inpt6.Attributes["placeholder"] = "Ticker";
            inpt6.ID = "inpt6" + obj._id;
            inpt6.Attributes["class"] = "input";
            inpt6.Attributes["value"] = obj.Ticker.ToString();

            System.Web.UI.WebControls.Label lbl7 = new System.Web.UI.WebControls.Label();
            lbl7.Attributes["class"] = "lblDetails";
            lbl7.Text = "Price";
            HtmlGenericControl inpt7 = new HtmlGenericControl("input");
            inpt7.Attributes["type"] = "text";
            inpt7.Attributes["placeholder"] = "Price";
            inpt7.ID = "inpt7" + obj._id;
            inpt7.Attributes["class"] = "input";
            inpt7.Attributes["value"] = obj.Price.ToString();

            System.Web.UI.WebControls.Label lbl8 = new System.Web.UI.WebControls.Label();
            lbl8.Attributes["class"] = "lblDetails";
            lbl8.Text = "Volatility (Month)";
            HtmlGenericControl inpt8 = new HtmlGenericControl("input");
            inpt8.Attributes["type"] = "text";
            inpt8.Attributes["placeholder"] = "Volatility (Month)";
            inpt8.ID = "inpt8" + obj._id;
            inpt8.Attributes["class"] = "input";
            inpt8.Attributes["value"] = obj.VolatilityMonth.ToString();

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

            tbl.Controls.Add(row2);


            TableRow row3 = new TableRow();

            TableCell cell15 = new TableCell();
            cell15.Controls.Add(lbl5);
            row3.Controls.Add(cell15);
            TableCell cell16 = new TableCell();
            cell16.Controls.Add(lbl6);
            row3.Controls.Add(cell16);
            TableCell cell17 = new TableCell();
            cell17.Controls.Add(lbl7);
            row3.Controls.Add(cell17);
            TableCell cell18 = new TableCell();
            cell18.Controls.Add(lbl8);
            row3.Controls.Add(cell18);

            tbl.Controls.Add(row3);

            TableRow row4 = new TableRow();

            TableCell cell25 = new TableCell();
            cell25.Controls.Add(inpt5);
            row4.Controls.Add(cell25);
            TableCell cell26 = new TableCell();
            cell26.Controls.Add(inpt6);
            row4.Controls.Add(cell26);
            TableCell cell27 = new TableCell();
            cell27.Controls.Add(inpt7);
            row4.Controls.Add(cell27);
            TableCell cell28 = new TableCell();
            cell28.Controls.Add(inpt8);
            row4.Controls.Add(cell28);

            tbl.Controls.Add(row4);

            tbl.Attributes["class"] = "tableInpt";
            return tbl;
        }

        //public Panel createPopUp(Stockobject obj)
        //{
        //    try
        //    {
        //        Panel pnlP = new Panel();
        //        pnlP.Attributes["class"] = "poping";
        //        Panel pnlS = new Panel();
        //        pnlS.ID = "Panel" + obj._id.ToString();
        //        pnlS.Attributes["class"] = "content";
        //        HtmlGenericControl h1 = new HtmlGenericControl("h1");
        //        h1.InnerHtml = "Details";
        //        HtmlGenericControl div = new HtmlGenericControl("div");
        //        div.ID = "content" + obj._id.ToString();
        //        HtmlGenericControl a = new HtmlGenericControl("a");
        //        a.ID = "modalclosebutton" + obj._id.ToString();
        //        div.Controls.Add(h1);
        //        div.Controls.Add(a);
        //        div.Controls.Add(createInputs(obj));
        //        pnlS.Controls.Add(div);

        //        AjaxControlToolkit.ModalPopupExtender modalPop = new AjaxControlToolkit.ModalPopupExtender();
        //        modalPop.ID = "popUp" + obj._id.ToString();
        //        modalPop.PopupControlID = "Panel" + obj._id.ToString();
        //        modalPop.TargetControlID = "btn" + obj._id.ToString();
        //        modalPop.DropShadow = true;
        //        modalPop.BackgroundCssClass = "modalBackground";
        //        modalPop.CancelControlID = "modalclosebutton" + obj._id.ToString();


        //        pnlP.Controls.Add(pnlS);
        //        pnlP.Controls.Add(modalPop);

        //        return pnlP;
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        return null;
        //    }
        //}

    }
}