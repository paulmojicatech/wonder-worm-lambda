namespace pmt_auth.Models
{
  public class RegisterHttpPostRequest
  {
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
  }
}