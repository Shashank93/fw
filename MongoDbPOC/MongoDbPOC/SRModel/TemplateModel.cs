using Newtonsoft.Json;

namespace MongoDbPOC.SRModel
{
    public class TemplateModel
    {

        /*
          {
          "TemplateName": "BasicInformation",
          "CreatedBy": "SPAI",
          "GroupId": "A",
          "Date": "09-05-2019 10:18:51",
          "Time": "10:18:51.2917800",
          "UserDefinedTemplate": "No",
          "Field1": {
            "Name": "UserName",
            "DataType": "TEXT"
          },
          "Field2": {
            "Name": "SelectedGroup",
            "DataType": "TEXT"
          },
          "Field3": {
            "Name": "Type",
            "DataType": "TEXT"
          },
          "Field4": {
            "Name": "SwManufacturer",
            "DataType": "TEXT"
          },
          "Field5": {
            "Name": "OriginalSW",
            "DataType": "TEXT"
          },
          "Field6": {
            "Name": "Status",
            "DataType": "TEXT"
          },
          "Field7": {
            "Name": "VersionOriginalSW",
            "DataType": "NUMBER"
          },
          "Field8": {
            "Name": "CompatibleSW",
            "DataType": "TEXT"
          },
          "Field9": {
            "Name": "VersionCompatibleSW",
            "DataType": "TEXT"
          },
          "Field10": {
            "Name": "Departement3",
            "DataType": "TEXT"
          }
        }
      */

        [JsonProperty("TemplateName")]
        public string TemplateName { get; set; }

        [JsonProperty("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("GroupId")]
        public string GroupId { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("Time")]
        public string Time { get; set; }

        [JsonProperty("UserDefinedTemplate")]
        public string IsUserDefinedTemplate { get; set; }

        [JsonProperty("Field1")]
        public dynamic Field1 { get; set; }

        [JsonProperty("Field2")]
        public dynamic Field2 { get; set; }

        [JsonProperty("Field3")]
        public dynamic Field3 { get; set; }

        [JsonProperty("Field4")]
        public dynamic Field4 { get; set; }

        [JsonProperty("Field5")]
        public dynamic Field5 { get; set; }

        [JsonProperty("Field6")]
        public dynamic Field6 { get; set; }

        [JsonProperty("Field7")]
        public dynamic Field7 { get; set; }

        [JsonProperty("Field8")]
        public dynamic Field8 { get; set; }

        [JsonProperty("Field9")]
        public dynamic Field9 { get; set; }

        [JsonProperty("Field10")]
        public dynamic Field10 { get; set; }

    }
}
