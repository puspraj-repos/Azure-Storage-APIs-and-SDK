using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace TableStorage
{
    class Program
    {
        private static string _connection_string = "Storage_account_connection_string";
        private static string _table_name = "testtable";
        static void Main(string[] args)
        {
            CloudStorageAccount _account = CloudStorageAccount.Parse(_connection_string);
            CloudTableClient _table_client = _account.CreateCloudTableClient();
            CloudTable _table = _table_client.GetTableReference(_table_name);
            _table.CreateIfNotExists();
            Console.WriteLine("Created table if it was not there previously");

            addItem();
            addItemsBatch();
            readItem();
            updateItem();
            Console.ReadKey();
        }
        public static void addItem()
        {
            CloudStorageAccount _account = CloudStorageAccount.Parse(_connection_string);
            CloudTableClient _table_client = _account.CreateCloudTableClient();
            CloudTable _table = _table_client.GetTableReference(_table_name);

            Customer obj = new Customer("Puspraj", "Indore", "1");
            TableOperation _operation = TableOperation.Insert(obj);
            TableResult _result = _table.Execute(_operation);
        }
        public static void addItemsBatch()
        {
            CloudStorageAccount _account = CloudStorageAccount.Parse(_connection_string);
            CloudTableClient _client = _account.CreateCloudTableClient();
            CloudTable _table = _client.GetTableReference(_table_name);

            List<Customer> customers = new List<Customer>()
            {
                new Customer("Pankaj", "Rewa", "1"),
                new Customer("Neeraj", "Rewa", "2")
            };

            TableBatchOperation _batch_operation = new TableBatchOperation();

            foreach(Customer item in customers)
            {
                _batch_operation.Insert(item);
            }
            TableBatchResult _batch_result = _table.ExecuteBatch(_batch_operation);
        }
        public static void readItem()
        {
            CloudStorageAccount _account = CloudStorageAccount.Parse(_connection_string);
            CloudTableClient _client = _account.CreateCloudTableClient();
            CloudTable _table = _client.GetTableReference(_table_name);
            TableOperation _operation = TableOperation.Retrieve<Customer>("Indore", "1");
            TableResult _result = _table.Execute(_operation);
            Customer customer = _result.Result as Customer;
        }
        public static void updateItem()
        {
            CloudStorageAccount _account = CloudStorageAccount.Parse(_connection_string);
            CloudTableClient _table = _account.CreateCloudTableClient();
            CloudTable _client = _table.GetTableReference(_table_name);
            Customer obj = new Customer("Pooja", "Indore", "1");
            TableOperation _operation = TableOperation.InsertOrMerge(obj);
            TableResult result = _client.Execute(_operation);
        }
    }
}
