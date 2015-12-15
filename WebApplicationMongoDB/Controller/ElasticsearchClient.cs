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
            this.connexion = new Uri("http://192.168.1.14:9200");
           // this.connexion = new Uri("http://localhost:9200");
            this.settings = new ConnectionSettings(connexion, defaultIndex: "tp");
            this.client = new ElasticClient(settings);
            client.CreateIndex("tp");
            var mapResult = client.Map<Stockobject>(c => c.MapFromAttributes().IgnoreConflicts().Type("stocks").Indices("tp"));
        }


        public List<Stockobject> findAll()
        {
            try
            {
                var searchResults = client.Search<Stockobject>(s => s
                    .AllIndices()
                    .Size(5)
                    );
                return searchResults.Documents.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

    

        public List<Stockobject> SearchRequestToES(String _textToSearch)
        {
            try
            {
                var searchResults = client.Search<Stockobject>(s => s
                    .AllIndices()
                    .Size(10)
                    .Query(qs => qs.QueryString(q => q.Query(_textToSearch))));

                return searchResults.Documents.ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}