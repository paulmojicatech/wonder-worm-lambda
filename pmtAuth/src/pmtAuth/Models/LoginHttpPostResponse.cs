namespace pmt_auth.Models
{
  public class LoginHttpPostResponse
  {
    public string Token { get; set; }
    public string UserEmail { get; set; }
    public string Name { get; set; }
    public IEnumerable<Children> Children { get; set; }
  }
}