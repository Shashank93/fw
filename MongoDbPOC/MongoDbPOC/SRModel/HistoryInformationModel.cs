using Newtonsoft.Json;

namespace MongoDbPOC.SRModel
{
    public class HistoryInformationModel
    {

        /* 
            {
              "created": "1/1/2019",
              "loaded": "1/2/2019",
              "stored": "1/3/2019",
              "deleted": "1/10/2019",
              "datasetid": 1
            }
        */

        [JsonProperty("datasetid")]
        public string DatasetId { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("loaded")]
        public string Loaded { get; set; }

        [JsonProperty("stored")]
        public string Stored { get; set; }

        [JsonProperty("deleted")]
        public string Deleted { get; set; }
    }
}
