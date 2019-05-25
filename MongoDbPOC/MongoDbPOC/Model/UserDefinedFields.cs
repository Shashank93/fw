using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPOC.Model
{
    public class UserDefinedFields
    {
        public int udfId { get; set; }
        public string fieldName { get; set; }
        public string fieldValue { get; set; }
        public string fieldDataType { get; set; }
        public UserDetails createdBy { get; set; }
        public DateTime? createdTimeStamp { get; set; }
        public bool isActive { get; set; }
    }
}
