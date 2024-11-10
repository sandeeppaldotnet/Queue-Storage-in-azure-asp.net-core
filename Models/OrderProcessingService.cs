using Azure_Blob_Storage.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Azure_Blob_Storage.Models
{
    public class OrderProcessingService : BackgroundService
    {
        private readonly QueueService _queueService;
        private readonly IBlobStorageService _blobStorageService; // Service for handling Blob Storage

        public OrderProcessingService(QueueService queueService, IBlobStorageService blobStorageService)
        {
            _queueService = queueService;
            _blobStorageService = blobStorageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var orderDetails = await _queueService.DequeueOrderAsync();

                if (orderDetails != null)
                {
                    Console.WriteLine($"Processing order: {orderDetails}");
                    // Process the order (e.g., save to database, generate invoice, etc.)

                    // Store an order-related file (e.g., invoice) in Blob Storage
                    await _blobStorageService.SaveOrderFileAsync(orderDetails, "invoice.pdf");

                    Console.WriteLine("Order processed successfully!");
                }

                await Task.Delay(60000, stoppingToken); // Wait for 1 second before checking the queue again
            }
        }
    }
}