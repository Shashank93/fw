using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPOC.Model
{
    public class SoftwareDetails
    {
        public int softwareDetailsId { get; set; }
        public string manufacturerName { get; set; }
        public int manufacturerId { get; set; }
        public string OriginalSoftwareName { get; set; }
        public string OriginalSoftwareVersion { get; set; }
        public int compatibleSoftwareId { get; set; }
        public string compatibleSoftwareName { get; set; }
    }
}
