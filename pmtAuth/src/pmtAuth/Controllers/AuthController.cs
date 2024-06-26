using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pmt_auth.Services;
using pmt_auth.Context;
using pmt_auth.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace pmt_auth.Controllers {

  [Route("[controller]")]
  public class AuthController : Controller
  {
    private AuthService _authSvc;
    private IConfiguration _config;

    private readonly string _NAME_FROM_TOKEN_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

    public AuthController(PmtAuthContext ctx, IConfiguration config) {
      _config = config;
      _authSvc = new AuthService(ctx, _config.GetValue<string>("TokenKey"));
    }

    [HttpGet("verify")]    
    [Authorize]
    public async Task<IActionResult> Verify()
    {
      try
      {
        string token = HttpContext.Request.Headers["Authorization"].ToString()?.Split("Bearer ")[1];
        if (string.IsNullOrEmpty(token))
        {
          return StatusCode(401, "Unauthorized");
        }
        var jwtToken = new JwtSecurityToken(token);
        var name = jwtToken.Payload.First(data => data.Key == _NAME_FROM_TOKEN_KEY).Value.ToString();
        return Ok(new VerifyTokenHttpGetResponse() { Name = name });
      }
      catch (Exception ex)
      {
        return StatusCode(500, ex.Message);
      }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterHttpPostRequest request)
    {
      try
      {
        _authSvc.RegisterUser(request);
        return Ok();
      }
      catch (Exception ex)
      {
        if (ex.Message == "Invalid request")
        {
          return BadRequest(ex.Message);
        }
        else
        {
          return StatusCode(500, ex.Message);
        }        
      }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginHttpPostRequest request)
    {
      try
      {        
        LoginHttpPostResponse response = _authSvc.Login(request.Email, request.Password);
        return Ok(response);
      }
      catch (Exception ex)
      {
        if (ex.Message == "Invalid request")
        {
          return BadRequest(ex.Message);
        }
        else
        {
          return StatusCode(500, ex.Message);
        }
      }
    }    
  }  
  
}