using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionQueue
{
    public class Names
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Name { get; set; }
    }
}
