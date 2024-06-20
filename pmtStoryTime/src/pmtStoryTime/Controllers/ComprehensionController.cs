using System;
using System.Web;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using pmt_story_time.Services;
using pmt_story_time.Models;

namespace api.pmt_story_time.Controllers
{
    [Route("[controller]")]
    public class ComprehensionController : Controller
    {        
        private IConfiguration _config;
        private AuthHttpService _authHttpSvc;
        private OpenAIService _openAISvc;

        private readonly string _NAME_FROM_TOKEN_KEY = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        private readonly string _PROMPT = "Generate a unique children's story containing 500 words. also generate 5 question and answers (in multiple choice format) testing comprehension. i would like the response to be structured like an object where the keys are story, comprehension, which is an array of objects with the keys question and possibleAnswers where possibleAnswers will be an array with 4 objects with the keys answer and isCorrect as a boolean.";

        public ComprehensionController(IConfiguration config)
        {
            _config = config;
            _authHttpSvc = new AuthHttpService(_config.GetValue<string>("AuthLambdaUrl"));
            _openAISvc = new OpenAIService(_config.GetValue<string>("OpenAIKey"), _config.GetValue<string>("OpenAIUrl"));
        }
        
        [HttpGet("story")]
        public async Task<IActionResult> Story()
        {
            try
            {   
                string authHeader = HttpContext.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authHeader))
                {
                    return StatusCode(401, "Unauthorized");
                }
                VerifyTokenHttpGetResponse token = await _authHttpSvc.ValidateToken(authHeader);
                if (token == null)
                {
                    return StatusCode(401, "Unauthorized");
                }     
                StoryTimeHttpPostResponse response = await _openAISvc.GetStory(_PROMPT);           
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