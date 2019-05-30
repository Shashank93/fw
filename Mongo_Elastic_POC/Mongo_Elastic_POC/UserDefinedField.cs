using MongoDB.Bson.Serialization.Attributes;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo_Elastic_POC
{
    [BsonIgnoreExtraElements]
    public class UserDefinedField: Document
    {
        [Text(Name = "name")]
        public string Name { get; set; }
        [Text(Name = "value")]
        public string Value { get; set; }
        [Nested]
        [PropertyName("todoitems")]
        public List<UserDefinedField> ToDoItems { get; set; }
    }
}
