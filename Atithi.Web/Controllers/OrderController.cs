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

    }
}
