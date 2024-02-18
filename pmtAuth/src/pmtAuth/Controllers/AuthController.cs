using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace pmt_auth.Controllers {

  [Route("[controller]")]
  public class AuthController : Controller
  {

    public AuthController() {}

    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
      return Ok("Success");
    }
  }
  
}