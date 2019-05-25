using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo_Elastic_POC
{
    [BsonIgnoreExtraElements]
    public class UserDefinedField
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
