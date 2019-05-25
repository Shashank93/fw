using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo_Elastic_POC
{
    [BsonIgnoreExtraElements]
    public class SearchModel
    {
        public string UserName { get; set; }
        public string SelectedGroup { get; set; }
        public string ExpCreatedDate { get; set; }
        public string ExpCreatedTime { get; set; }
        public string ExpId { get; set; }

        public UserDefinedField Field1 { get; set; }
        public UserDefinedField Field2 { get; set; }
        public UserDefinedField Field3 { get; set; }
        public UserDefinedField Field4 { get; set; }
        public UserDefinedField Field5 { get; set; }
        public UserDefinedField Field6 { get; set; }
        public UserDefinedField Field7 { get; set; }
        public UserDefinedField Field8 { get; set; }
        public UserDefinedField Field9 { get; set; }
        public UserDefinedField Field10 { get; set; }

    }
}
