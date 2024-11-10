using Azure.Storage.Blobs;

namespace Azure_Blob_Storage.Models
{
    
        public interface IBlobStorageService
        {
            Task SaveOrderFileAsync(string orderDetails, string fileName);
        }
    
        public class BlobStorageService : IBlobStorageService
        {
            private readonly BlobContainerClient _containerClient;

            public BlobStorageService(IConfiguration configuration)
            {
                var connectionString = configuration["AzureStorage:ConnectionString"];
                var containerName = configuration["AzureStorage:BlobContainerName"];
                _containerClient = new BlobContainerClient(connectionString, containerName);
                _containerClient.CreateIfNotExists(); // Creates the container if it doesn't exist
            }

            public async Task SaveOrderFileAsync(string orderDetails, string fileName)
            {
                var blobClient = _containerClient.GetBlobClient(fileName);
                using var memoryStream = new MemoryStream(); // Simulate creating a file (e.g., invoice)
                var writer = new StreamWriter(memoryStream);
                writer.WriteLine($"Invoice for Order: {orderDetails}");
                writer.Flush();
                memoryStream.Position = 0;

                await blobClient.UploadAsync(memoryStream, overwrite: true);
                Console.WriteLine($"Order file saved to Blob Storage as {fileName}");
            }
        }
    }

