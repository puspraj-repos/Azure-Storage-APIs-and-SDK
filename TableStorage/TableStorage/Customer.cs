using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableStorage
{
    class Customer : TableEntity
    {
        public string CustomerName { get; set; }
        public Customer(string customer_name, string city, string id)
        {
            PartitionKey = city;
            RowKey = id;
            CustomerName = customer_name;
        }
        public Customer()
        {

        }
    }
}
