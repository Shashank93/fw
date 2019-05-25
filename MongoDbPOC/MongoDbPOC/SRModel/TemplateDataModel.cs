using Newtonsoft.Json;

namespace MongoDbPOC.SRModel
{
    public class TemplateDataModel
    {
        /* 
         * {
              "TemplateId": "2",
              "ExpId": "1",
              "ExpName": "Frog Analysis",
              "field1": "Animalkingdom",
              "field2": "1521",
              "field3": "11",
              "field4": "tree-forg",
              "field5": "Pass",
              "field6": "Normal",
              "field7": "F71",
              "field8": "F81",
              "field9": "F91",
              "field10": "F101"
            }
        */

        [JsonProperty("TemplateId")]
        public string TemplateId { get; set; }
        
        [JsonProperty("ExpId")]
        public string ExpId { get; set; }

        [JsonProperty("ExpName")]
        public string ExpName { get; set; }

        [JsonProperty("Field1")]
        public string Field1 { get; set; }

        [JsonProperty("Field2")]
        public string Field2 { get; set; }

        [JsonProperty("Field3")]
        public string Field3 { get; set; }

        [JsonProperty("Field4")]
        public string Field4 { get; set; }

        [JsonProperty("Field5")]
        public string Field5 { get; set; }

        [JsonProperty("Field6")]
        public string Field6 { get; set; }

        [JsonProperty("Field7")]
        public string Field7 { get; set; }

        [JsonProperty("Field8")]
        public string Field8 { get; set; }

        [JsonProperty("Field9")]
        public string Field9 { get; set; }

        [JsonProperty("Field10")]
        public string Field10 { get; set; }
    }
}
