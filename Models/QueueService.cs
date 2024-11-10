using Azure.Storage.Queues;
using System.Text;

namespace Azure_Blob_Storage.Models
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var queueName = configuration["AzureStorage:QueueName"];

            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists(); // Creates the queue if it doesn't exist
        }

        // Enqueue an order message
        public async Task EnqueueOrderAsync(string orderDetails)
        {
            var message = Convert.ToBase64String(Encoding.UTF8.GetBytes(orderDetails)); // Encode message as base64
            await _queueClient.SendMessageAsync(message);
        }

        // Dequeue an order message
        public async Task<string> DequeueOrderAsync()
        {
            var peekedMessage = await _queueClient.ReceiveMessagesAsync(1);
            var message = peekedMessage.Value?.FirstOrDefault();

            if (message != null)
            {
                await _queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                return Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText)); // Decode the message
            }

            return null;
        }
    }
}
