using Atithi.Web.Context;
using Atithi.Web.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Atithi.Web.Services
{
    public class OrderedDeliveredService
    {
        private readonly AtithiDbContext _dbContext;

        public OrderedDeliveredService(AtithiDbContext context)
        {
            _dbContext = context;
        }

        public async IAsyncEnumerable<OrderDeliveryDTO> GetNewItemsAsync(Guid param1, int param2, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<bool>();

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
                var result = await _dbContext.Order
                    .FirstOrDefaultAsync(o => o.OrderId == param1 && o.RoomId == param2);

                if (result != null && result.IsDelivered)
                {
                    var orderDetail = new OrderDeliveryDTO
                    {
                        OrderId = result.OrderId,
                        RoomId = result.RoomId,
                    };
                    yield return orderDetail;

                    // Signal completion and cancel the operation
                    tcs.TrySetResult(true);
                    break;
                }
                else
                {
                    yield return null;
                }
            }

            // Wait for the cancellation token or for the result to be true
            await Task.WhenAny(Task.Delay(Timeout.Infinite, cancellationToken), tcs.Task);
        }

    }
}