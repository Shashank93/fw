using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo_Elastic_POC
{
    public static class SearchDb
    {

        public static ConnectionSettings EsConfig;
        public static ElasticClient EsClient;

        public static MongoClient MgClient;
        public static IMongoDatabase MgDb;
        const string connectionString = "mongodb://localhost:27017";

        public static void SetupElasticClient()
        {
            if (EsClient == null)
            {
                EsConfig = new ConnectionSettings()
               .DefaultIndex("searchdb")
               .DefaultMappingFor<SearchModel>(m => m
                   .IndexName("searchdb")
               );
                EsClient = new ElasticClient(EsConfig);
            }
        }

        public static void SetupMongoClient()
        {
            if (MgClient == null)
            {
                // Create a MongoClient object by using the connection string
                MgClient = new MongoClient(connectionString);
                MgDb = MgClient.GetDatabase("LASXdb");
                BsonClassMap.RegisterClassMap<dynamic>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }

        }

        public static void SearchData(Dictionary<string, string> searchStrings, Dictionary<string, string> likeStrings, Dictionary<string, string> tagStrings)
        {
            var mgResult = MongoSearch(searchStrings, likeStrings, tagStrings);
            var elResult = ElasticSearch(searchStrings, likeStrings, tagStrings);
        }

        public static Result ElasticSearch(Dictionary<string, string> searchStrings, Dictionary<string, string> likeStrings, Dictionary<string, string> tagStrings)
        {
            //TEST ELASTIC CLIENT
            //ADD FILTERS
            List<QueryContainer> lstSearchFieldQuery = new List<QueryContainer>();

            QueryContainer searchQueryBulder = null;
            foreach (var srchStr in searchStrings)
            {
                var qb1 = new TermQuery
                {
                    Field = srchStr.Key.ToLower(),
                    Value = srchStr.Value.ToLower()

                };

                searchQueryBulder &= qb1;

            }

            foreach (var tgStr in tagStrings)
            {
                QueryContainer nestQuery = null;
                nestQuery &= new WildcardQuery
                {
                    Field = "userdefinedfields.name",
                    Value = "*" + tgStr.Key.ToLower() + "*"

                };

                nestQuery &= new WildcardQuery
                {
                    Field = "userdefinedfields.value",
                    Value = "*" + tgStr.Value.ToLower() + "*"
                };

                searchQueryBulder &= new NestedQuery
                {
                    Path = "userdefinedfields",
                    Query = nestQuery,
                    IgnoreUnmapped = true
                };

            }

            foreach (var lkStr in likeStrings)
            {
                searchQueryBulder &= new MatchQuery
                {
                    Field = lkStr.Key.ToLower(),
                    Query = lkStr.Value.ToLower()
                };
            }


            //check json request data
            //var req = new SearchRequest<SearchModel>
            //{
            //    Query = searchQueryBulder
            //};

            //using (var ms = new MemoryStream())
            //{
            //    elasticClient.SourceSerializer.Serialize(req, ms, Elasticsearch.Net.SerializationFormatting.Indented);
            //    string jsonQuery = Encoding.UTF8.GetString(ms.ToArray());
            //};

            var watch1 = System.Diagnostics.Stopwatch.StartNew();

            var searchResponse = EsClient.Search<SearchModel>(new SearchRequest<SearchModel>
            {
                Source = new SourceFilter
                {
                    Includes = "experimentid"
                },
                Query = searchQueryBulder
            });
            //using the object initializer syntax
            watch1.Stop();
            var elapsedMs1 = watch1.ElapsedMilliseconds;
            var rslt = new List<string>();
            foreach (var fieldValues in searchResponse.Documents)
            {
                rslt.Add(fieldValues.ExpId);
            }

            Result rst = new Result();
            rst.TimeTaken = elapsedMs1;
            rst.ExperimentIds = rslt;
            return rst;
        }

        public static Result MongoSearch(Dictionary<string, string> searchStrings, Dictionary<string, string> likeStrings, Dictionary<string, string> tagStrings)
        {
            //TEST MONGO CLIENT SEARCH



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
                BsonElement bsonStr = new BsonElement("tags." + tgString.Key, new BsonRegularExpression(string.Format("^{0}", tgString.Value)));
                lstBsonCriteriaQuery.Add(bsonStr);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var collection = MgDb.GetCollection<BsonDocument>("ExternalData");
            //QUERY PREP WITH FILTERS
            BsonDocument findData = new BsonDocument(lstBsonCriteriaQuery);

            //DEFINE FIELDS TO BE FETCHED
            var fields = Builders<BsonDocument>.Projection.Include("expid");

            //RESULT
            var resultDoc = collection.Find(findData).Project<BsonDocument>(fields).ToList();

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            var rslt = new List<string>();
            foreach (var fieldValue in resultDoc)
            {
                rslt.Add(fieldValue.GetValue("expid").AsString);
            }
            Result rst = new Result();
            rst.TimeTaken = elapsedMs;
            rst.ExperimentIds = rslt;
            return rst;
        }
    }
}
