using System.Text.RegularExpressions;
using pmt_auth.Context;
using pmt_auth.Models;

namespace pmt_auth.Services
{
  public class AuthService
  {
    private PmtAuthContext _context;
    private string _tokenKey;
    
    public AuthService(PmtAuthContext ctx, string tokenKey) 
    {
      _context = ctx;
      _tokenKey = tokenKey;
    }

    public void RegisterUser(RegisterHttpPostRequest request)
    {
      try
      {
        if (!ValidateRegisterRequest(request))
        {
          throw new Exception("Invalid request");
        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        User user = new User
        {
          Name = request.Name,
          Email = request.Email,
          DateOfBirth = request.DateOfBirth,
          PasswordHash = hashedPassword,
          Status = UserStatus.PENDING
        };
        _context.Users.Add(user);
        _context.SaveChanges();
      }
      catch
      {
        throw;
      }      
    }

    private bool ValidateRegisterRequest(RegisterHttpPostRequest request)
    {      
      if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.DateOfBirth) || string.IsNullOrEmpty(request.Password))
      {
        return false;
      }
      string emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";      
      if (!Regex.IsMatch(request.Email, emailPattern))
      {
        return false;
      }       
      if (!DateTime.TryParse(request.DateOfBirth, out DateTime dob))
      {
        return false;
      }
      string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$";      
      if (!Regex.IsMatch(request.Password, passwordPattern))
      {
        return false;
      }
      return true;

    }
  }
}