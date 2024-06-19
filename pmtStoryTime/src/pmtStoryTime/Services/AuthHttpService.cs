using System.Net.Http.Headers;
using Newtonsoft.Json;
using pmt_story_time.Models;

namespace pmt_story_time.Services
{
    public class AuthHttpService
    {

        private string _LAMBDA_URL;
        public AuthHttpService(string lambdaUrl)
        {
            _LAMBDA_URL = lambdaUrl;
        }

        public async Task<VerifyTokenHttpGetResponse> ValidateToken(string token)
        {
            // Call the lambda function to validate the token
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{_LAMBDA_URL}/auth/verfiy");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", token);

                HttpResponseMessage response =  await client.GetAsync("verify");
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    VerifyTokenHttpGetResponse tokenResponse = JsonConvert.DeserializeObject<VerifyTokenHttpGetResponse>(responseString);
                    return tokenResponse;
                }
                else
                {
                    throw new Exception("Unauthorized");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}