using Newtonsoft.Json;

namespace MongoDbPOC.SRModel
{
    public class DataSetModel
    {
        /* 
            {
                "BaseInformationDataId": "1",
                "TemplateDataId": "1",
                "FilesListId": "1",
                "ExpId": "1",
                "ExpName": "Frog Analysis"
            }
        */

        [JsonProperty("BaseInformationDataId")]
        public string BaseInformationDataId { get; set; }

        [JsonProperty("TemplateDataId")]
        public string TemplateDataId { get; set; }

        [JsonProperty("FilesListId")]
        public string FilesListId { get; set; }

        [JsonProperty("ExpId")]
        public string ExpId { get; set; }

        [JsonProperty("ExpName")]
        public string ExpName { get; set; }

    }
}
