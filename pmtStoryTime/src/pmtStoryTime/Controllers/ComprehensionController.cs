using System;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace api.pmt_story_time.Controllers
{
    [Route("[controller]")]
    public class ComprehensionController : Controller
    {        
        private IConfiguration _config;

        private readonly string _NAME_FROM_TOKEN_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        public ComprehensionController(IConfiguration config)
        {
            _config = config;
        }
        
        [HttpGet("story")]
        public async Task<IActionResult> Story()
        {
            try
            {   
                string token = HttpContext.Request.Headers["Authorization"].ToString().Split("Bearer ")[1];
                var jwtToken = new JwtSecurityToken(token);
                var name = jwtToken.Payload.First(data => data.Key == _NAME_FROM_TOKEN_KEY).Value.ToString();
                return Ok(name);
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