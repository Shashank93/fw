using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo_Elastic_POC
{
    public abstract class Document
    {
        public JoinField Join { get; set; }
    }
}
