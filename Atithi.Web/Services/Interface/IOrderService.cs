using Atithi.Web.Models.DTO;

namespace Atithi.Web.Services.Interface
{
    public interface IOrderService
    {
        Task<bool> ConfirmOrderDeliveryAsync(Guid orderId, int roomId);
        Task<MenuDTO> GetOrderByRoomId(int RoomId); // Fetch a menu item by MenuId
        Task<OrderFetchDetailsDTO> GetOrderDetailsAsync(Guid orderId, int roomId);
        Task<Guid> PlaceOrder(OrderDetailsDTO orderDetails);
    }
}
