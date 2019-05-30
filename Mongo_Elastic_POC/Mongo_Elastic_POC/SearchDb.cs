using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static IMongoCollection<BsonDocument> mgCollection;
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
                MgDb = MgClient.GetDatabase("Mongo_Elastic_POC");
                BsonClassMap.RegisterClassMap<dynamic>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
                mgCollection = MgDb.GetCollection<BsonDocument>("ExternalData");
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
            QueryContainer searchQueryBulder = null;
            List<QueryContainer> lstSearchFieldQuery = new List<QueryContainer>();
            List<QueryContainer> lstFilterQuery = new List<QueryContainer>();

            foreach (var srchStr in searchStrings)
            {
                QueryContainer filterQueryBulder = new TermQuery
                {
                    Field = srchStr.Key,
                    Value = srchStr.Value

                };
                lstFilterQuery.Add(filterQueryBulder);

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

                searchQueryBulder&= new NestedQuery
                {
                    Path = "userdefinedfields",
                    Query = nestQuery,
                    IgnoreUnmapped = true
                };
               
            }

            foreach (var lkStr in likeStrings)
            {
                if (lkStr.Key.Equals("expcreateddate"))
                {
                    var qry = new DateRangeQuery
                    {
                        Field = lkStr.Key.ToLower(),
                        GreaterThanOrEqualTo = lkStr.Value.ToLower(),
                        LessThanOrEqualTo = lkStr.Value.ToLower(),
                        Format = "yyyy-MM-dd"
                    };
                    lstFilterQuery.Add(qry);
                }
                else
                {
                    searchQueryBulder &= new MatchQuery
                    {
                        Field = lkStr.Key.ToLower(),
                        Query = lkStr.Value.ToLower()
                    };
                }
                
            }

            lstSearchFieldQuery.Add(searchQueryBulder);

            var query = new BoolQuery()
            {
                Name = "named_query",
                Boost = 1.1,
                Must = lstFilterQuery,
                Should = lstSearchFieldQuery
            };


            //check json request data
<<<<<<< HEAD
            //var req = new SearchRequest<SearchModel>
            //{
            //    Query = searchQueryBulder
            //};

            //using (var ms = new MemoryStream())
            //{
            //    elasticClient.SourceSerializer.Serialize(req, ms, Elasticsearch.Net.SerializationFormatting.Indented);
            //    string jsonQuery = Encoding.UTF8.GetString(ms.ToArray());
            //};

            
            
=======
            var req = new SearchRequest<SearchModel>
            {
                From = 0,
                Size = 200000,
                Source = new SourceFilter
                {
                    Includes = "experimentid"
                },
                Query = query
            };

            using (var ms = new MemoryStream())
            {
                EsClient.SourceSerializer.Serialize(req, ms, Elasticsearch.Net.SerializationFormatting.Indented);
                string jsonQuery = Encoding.UTF8.GetString(ms.ToArray());
            };

>>>>>>> 5123f669218c1610d1f1da77674a140802aaf8cd
            var searchResponse = EsClient.Search<SearchModel>(new SearchRequest<SearchModel>
            {
                Size = 200000,
                Source = new SourceFilter
                {
                    Includes = "experimentid"
                },
                Query = query
            });
            //using the object initializer syntax
<<<<<<< HEAD
            
=======
           
>>>>>>> 5123f669218c1610d1f1da77674a140802aaf8cd
            var rslt = new List<string>();
            foreach (var fieldValues in searchResponse.Documents)
            {
                rslt.Add(fieldValues.ExpId);
            }

            Result rst = new Result();
<<<<<<< HEAD
            rst.TimeTaken = searchResponse.Took; 
=======
            rst.TimeTaken = searchResponse.Took;
>>>>>>> 5123f669218c1610d1f1da77674a140802aaf8cd
            rst.ExperimentIds = rslt;
            return rst;
        }

        public static Result MongoSearch(Dictionary<string, string> searchStrings, Dictionary<string, string> likeStrings, Dictionary<string, string> tagStrings)
        {
            //TEST MONGO CLIENT SEARCH


            var watch = System.Diagnostics.Stopwatch.StartNew();
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
                BsonElement bsonStr = new BsonElement("tags." + tgString.Key, new BsonRegularExpression(string.Format("/{0}/", tgString.Value)));
                lstBsonCriteriaQuery.Add(bsonStr);
            }

           

            //QUERY PREP WITH FILTERS
            BsonDocument findData = new BsonDocument(lstBsonCriteriaQuery);

            //DEFINE FIELDS TO BE FETCHED
            var fields = Builders<BsonDocument>.Projection.Include("expid");

            //RESULT
            var resultDoc = mgCollection.Find(findData).Project<BsonDocument>(fields).ToList();

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

        public static Result ElasticSearchAll(string searchPhrase)
        {
            //TEST ELASTIC CLIENT
            //ADD FILTERS



            var searchResponse = EsClient.Search<UserDefinedField>(sd => sd
           .Index("tagpreferences")
           .Size(2000000)
           .Query(q => q
               .Match(m => m.Field("value").Query(searchPhrase)
               )));


            //using the object initializer syntax

            var rslt = new List<string>();
            foreach (var fieldValues in searchResponse.Documents)
            {
                rslt.Add(fieldValues.Value);
                //rslt.Add(fieldValues.Value);
            }

            Result rst = new Result();
            rst.TimeTaken = searchResponse.Took;
            rst.ExperimentIds = rslt;
            return rst;
        }
    }
}
