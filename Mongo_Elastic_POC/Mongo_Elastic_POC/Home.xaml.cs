using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mongo_Elastic_POC
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();

            //SearchDb.SetupElasticClient();
            //SearchDb.SetupMongoClient();

            ////for exact match strings  ---MONGO DB
            //Dictionary<string, string> searchStrings = new Dictionary<string, string>();
            //searchStrings.Add("username", "bad99954");
            //searchStrings.Add("selectedgroup", "D");

            ////for like strings
            //Dictionary<string, string> likeStrings = new Dictionary<string, string>();
            //likeStrings.Add("expcreateddate", "2019-05-24");
            //likeStrings.Add("expcreatedtime", "69.14:");


            ////for tag strings
            //Dictionary<string, string> tagStrings = new Dictionary<string, string>();
            //tagStrings.Add("tag1099954", "SPUNGOB");

            //var resultsElastic = SearchDb.ElasticSearch(searchStrings, likeStrings, tagStrings);
            //var resultsMongo = SearchDb.MongoSearch(searchStrings, likeStrings, tagStrings);




        }
    }
}
