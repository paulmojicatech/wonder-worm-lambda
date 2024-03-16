using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
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
          Status = UserStatus.PENDING,
          RegistrationCode = Guid.NewGuid().ToString()
        };
        _context.Users.Add(user);
        _context.SaveChanges();
      }
      catch
      {
        throw;
      }      
    }

    public string Login(string email, string password)
    {
      try
      {
        User user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null)
        {
          throw new Exception("Invalid request");
        }
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
          throw new Exception("Invalid request");
        }
        return CreateToken(user, _tokenKey);
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
      if (DoesEmailAlreadyExist(request.Email))
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

    private bool DoesEmailAlreadyExist(string email)
    {
      return _context.Users.Any(u => u.Email == email);
    }

    private string CreateToken(User user, string tokenKey)
    {
      try
      {
        List<Claim> claims = new List<Claim>
        {
          new Claim(ClaimTypes.Name, user.Email)
        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));
        SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.Aes128CbcHmacSha256);
        JwtSecurityToken token = new JwtSecurityToken(
          claims: claims,
          expires: DateTime.Now.AddDays(1),
          signingCredentials: cred);
        string jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
      }
      catch
      {
        throw;
      }
    }

  }
}