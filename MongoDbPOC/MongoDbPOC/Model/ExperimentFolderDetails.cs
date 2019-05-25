using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbPOC.Model
{
    public class ExperimentFolderDetails
    {
        public int experimentFolderId { get; set; }
        public string experimentFolderName { get; set; }
        public string path { get; set; }
        public string status { get; set; }
        public DateTime? createdTimeStamp { get; set; }
        public string folderSize { get; set; }
        
    }
}
