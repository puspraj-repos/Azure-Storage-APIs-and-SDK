using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json.Linq;

namespace AzureFunctionQueue
{
    public static class QueueFunction
    {
        [FunctionName("QueueFunction")]
        [return: Table("testtable",Connection = "connection-string")]
        public static Names Run([QueueTrigger("queue1", Connection = "connection-string")]JObject myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            Names obj = new Names();
            obj.Name = myQueueItem["Name"].ToString();
            obj.PartitionKey = myQueueItem["PartitionKey"].ToString();
            obj.RowKey = myQueueItem["RowKey"].ToString();
            return (obj);
            //CloudStorageAccount _account = CloudStorageAccount.Parse()
        }
    }
}
