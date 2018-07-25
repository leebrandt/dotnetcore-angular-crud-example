using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using okta_dotnetcore_react_example.Data;

namespace dotnet_angular_crud_example.Controllers
{
  [Authorize]
  [Route("/api/[controller]")]
  public class RestaurantRatingController : Controller
  {
    private readonly RestaurantRatingContext context;

    public RestaurantRatingController(RestaurantRatingContext context)
    {
      this.context = context;
    }

    public async Task<ActionResult> GetAsync()
    {
      var userId = User.Claims.SingleOrDefault(u => u.Type == "uid")?.Value;
      var ratings = await context.RestaurantRatings.Where(rr => rr.UserID == userId).ToListAsync();
      return Ok(ratings);
    }
  }
}