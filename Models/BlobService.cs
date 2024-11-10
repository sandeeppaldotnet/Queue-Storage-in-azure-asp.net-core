
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
namespace Azure_Blob_Storage.Models
{
    public class BlobService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobService(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("AzureStorage:ConnectionString");
            _containerName = configuration.GetValue<string>("AzureStorage:ContainerName");
        }
        // Upload file to Blob Storage
        public async Task UploadFileAsync(string fileName, Stream fileStream)
        {
            var client = new BlobServiceClient(_connectionString);
            var containerClient = client.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, overwrite: true);
        }
        // Download file from Blob Storage
        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var client = new BlobServiceClient(_connectionString);
            var containerClient = client.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            var blobDownloadInfo = await blobClient.DownloadAsync();
            return blobDownloadInfo.Value.Content;
        }
    }
}
