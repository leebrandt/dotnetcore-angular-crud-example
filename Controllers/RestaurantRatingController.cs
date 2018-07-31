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

    [HttpGet]
    public async Task<ActionResult> GetAsync()
    {
      var userId = this.GetUserId();
      var ratings = await context.RestaurantRatings.Where(rr => rr.UserID == userId).ToListAsync();
      return Ok(ratings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(int id)
    {
      var userId = this.GetUserId();
      var rating = await context.RestaurantRatings.SingleOrDefaultAsync<RestaurantRating>(rr => rr.ID == id);
      if (rating.UserID != userId)
      {
        return Unauthorized();
      }
      else
      {
        return Ok(rating);
      }
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] RestaurantRating rating)
    {
      var userId = this.GetUserId();
      if (rating.ID > 0)
      {
        var savedRating = await context.RestaurantRatings.SingleOrDefaultAsync<RestaurantRating>(rr => rr.ID == rating.ID);
        if (savedRating == null) // Make sure there is a rating with that ID
        {
          return NotFound(rating);
        }
        if (savedRating.UserID != userId) // Make sure the user making the request can update this rating
        {
          return Unauthorized();
        }
        savedRating.RestaurantName = rating.RestaurantName;
        savedRating.RestaurantType = rating.RestaurantType;
        savedRating.Rating = rating.Rating;
        await context.SaveChangesAsync();
        return Ok(rating);
      }
      else
      {
        rating.UserID = userId;
        await context.AddAsync<RestaurantRating>(rating);
        await context.SaveChangesAsync();
        return CreatedAtAction("GetByIdAsync", rating);
      }

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
      var ratingToDelete = new RestaurantRating { ID = id };
      context.RestaurantRatings.Attach(ratingToDelete);
      context.Entry(ratingToDelete).State = EntityState.Deleted;
      await context.SaveChangesAsync();
      return Ok();
    }

    private string GetUserId()
    {
      return User.Claims.SingleOrDefault(u => u.Type == "uid")?.Value;
    }
  }
}