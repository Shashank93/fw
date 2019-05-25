using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPOC.Model
{
    public class Templates
    {
        public int templateId { get; set; }
        public GroupDetails templateGroup { get; set; }
        public UserDetails createdBy { get; set; }
        public DateTime? createdTimeStamp { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public List<UserDefinedFields> fields { get; set; }

    }
}
