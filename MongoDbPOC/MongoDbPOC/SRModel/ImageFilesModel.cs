using Newtonsoft.Json;

namespace MongoDbPOC.SRModel
{
    public class ImageFilesModel
    {
        /* 
             {
                "ExpId": "1",
                "ExpName": "Frog Analysis",
                "FolderPath": "D:\temp",
                "Size": "50GB"
            }
        */

        [JsonProperty("ExpId")]
        public string ExpId { get; set; }

        [JsonProperty("ExpName")]
        public string ExpName { get; set; }

        [JsonProperty("FolderPath")]
        public string FolderPath { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }
    }
}
