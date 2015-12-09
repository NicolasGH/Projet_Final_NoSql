using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplicationMongoDB.Controller;

namespace WebApplicationMongoDB.View
{
    public partial class index : System.Web.UI.Page
    {
        protected WebApplicationMongoDB.Controller.ElasticsearchClient elastic = new Controller.ElasticsearchClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            MongoDBClient mdc = new MongoDBClient("stockcollection");
            //mdc.LoadLocalData(@"Z:\Downloads\stocks-2.json");
            mdc.mapReduceMarketCapCountry();
            int i=1;
            foreach (WebApplicationMongoDB.Stockobject obj in elastic.findAll())
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
                
                if(srchIpt.Value=="")
                {
                    //containerStock.Controls.Add(elastic.createPopUp(obj));
                    containerStock.Controls.Add(pnlP);
                }
                i++;
            }
            Button btnB = new Button();
            btnB.Attributes["class"] = "btn-popup";
            btnB.Text = "Update";
            Button btnS = new Button();
            btnS.Attributes["class"] = "btn-popup";
            btnS.Text = "Delete";
            ModalPopupExtender1.TargetControlID = "btn1";
            
            content.Controls.Add(createInputs(elastic.findAll().ToList()[0]));
            content.Controls.Add(btnS);
            content.Controls.Add(btnB);
            
            
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

    }
}