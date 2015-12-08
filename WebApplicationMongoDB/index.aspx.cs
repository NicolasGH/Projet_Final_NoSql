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
        protected WebApplicationMongoDB.Controller.ElasticsearchClient elastic = new Controller.ElasticsearchClient();
        protected void Page_Load(object sender, EventArgs e)
        {

            foreach (WebApplicationMongoDB.Controller.ElasticsearchClient.Stockobject obj in elastic.findAll())
            {
                Panel pnlC = new Panel();
                pnlC.Attributes["class"] = "panel-body";
                Panel pnlP = new Panel();
                pnlP.ID = "pnlP";
                pnlP.Attributes["class"] = "panel panel-default panelStock";
                Label lbl = new Label();
                lbl.Text = obj.Company;
                pnlC.Controls.Add(lbl);
                pnlP.Controls.Add(pnlC);
                if(srchIpt.Value=="")
                {
                    containerStock.Controls.Add(pnlP);
                }
               

            } 

        }

        protected void searchfct(object sender, EventArgs e)
        {
            //containerStock.Controls.Remove(pnlP);
            foreach (WebApplicationMongoDB.Controller.ElasticsearchClient.Stockobject obj in elastic.NavBar(srchIpt.Value))
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

        }

    }
}