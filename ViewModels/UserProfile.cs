public class UserProfileViewModel
{
    public string? UserName { get; set; }
    public string? FullName { get; set; }
   
    public List<Order> Orders { get; set; } = new List<Order>();
}
