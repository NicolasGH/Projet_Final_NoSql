using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationMongoDB.View
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            WebApplicationMongoDB.Controller.ElasticsearchClient elastic = new Controller.ElasticsearchClient();
            System.Diagnostics.Debug.WriteLine("Hello");
            foreach (WebApplicationMongoDB.Controller.ElasticsearchClient.Stockobject obj in elastic.findAll())
            {
                Panel pnlC = new Panel();
                pnlC.Attributes["class"] = "panel-body";
                Panel pnlP = new Panel();
                pnlP.Attributes["class"] = "panel panel-default panelStock";
                Label lbl = new Label();
                lbl.Text = obj.Company;
                pnlC.Controls.Add(lbl);
                pnlP.Controls.Add(pnlC);
                containerStock.Controls.Add(pnlP);

            }
            //  elastic.LoadLocalData(@"Z:\Desktop\A4S7\Advanced topics in NoSql databases\TP Final\WebApplicationMongoDB\bin\stocks.json");


        }

    }
}