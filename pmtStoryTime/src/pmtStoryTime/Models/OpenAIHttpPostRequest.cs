using System.Collections.Generic;

namespace pmt_story_time.Models
{
    public class OpenAIHttpPostRequest
    {
        public string model { get; set; }
        public IEnumerable<OpenAIMessage> messages { get; set; }
        public double temperature { get; set; }
    }

    public class OpenAIMessage 
    {
        public string role { get; set; }
        public string content { get; set; } 
        public string? logprobs { get; set; }    
        public string? finish_reason { get; set; }
    }
}
