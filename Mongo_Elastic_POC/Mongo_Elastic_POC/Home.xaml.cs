using System;
using System.Collections.Generic;
using System.Globalization;
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
            SearchDb.SetupElasticClient();
            SearchDb.SetupMongoClient();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            

            //for exact match strings  ---MONGO DB
            Dictionary<string, string> searchStrings = new Dictionary<string, string>();
            //searchStrings.Add("username", "bad99954");
            //searchStrings.Add("selectedgroup", "D");

            //for like strings
            Dictionary<string, string> likeStrings = new Dictionary<string, string>();
            //likeStrings.Add("expcreateddate", "2019-05-24");
            //likeStrings.Add("expcreatedtime", "69.14:");

            //for tag strings
            Dictionary<string, string> tagStrings = new Dictionary<string, string>();
            //tagStrings.Add("tag1099954", "SPUNGOB");

            //MANDATORY FIELDS
            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                searchStrings.Add("username", txtUserName.Text);
            }
            if (!string.IsNullOrEmpty(txtGroup.Text))
            {
                searchStrings.Add("selectedgroup", txtGroup.Text);
            }

            //LIKE STRINGS
            if (dpCreatedDate.SelectedDate != null)
            {
                //TODO: GENERIC DATE IMPLEMENTATION NEEDED 
                var slDate = dpCreatedDate.SelectedDate.Value;
                string month = slDate.Month.ToString();
                string day = slDate.Day.ToString();
                if(slDate.Month.ToString().Length == 1)
                {
                    month = "0" + slDate.Month;
                }
                if (slDate.Day.ToString().Length == 1)
                {
                    day = "0" + slDate.Day;
                }
                string dtStr = slDate.Year + "-" + month + "-" + day;
                likeStrings.Add("expcreateddate", dtStr);
            }
            if (!string.IsNullOrEmpty(txtTime.Text))
            {
                likeStrings.Add("expcreatedtime", txtTime.Text);
            }

            //TAG - USER DEF FIELDS STRINGS
            if ((!string.IsNullOrEmpty(txtField1.Text) && !string.IsNullOrEmpty(txtField1Value.Text)))
            {
                tagStrings.Add(txtField1.Text, txtField1Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField2.Text) && !string.IsNullOrEmpty(txtField2Value.Text)))
            {
                tagStrings.Add(txtField2.Text, txtField2Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField3.Text) && !string.IsNullOrEmpty(txtField3Value.Text)))
            {
                tagStrings.Add(txtField3.Text, txtField3Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField4.Text) && !string.IsNullOrEmpty(txtField4Value.Text)))
            {
                tagStrings.Add(txtField4.Text, txtField4Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField5.Text) && !string.IsNullOrEmpty(txtField5Value.Text)))
            {
                tagStrings.Add(txtField5.Text, txtField5Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField6.Text) && !string.IsNullOrEmpty(txtField6Value.Text)))
            {
                tagStrings.Add(txtField6.Text, txtField6Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField7.Text) && !string.IsNullOrEmpty(txtField7Value.Text)))
            {
                tagStrings.Add(txtField7.Text, txtField7Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField8.Text) && !string.IsNullOrEmpty(txtField8Value.Text)))
            {
                tagStrings.Add(txtField8.Text, txtField8Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField9.Text) && !string.IsNullOrEmpty(txtField9Value.Text)))
            {
                tagStrings.Add(txtField9.Text, txtField9Value.Text);
            }
            if ((!string.IsNullOrEmpty(txtField10.Text) && !string.IsNullOrEmpty(txtField10Value.Text)))
            {
                tagStrings.Add(txtField10.Text, txtField10Value.Text);
            }

            var resultsElastic = SearchDb.ElasticSearch(searchStrings, likeStrings, tagStrings);
            var resultsMongo = SearchDb.MongoSearch(searchStrings, likeStrings, tagStrings);

            lstMongoBoxResults.ItemsSource = resultsMongo.ExperimentIds;
            lstElasticBoxResults.ItemsSource = resultsElastic.ExperimentIds;
            lblElSrTime.Content = resultsElastic.TimeTaken + " ms";
            lblMgSrTime.Content = resultsMongo.TimeTaken + " ms";
            lbrElRecCount.Content = resultsElastic.ExperimentIds.Count + " records";
            lblMgRecCount.Content = resultsMongo.ExperimentIds.Count + " records";
        }

        private void BtnSearchAll_Click(object sender, RoutedEventArgs e)
        {
            var searchText = txtSearchAll.Text;
            SearchDb.ElasticSearchAll(searchText);
        }
    }
}
