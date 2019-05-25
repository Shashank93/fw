using Elasticsearch.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mongo_Elastic_POC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //FeedData();
            //var externalDataMongoToElasticTransfer = GetAllExternalDataFromMongo();
            //PostExternalDataToElastic(externalDataMongoToElasticTransfer);

            //for exact match strings
            Dictionary<string, string> searchStrings = new Dictionary<string, string>();
            searchStrings.Add("username", "bad99954");
            searchStrings.Add("selectedgroup", "D");

            //for like strings
            Dictionary<string, string> likeStrings = new Dictionary<string, string>();
            likeStrings.Add("expcreateddate", "2019-05-24");
            likeStrings.Add("expcreatedtime", "69.14:");


            //for tag strings
            Dictionary<string, string> tagStrings = new Dictionary<string, string>();
            tagStrings.Add("tags.tag1099954", "SPUNGOB");


            SearchData(searchStrings, likeStrings, tagStrings);


        }

        public List<dynamic> GetAllExternalDataFromMongo()
        {
            const string connectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(connectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("LASXdb");

            var fields = Builders<dynamic>.Projection.Exclude("_id");
            //get mongodb collection
            var collection = database.GetCollection<dynamic>("ExternalData");
            var documents = collection.Find(_ => true).Project<dynamic>(fields).ToListAsync();
            return documents.Result;
        }

        /// <summary>
        /// MAIN METHOD FOR EXPERIMENT
        /// </summary>
        public void SearchData(Dictionary<string, string> searchStrings, Dictionary<string, string> likeStrings, Dictionary<string, string> tagStrings)
        {
            //TEST MONGO CLIENT SEARCH
            const string connectionString = "mongodb://localhost:27017";
            // Create a MongoClient object by using the connection string
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("LASXdb");
            BsonClassMap.RegisterClassMap<dynamic>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });


            List<BsonElement> lstBsonCriteriaQuery = new List<BsonElement>();

            //ADD FILTERS
            foreach (var srchStr in searchStrings)
            {
                BsonElement bsonCrt1 = new BsonElement(srchStr.Key, srchStr.Value);
                lstBsonCriteriaQuery.Add(bsonCrt1);
            }

            foreach (var lkString in likeStrings)
            {
                BsonElement bsonCrt3 = new BsonElement(lkString.Key, new BsonRegularExpression(string.Format("^{0}", lkString.Value)));
                lstBsonCriteriaQuery.Add(bsonCrt3);
            }

            foreach (var tgString in tagStrings)
            {
                BsonElement bsonStr = new BsonElement(tgString.Key, new BsonRegularExpression(string.Format("^{0}", tgString.Value)));
                lstBsonCriteriaQuery.Add(bsonStr);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var collection = database.GetCollection<BsonDocument>("ExternalData");
            //QUERY PREP WITH FILTERS
            BsonDocument findData = new BsonDocument(lstBsonCriteriaQuery);

            //DEFINE FIELDS TO BE FETCHED
            var fields = Builders<BsonDocument>.Projection.Include("expid");

            //RESULT
            var resultDoc = collection.Find(findData).Project<BsonDocument>(fields).ToList();

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;


            //TEST ELASTIC CLIENT



            
            
            var watch1 = System.Diagnostics.Stopwatch.StartNew();
            

            //TEST ELASTICSEARCH CLIENT SEARCH
          
            var elasticClient = new ElasticClient();
            var searchResponse = elasticClient.Search<SearchModel>(s => s
            .Index("searchdb")
            .Query(q => q
                .Match(m => m
                    .Field(f => f.UserName)
                    .Query("bad99954")
                )
            )
        );
            
            var elasticResult = searchResponse.Documents;
            watch1.Stop();
            var elapsedMs1 = watch1.ElapsedMilliseconds;
        }
        
        public static string RandomString(int length)
        {
            //length = length < 0 ? length * -1 : length;
            var str = "";

            do
            {
                str += Guid.NewGuid().ToString().Replace("-", "");
            }

            while (length > str.Length);

            return str.Substring(0, length);
        }

        public void FeedExternalDataExecute(List<dynamic> lstExternalData)
        {
            const string connectionString = "mongodb://localhost:27017";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(connectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("SearchExternalDb");

            //get mongodb collection
            var collection = database.GetCollection<dynamic>("SearchExternalData");


            //INSERTS DATA TO ELASTICSEARCH
            PostExternalDataToElastic(lstExternalData);
            //INSERTS DATA TO MONGODB
            collection.InsertMany(lstExternalData);

        }

        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }


        public void PostExternalDataToElastic(List<dynamic> srcDta)
        {
            var EsClient = new ElasticClient();
            
            List<SearchModel> lstElstModel = new List<SearchModel>();
            //CREATE MODEL FOR ELASTIC DATA 
            foreach (var data in srcDta)
            {


                SearchModel srcModel = new SearchModel();
                //Dictionary<string, string> dctUsrDefFields = new Dictionary<string, string>();
                foreach (KeyValuePair<string, object> kvp in data)
                {
                    string propertyName = kvp.Key;
                    var value = kvp.Value;
                    if (propertyName.ToLower() == "username")
                    {
                        srcModel.UserName = Convert.ToString(value);
                    }
                    else if (propertyName.ToLower() == "selectedgroup")
                    {
                        srcModel.SelectedGroup = Convert.ToString(value);
                    }
                    else if (propertyName.ToLower() == "expcreateddate")
                    {
                        srcModel.ExpCreatedDate = Convert.ToString(value);
                    }
                    else if (propertyName.ToLower() == "expcreatedtime")
                    {
                        srcModel.ExpCreatedTime = Convert.ToString(value);
                    }
                    else if (propertyName.ToLower() == "expid")
                    {
                        srcModel.ExpId = Convert.ToString(value);
                    }
                    else if (propertyName.ToLower() == "tags")
                    {
                        dynamic nestedUsrFields = value;
                        string proName = "Field";
                        int propSerial = 1;
                        Type type = typeof(SearchModel);
                        try
                        {
                            foreach (KeyValuePair<string, object> nestedData in nestedUsrFields)
                            {
                                string propName = proName + propSerial;
                                string nestedPropertyName = nestedData.Key;
                                string nestedValue = Convert.ToString(nestedData.Value);
                                UserDefinedField usrFld = new UserDefinedField();
                                usrFld.Name = nestedPropertyName;
                                usrFld.Value = nestedValue;
                                PropertyInfo piInstance = type.GetProperty(propName);
                                piInstance.SetValue(srcModel, usrFld);
                                propSerial++;
                                //dctUsrDefFields.Add(nestedPropertyName, Convert.ToString(nestedValue));
                            }
                        }
                        catch (Exception)
                        {
                            //to be logged
                        }
                    }
                }

                lstElstModel.Add(srcModel);
            }


            //can cancel the operation by calling .Cancel() on this
            var cancellationTokenSource = new CancellationTokenSource();

            // set up the bulk all observable
            var bulkAllObservable = EsClient.BulkAll(lstElstModel, ba => ba
                // number of concurrent requests
                .MaxDegreeOfParallelism(8)
                // in case of 429 response, how long we should wait before retrying
                .BackOffTime(TimeSpan.FromSeconds(5))
                // in case of 429 response, how many times to retry before failing
                .BackOffRetries(2)
                // number of documents to send in each request
                .Size(500)
                .Index("searchdb")
                .RefreshOnCompleted(),
                cancellationTokenSource.Token
            );

            var waitHandle = new ManualResetEvent(false);
            Exception ex = null;

            // what to do on each call, when an exception is thrown, and 
            // when the bulk all completes
            var bulkAllObserver = new BulkAllObserver(
                onNext: bulkAllResponse =>
                {
                    // do something after each bulk request
                },
                onError: exception =>
                {
                    // do something with exception thrown
                    ex = exception;
                    waitHandle.Set();
                },
                onCompleted: () =>
                {
                    // do something when all bulk operations complete
                    waitHandle.Set();
                });

            bulkAllObservable.Subscribe(bulkAllObserver);

            // wait for handle to be set.
            waitHandle.WaitOne();

            if (ex != null)
            {
                throw ex;
            }


        }

        public void InsertElasticDataFromMongo(List<dynamic> srcDta)
        {
            var node = new Uri("http://localhost:9200");
            var config = new ConnectionConfiguration(node);
            var client = new ElasticLowLevelClient(config);

            foreach (var data in srcDta)
            {
                /*var response = client.Index("ëxternaldata","experimentdata",);*/ //or specify index via settings.DefaultIndex("mytweetindex");

            }






        }
    }
}


