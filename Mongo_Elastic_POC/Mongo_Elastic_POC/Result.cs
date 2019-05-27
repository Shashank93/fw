using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo_Elastic_POC
{
    public class Result
    {
        public List<string> ExperimentIds { get; set; }
        public long TimeTaken { get; set; }
    }
}
