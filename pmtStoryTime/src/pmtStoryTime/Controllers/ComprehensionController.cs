using System;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using pmt_story_time.Services;

namespace api.pmt_story_time.Controllers
{
    [Route("[controller]")]
    public class ComprehensionController : Controller
    {        
        private IConfiguration _config;
        private AuthHttpService _authHttpSvc;

        private readonly string _NAME_FROM_TOKEN_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

        public ComprehensionController(IConfiguration config)
        {
            _config = config;
            _authHttpSvc = new AuthHttpService(_config.GetValue<string>("AuthLambdaUrl"));
        }
        
        [HttpGet("story")]
        public async Task<IActionResult> Story()
        {
            try
            {   
                string authHeader = HttpContext.Request.Headers["Authorization"];
                if (!authHeader)
                {
                    return StatusCode(401, "Unauthorized");
                }
                bool isTokenValid = await _authHttpSvc.ValidateToken(authHeader);
                if (!isTokenValid)
                {
                    return StatusCode(401, "Unauthorized");
                }
                var name = "paulmojicatech";
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