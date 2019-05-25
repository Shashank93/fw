using Newtonsoft.Json;

namespace MongoDbPOC.SRModel
{
    public class BaseInformationDataModel
    {
        /* 
         * {
              "TemplateId": "1",
              "UserName": "spai1",
              "SelectedGroup": "A",
              "Type": "Confocal",
              "SwManufacturer": "Leica",
              "OriginalSW": "LMS",
              "VersionOriginalSW": "1.0",
              "CompatibleSW": "1",
              "VersionCompatibleSW": "2.0",
              "Date": "03-05-2019 10:18:51",
              "Time": "10:18:51.2917800"
            }
        */

        [JsonProperty("TemplateId")]
        public string TemplateId { get; set;}

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("SelectedGroup")]
        public string SelectedGroup { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("SwManufacturer")]
        public string SwManufacturer { get; set; }

        [JsonProperty("OriginalSW")]
        public string OriginalSW { get; set; }

        [JsonProperty("VersionOriginalSW")]
        public string VersionOriginalSW { get; set; }

        [JsonProperty("CompatibleSW")]
        public string CompatibleSW { get; set; }

        [JsonProperty("VersionCompatibleSW")]
        public string VersionCompatibleSW { get; set; }

        [JsonProperty("Date")]
        public string CreatedDate { get; set; }

        [JsonProperty("Time")]
        public string CreatedTime { get; set; }

        //[JsonProperty("id")]
        //public string Id { get; set; }
    }
}
