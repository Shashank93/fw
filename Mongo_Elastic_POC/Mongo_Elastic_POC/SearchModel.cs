﻿using MongoDB.Bson.Serialization.Attributes;
using Nest;
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
        
        [PropertyName("username")]
        public string UserName { get; set; }
        [PropertyName("selectedgroup")]
        public string SelectedGroup { get; set; }
        [PropertyName("expcreateddate")]
        public string ExpCreatedDate { get; set; }
        [PropertyName("expcreatedtime")]
        public string ExpCreatedTime { get; set; }
        [PropertyName("experimentid")]
        public string ExpId { get; set; }
        [Nested]
        [PropertyName("userdefinedfields")]
        public List<UserDefinedField> UserDefinedFields { get; set; }
        

    }
}
