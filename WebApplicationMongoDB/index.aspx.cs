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
            dctMdl.Add(1, ModalPopupExtender1);
            dctMdl.Add(2, ModalPopupExtender2);

            //mdc.LoadLocalData(@"Z:\Downloads\stocks-2.json");
            mdc.mapReduceIndustryCountry("France");
            int i=1;
            Task t = Task.Run(() =>
            {
                for (i = 1; i <= 2; i++)
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
                Panel pnlC = new Panel();
                pnlC.Attributes["class"] = "panel-body";
                Button btn = new Button();
                //pnlC.ID="btn" + obj._id.ToString();
                pnlC.ID = "btn" + i;
                //btn.ID = "btn" + obj._id.ToString();
                Panel pnlP = new Panel();
                pnlP.Attributes["class"] = "panel panel-default panelStock";
                Label lbl = new Label();
                lbl.Text = obj.Company;
                pnlC.Controls.Add(lbl);
                //pnlC.Controls.Add(btn);
                pnlP.Controls.Add(pnlC);

                if (srchIpt.Value == "")
                {
                    //containerStock.Controls.Add(elastic.createPopUp(obj));
                    containerStock.Controls.Add(pnlP);
                }
                i++;
            }
           
            for (i = 1; i <= 2; i++)
            {
                try
                {
                  dctPnl[i].Controls.Add(createInputs(elastic.SearchRequestToES(srchIpt.Value).ToList()[i-1]));
                  dctMdl[i].TargetControlID = "btn" + i;
                }
                catch (Exception ex)
                {
                    dctMdl[i].TargetControlID = "secour1";
                }
                Button btnB = new Button();
                btnB.Attributes["class"] = "btn-popup";
                btnB.Text = "Update";
                Button btnS = new Button();
                btnS.Attributes["class"] = "btn-popup";
                btnS.Text = "Delete";

                dctPnl[i].Controls.Add(btnS);
                dctPnl[i].Controls.Add(btnB);
            }
            //try
            //{
            //    content.Controls.Add(createInputs(elastic.SearchRequestToES(srchIpt.Value).ToList()[0]));
            //    ModalPopupExtender1.TargetControlID = "btn1";
            //}
            //catch (Exception ex)
            //{
            //    ModalPopupExtender1.TargetControlID = "secour1";
            //}
            //try
            //{
            //    content2.Controls.Add(createInputs(elastic.SearchRequestToES(srchIpt.Value).ToList()[1]));
            //    ModalPopupExtender2.TargetControlID = "btn2";
            //}
            //catch (Exception ex)
            //{
            //    ModalPopupExtender2.TargetControlID = "secour2";
            //}
            
            
            
        }

        protected void searchfct(object sender, EventArgs e)
        {
            //containerStock.Controls.Remove(pnlP);
            int i = 1;
            foreach (WebApplicationMongoDB.Stockobject obj in elastic.SearchRequestToES(srchIpt.Value))
            {
                Panel pnlC = new Panel();
                pnlC.Attributes["class"] = "panel-body";
                pnlC.ID = "btn" + i;
                Panel pnlP = new Panel();
                pnlP.Attributes["class"] = "panel panel-default panelStock";
                Label lbl = new Label();
                lbl.Text = obj.Company;
                pnlC.Controls.Add(lbl);
                pnlP.Controls.Add(pnlC);
                containerStock.Controls.Add(pnlP);
                i++;
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
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }

        public Table createInputs(Stockobject obj)
        {
            Label lbl1 = new Label();
            lbl1.Attributes["class"] = "lblDetails";
            lbl1.Text = "Company";
            HtmlGenericControl inpt1 = new HtmlGenericControl("input");
            inpt1.Attributes["type"] = "text";
            inpt1.Attributes["placeholder"] = "Company";
            inpt1.ID = "inpt1" + obj._id;
            inpt1.Attributes["class"] = "input";
            inpt1.Attributes["value"] = obj.Company.ToString();

            Label lbl2 = new Label();
            lbl2.Attributes["class"] = "lblDetails";
            lbl2.Text = "Industry";
            HtmlGenericControl inpt2 = new HtmlGenericControl("input");
            inpt2.Attributes["type"] = "text";
            inpt2.Attributes["placeholder"] = "Industry";
            inpt2.ID = "inpt2" + obj._id;
            inpt2.Attributes["class"] = "input";
            inpt2.Attributes["value"] = obj.Industry;

            //Label lbl3 = new Label();
            //lbl3.Attributes["class"] = "lblDetails";
            //lbl3.Text = "Current Ratio";
            //HtmlGenericControl inpt3 = new HtmlGenericControl("input");
            //inpt3.Attributes["type"] = "text";
            //inpt3.Attributes["placeholder"] = "Current Ratio";
            //inpt3.ID = "inpt3";
            //inpt3.Attributes["class"] = "input";
            //inpt3.Attributes["value"] = obj.CurrentRatio.ToString();

            Label lbl4 = new Label();
            lbl4.Attributes["class"] = "lblDetails";
            lbl4.Text = "Country";
            HtmlGenericControl inpt4 = new HtmlGenericControl("input");
            inpt4.Attributes["type"] = "text";
            inpt4.Attributes["placeholder"] = "Country";
            inpt4.ID = "inpt4"+obj._id;
            inpt4.Attributes["class"] = "input";
            inpt4.Attributes["value"] = obj.Country.ToString();

            Label lbl5 = new Label();
            lbl5.Attributes["class"] = "lblDetails";
            lbl5.Text = "Sector";
            HtmlGenericControl inpt5 = new HtmlGenericControl("input");
            inpt5.Attributes["type"] = "text";
            inpt5.Attributes["placeholder"] = "Sector";
            inpt5.ID = "inpt5" + obj._id;
            inpt5.Attributes["class"] = "input";
            inpt5.Attributes["value"] = obj.Sector.ToString();

            Label lbl6 = new Label();
            lbl6.Attributes["class"] = "lblDetails";
            lbl6.Text = "Ticker";
            HtmlGenericControl inpt6 = new HtmlGenericControl("input");
            inpt6.Attributes["type"] = "text";
            inpt6.Attributes["placeholder"] = "Ticker";
            inpt6.ID = "inpt6" + obj._id;
            inpt6.Attributes["class"] = "input";
            inpt6.Attributes["value"] = obj.Ticker.ToString();

            Table tbl = new Table();
            TableRow row1 = new TableRow();
            TableCell cell11 = new TableCell();
            cell11.Controls.Add(lbl1);
            row1.Controls.Add(cell11);
            TableCell cell12 = new TableCell();
            cell12.Controls.Add(lbl2);
            row1.Controls.Add(cell12);
            TableCell cell13 = new TableCell();
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


            tbl.Controls.Add(row1);

            TableRow row2 = new TableRow();
            TableCell cell21 = new TableCell();
            cell21.Controls.Add(inpt1);
            row2.Controls.Add(cell21);
            TableCell cell22 = new TableCell();
            cell22.Controls.Add(inpt2);
            row2.Controls.Add(cell22);
            TableCell cell23 = new TableCell();
            //cell23.Controls.Add(inpt3);
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

            tbl.Controls.Add(row2);

            tbl.Attributes["class"] = "tableInpt";
            //tbl.ID = "tableInpt";
            return tbl;
        }



    }
}