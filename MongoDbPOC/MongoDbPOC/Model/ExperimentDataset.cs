using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPOC.Model
{
    public class ExperimentDataset
    {
        public string uniqueId { get; set; }
        public UserDetails userDetail { get; set; }
        public GroupDetails groupId { get; set; }
        public DateTime? createdTimeStamp { get; set; }
        public SoftwareDetails softwareDetailsId { get; set; }
        public UserDefinedFields userDefinedFieldsId { get; set; }
        public HistoryDetails historyId { get; set; }
        public string status { get; set; }
    }
}
