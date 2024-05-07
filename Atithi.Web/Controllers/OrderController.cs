using Atithi.Web.Context;
using Microsoft.AspNetCore.Mvc;

namespace Atithi.Web.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AtithiDbContext _atithiDbContext;
        public OrderController(AtithiDbContext atithiDbContext)
        {
            this._atithiDbContext = atithiDbContext;
        }

    }
}
