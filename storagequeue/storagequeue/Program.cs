using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using System;

namespace storagequeue
{
    class Program
    {
        private static string _connection_string = "Connection_string_for_storage_account";
        private static string _queue_name = "queue1";
        static void Main(string[] args)
        {
            addMessages();
            peekMessages();
            receiveMessage();
            addBase64encodedMessage();
            Console.ReadKey();
        }
        public static void addMessages()
        {
            QueueClient _client = new QueueClient(_connection_string, _queue_name);

            string message;
            if (_client.Exists())
            {
                for (int i = 0; i < 5; i++)
                {
                    message = $"This is message {i}";
                    _client.SendMessage(message);
                }
                Console.WriteLine("Message Added");
            }
            else
            {
                Console.WriteLine("Queue does not exists");
            }
        }
        public static void peekMessages()
        {
            QueueClient _client = new QueueClient(_connection_string, _queue_name);
            if(_client.Exists())
            {
                PeekedMessage[] _messages = _client.PeekMessages(2);
                foreach(PeekedMessage _message in _messages)
                {
                    Console.WriteLine(_message.Body.ToString());
                }
            }
        }
        public static void receiveMessage()
        {
            QueueClient _client = new QueueClient(_connection_string, _queue_name);
            if(_client.Exists())
            {
                QueueMessage _message =  _client.ReceiveMessage();
                Console.WriteLine(_message.Body.ToString());
                _client.DeleteMessage(_message.MessageId, _message.PopReceipt);

            }
        }
        public static void addBase64encodedMessage()
        {
            QueueClient _client = new QueueClient(_connection_string, _queue_name);
            string message, temp;
            for(int i = 0; i < 1; i++)
            {
                temp = JsonHelper();
                var bytes = System.Text.Encoding.UTF8.GetBytes(temp);
                message = System.Convert.ToBase64String(bytes);
                _client.SendMessage(message);
            }
        }
        public static string JsonHelper()
        {
            Names obj = new Names();
            obj.Name = "Puspraj";
            obj.PartitionKey = "25";
            obj.RowKey = "8989028678";
            string jsonData = JsonConvert.SerializeObject(obj);
            return jsonData;
        }
    }
}
