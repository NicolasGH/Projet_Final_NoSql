using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplicationMongoDB.View
{
    public partial class index : System.Web.UI.Page
    {
        protected WebApplicationMongoDB.Controller.ElasticsearchClient elastic = new Controller.ElasticsearchClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            int i=1;
            foreach (WebApplicationMongoDB.Controller.ElasticsearchClient.Stockobject obj in elastic.findAll())
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
            
            content.Controls.Add(elastic.createInputs(elastic.findAll().ToList()[0]));
            content.Controls.Add(btnS);
            content.Controls.Add(btnB);
            
            
        }

        protected void searchfct(object sender, EventArgs e)
        {
            //containerStock.Controls.Remove(pnlP);
            int i = 1;
            foreach (WebApplicationMongoDB.Controller.ElasticsearchClient.Stockobject obj in elastic.NavBar(srchIpt.Value))
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

    }
}