using System.ComponentModel.DataAnnotations;

namespace okta_dotnetcore_react_example.Data
{
  public class RestaurantRating
  {
    [Key]
    public int ID { get; set; }
    public string UserID { get; set; }
    public string RestaurantName { get; set; }
    public string RestaurantType { get; set; }
    public int Rating { get; set; }
  }
}