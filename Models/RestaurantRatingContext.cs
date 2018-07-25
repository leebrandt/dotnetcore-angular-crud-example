using Microsoft.EntityFrameworkCore;

namespace okta_dotnetcore_react_example.Data
{
  public class RestaurantRatingContext : DbContext
  {
    public RestaurantRatingContext(DbContextOptions<RestaurantRatingContext> options) : base(options)
    { }

    public DbSet<RestaurantRating> RestaurantRatings { get; set; }
  }
}