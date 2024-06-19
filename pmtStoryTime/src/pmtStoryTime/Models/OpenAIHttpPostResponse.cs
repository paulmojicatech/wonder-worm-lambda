using System.Collections.Generic;

namespace pmt_story_time.Models
{
    public class OpenAIHttpPostResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public string Model { get; set; }        
        public int Created { get; set; }
        public IEnumerable<OpenAIMessage> Choices { get; set; }
        public OpenAIUsage Usage { get; set; }

    }

    public class OpenAIUsage
    {
        public int PromptTokens { get; set; }
        public int CompletionTokens { get; set; }
        public int TotalTokens { get; set; }
    }

}