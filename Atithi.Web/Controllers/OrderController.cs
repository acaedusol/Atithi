using Atithi.Web.Context;
using Atithi.Web.Models.Domain;
using Atithi.Web.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Atithi.Web.Services.Interface;

namespace Atithi.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AtithiDbContext _atithiDbContext;
        private readonly IOrderService _orderService;

        public OrderController(AtithiDbContext atithiDbContext, IOrderService orderService)
        {
            this._atithiDbContext = atithiDbContext;
            this._orderService = orderService;
        }

        [HttpPost("placeOrder")] // Endpoint for placing an order
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDetailsDTO orderDetails)
        {
            if (orderDetails == null || orderDetails.OrderItems == null || orderDetails.OrderItems.Count == 0)
            {
                return BadRequest("Invalid order data. Room ID and order items are required.");
            }

            try
            {
                var result = await _orderService.PlaceOrder(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while placing the order. Please try again later.");
            }
            return Ok(true); // Return the created order
        }

        [HttpPost("confirm-delivery")]
        public async Task<IActionResult> ConfirmOrderDelivery([FromBody] OrderDeliveryDTO confirmOrderDto)
        {
            try
            {
                // Confirm the order delivery
                var isConfirmed = await _orderService.ConfirmOrderDeliveryAsync(
                    confirmOrderDto.OrderId,
                    confirmOrderDto.RoomId);

                if (!isConfirmed)
                {
                    // If the order was not found, return 404 Not Found
                    return NotFound($"Order with ID {confirmOrderDto.OrderId} for Room {confirmOrderDto.RoomId} not found.");
                }

                return Ok("Order confirmed as delivered.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while confirming the order delivery.");
            }
        }

        [HttpGet("orderDetails/{orderId}/{roomId}")]
        public async Task<IActionResult> OrderDetails(Guid orderId, int roomId)
        {
            try
            {
                // Confirm the order delivery
                var orderDetails = await _orderService.GetOrderDetailsAsync(orderId, roomId);

                if (orderDetails == null)
                {
                    return NotFound($"No order found with ID {orderId} for Room {roomId}.");
                }

                return Ok(orderDetails); // Return the order details
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while Fetching the order details.");
            }
        }

    }
}
