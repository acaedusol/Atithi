using Atithi.Web.Models.DTO;

namespace Atithi.Web.Services.Interface
{
    public interface IOrderService
    {
        Task<MenuDTO> GetOrderByRoomId(int RoomId); // Fetch a menu item by MenuId
        Task<bool> PlaceOrder(OrderDetailsDTO orderDetails);
    }
}
