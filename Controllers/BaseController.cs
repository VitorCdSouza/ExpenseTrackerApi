using ExpenseTrackerApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetUserIdToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Remove(0, 7);
            int userId = 0;
            if (!Int32.TryParse(Encryption.GetIdUserByToken(token), out userId))
            {
                return 0;
            }
            return userId;
        }
    }
}
