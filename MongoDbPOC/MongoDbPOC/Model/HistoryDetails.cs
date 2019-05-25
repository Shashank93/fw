using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPOC.Model
{
    public class HistoryDetails
    {
        public int historyId { get; set; }
        public DateTime? createdTimeStamp { get; set; }
        public UserDetails userDetails { get; set; }
        public string action { get; set; }
        public bool isActive { get; set; }
    }
}
