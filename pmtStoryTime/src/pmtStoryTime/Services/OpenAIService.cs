using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using pmt_story_time.Models;

namespace pmt_story_time.Services {
  public class OpenAIService {

    private string _openAIKey;
    private string _openApiUrl;

    public OpenAIService(string openAIKey, string openApiUrl) {
      _openAIKey = openAIKey;
      _openApiUrl = openApiUrl;
    }


    public async Task<OpenAIHttpPostResponse> GetStory(string prompt) {
      // Call the OpenAI API to get a story
      try {        
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(_openApiUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAIKey}");

        var request = new OpenAIHttpPostRequest
        {
            model = "gpt-3.5-turbo",
            messages = new List<OpenAIMessage>
            {
                new OpenAIMessage
                {
                    role = "user",
                    content = prompt
                }
            },
            temperature = 0.7
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("completions", content);
        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            JObject jObj = JObject.Parse(responseString);            
            OpenAIHttpPostResponse openAiResponse = new OpenAIHttpPostResponse
            {
                Id = jObj["id"]?.ToString(),
                Object = jObj["object"]?.ToString(),
                Model = jObj["model"]?.ToString(),
                Created = int.Parse(jObj["created"]?.ToString()),
                Choices = new List<OpenAIMessage>
                {
                    new OpenAIMessage
                    {
                        role = jObj["choices"][0]["message"]?["role"]?.ToString(),
                        content = jObj["choices"][0]["message"]?["content"].ToString(),
                        logprobs = jObj["choices"][0]["logprobs"]?.ToString(),
                        finish_reason = jObj["choices"][0]["finish_reason"]?.ToString()
                    }
                },
                Usage = jObj["usage"]?.ToObject<OpenAIUsage>()
            };
            return openAiResponse;
        }
        else
        {
            throw new Exception("Unauthorized");
        }
      } catch (Exception ex) {
        throw;
      }
    }


  }
}