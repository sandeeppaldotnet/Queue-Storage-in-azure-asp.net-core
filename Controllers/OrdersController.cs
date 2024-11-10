using Azure_Blob_Storage.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Azure_Blob_Storage.Controllers
{
   // [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly QueueService _queueService;

        public OrdersController(QueueService queueService)
        {
            _queueService = queueService;
        }

        // Place an order by enqueuing it to Azure Queue
        [HttpPost("placeorder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequest orderRequest)
        {
            if (string.IsNullOrEmpty(orderRequest.OrderDetails))
            {
                return BadRequest("Order details cannot be empty.");
            }

            await _queueService.EnqueueOrderAsync(orderRequest.OrderDetails);
            return Ok($"Order placed: {orderRequest.OrderDetails}");
        }
    }
}
