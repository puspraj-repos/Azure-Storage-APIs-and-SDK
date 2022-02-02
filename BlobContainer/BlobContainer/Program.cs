using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.IO;

namespace BlobContainer
{
    class Program
    {
        private static string _connection_string = "Connection_string_for_storage_account";
        private static string _container_name = "az-204";
        private static string _blob_name = "Sample_Input_File.txt";
        private static string _locaiton = "F:\\Sample_input_File.txt";
        static void Main(string[] args)
        {
            BlobServiceClient _service_client = new BlobServiceClient(_connection_string);
            _service_client.CreateBlobContainer(_container_name);
            BlobContainerClient _container_client = new BlobContainerClient(_connection_string,_container_name);
            BlobContainerClient _container_client = _service_client.GetBlobContainerClient(_container_name);
            BlobClient _blob_client = _container_client.GetBlobClient(_blob_name);
            _blob_client.Upload(_locaiton);
            Console.WriteLine("Blob created");
            foreach(BlobItem item in _container_client.GetBlobs())
            {
            Console.WriteLine(item.Name);
            }
            BlobClient _blob_client = _container_client.GetBlobClient(_blob_name);
            _blob_client.DownloadTo(_locaiton);


            ReadBlob();
            getBlobProperties();
            getMetadata();
            setMetadata();
            getLease();
            Console.ReadKey();
        }
        public static Uri getSas()
        {
            BlobServiceClient _service_client = new BlobServiceClient(_connection_string);
            BlobContainerClient _container_client = _service_client.GetBlobContainerClient(_container_name);
            BlobClient _blob_client = _container_client.GetBlobClient(_blob_name);
            BlobSasBuilder _sas_builder = new BlobSasBuilder()
            {
                BlobContainerName = _container_name,
                BlobName = _blob_name,
                Resource = "b"
            };
            _sas_builder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.List);
            _sas_builder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
            return _blob_client.GenerateSasUri(_sas_builder);

        }
        public static void ReadBlob()
        {
            Uri _sas_uri = getSas();
            BlobClient _client = new BlobClient(_sas_uri);
            _client.DownloadTo(_locaiton);
        }

        public static void getBlobProperties()
        {
            BlobServiceClient _service = new BlobServiceClient(_connection_string);
            BlobContainerClient _container = _service.GetBlobContainerClient(_container_name);
            BlobClient _blob = _container.GetBlobClient(_blob_name);
            BlobProperties _property = _blob.GetProperties();
            Console.WriteLine($"The access tier is {_property.AccessTier}");
            Console.WriteLine(_property.ContentLength);
        }

        public static void getMetadata()
        {
            BlobServiceClient _service = new BlobServiceClient(_connection_string);
            BlobContainerClient _container = _service.GetBlobContainerClient(_container_name);
            BlobClient _blob = _container.GetBlobClient(_blob_name);
            BlobProperties _property = _blob.GetProperties();
            IDictionary<string, string> dict = _property.Metadata;
            foreach(var item in dict)
            {
                Console.WriteLine(item.Key);
                Console.WriteLine(item.Value);
            }
        }
        public static void setMetadata()
        {
            BlobServiceClient _service = new BlobServiceClient(_connection_string);
            BlobContainerClient _container = _service.GetBlobContainerClient(_container_name);
            BlobClient _blob = _container.GetBlobClient(_blob_name);
            BlobProperties _property = _blob.GetProperties();
            IDictionary<string, string> dict = _property.Metadata;
            dict.Add("name", "Puspraj");
            _blob.SetMetadata(dict);
            
        }

        public static void getLease()
        {
            BlobServiceClient _service = new BlobServiceClient(_connection_string);
            BlobContainerClient _container = _service.GetBlobContainerClient(_container_name);
            BlobClient _blob = _container.GetBlobClient(_blob_name);
            BlobProperties _property = _blob.GetProperties();
            MemoryStream _memory = new MemoryStream();
            _blob.DownloadTo(_memory);
            _memory.Position = 0;
            StreamReader _reader = new StreamReader(_memory);

            Console.WriteLine(_reader.ReadToEnd());
            BlobLeaseClient _lease_client = _blob.GetBlobLeaseClient();
            BlobLease _lease = _lease_client.Acquire(TimeSpan.FromSeconds(30));
            Console.WriteLine(_lease.LeaseId);
            StreamWriter _writer = new StreamWriter(_memory);
            _writer.WriteLine("Change done twice");
            _writer.Flush();
            _memory.Position = 0;

            BlobUploadOptions _upload_options = new BlobUploadOptions()
            {
                Conditions = new BlobRequestConditions()
                {
                    LeaseId = _lease.LeaseId
                }
            };

            _blob.Upload(_memory, _upload_options); // true is added to override existing file
            _lease_client.Release();

        }
    }
}
