using Atithi.Web.Context;
using Atithi.Web.Models.Domain;
using Atithi.Web.Models.DTO;
using Atithi.Web.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Atithi.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly AtithiDbContext _atithiDbContext;

        public OrderService(AtithiDbContext atithiDbContext) 
        {
            _atithiDbContext = atithiDbContext;
        }

        public Task<MenuDTO> GetOrderByRoomId(int RoomId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PlaceOrder(OrderDetailsDTO orderDetails)
        {
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                OrderId = orderId,
                IsDelivered = false,
                OrderDate = DateTime.Now,
                RoomId = orderDetails.RoomId
            };

            var orderItems = new List<OrderItem>();
            foreach (var item in orderDetails.OrderItems)
            {
                var items = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    OrderId = orderId,
                };
                orderItems.Add(items);
            }

            using (IDbContextTransaction transaction = await _atithiDbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _atithiDbContext.Order.AddAsync(order);
                    await _atithiDbContext.OrderItem.AddRangeAsync(orderItems);
                    await _atithiDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            return true;
        }

        public async Task<bool> ConfirmOrderDeliveryAsync(Guid orderId, int roomId)
        {
            // Find the order by OrderId and RoomId
            var order = await _atithiDbContext.Order
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.RoomId == roomId);

            if (order == null)
            {
                return false;
            }

            if (order.IsDelivered)
            {
                return true;
            }

            order.IsDelivered = true;
            await _atithiDbContext.SaveChangesAsync();

            await OrderStatusWebSocket.SendOrderStatusUpdate(orderId, order.IsDelivered, roomId);

            return true;
        }

        public async Task<OrderFetchDetailsDTO?> GetOrderDetailsAsync(Guid orderId, int roomId)
        {
            // Fetch the order with its order items and related menu information
            var order = await _atithiDbContext.Order
                .Include(o => o.OrderItems) // Include OrderItems
                .ThenInclude(oi => oi.Menu) // Include related Menu items
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.RoomId == roomId);

            if (order == null || order.IsDelivered)
            {
                return null; // If no order is found, return null
            }

            // Create the DTO with the order details
            var orderDetails = new OrderFetchDetailsDTO
            {
                OrderId = order.OrderId,
                RoomId = order.RoomId,
                OrderDate = order.OrderDate,
                IsDelivered = order.IsDelivered,
                OrderItems = order.OrderItems.Select(oi => new TotalOrderItemDTO
                {
                    MenuId = oi.Menu.MenuId,
                    ItemName = oi.Menu.ItemName, // Get the name from the related Menu
                    Quantity = oi.Quantity,
                    PriceOfEach = oi.Menu.Price,
                    TotalPrice = oi.Quantity * oi.Menu.Price // Calculate the price for this item
                }).ToList() // Convert to list of OrderItemDTO
            };

            // Calculate the total price of the order
            orderDetails.TotalPrice = orderDetails.OrderItems.Sum(oi => oi.TotalPrice);

            return orderDetails; // Return the order details
        }
    }
}
