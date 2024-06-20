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

    public class OpenAIContentDetails {
      public string Story {get; set;}
      public IEnumerable<OpenAIQuestionAndAnswer> QuestionsAndAnswers {get; set;}
    }

    public class OpenAIQuestionAndAnswer {
      public string Question {get; set;}
      public IEnumerable<OpenAIPossibleAnswer> PossibleAnswers {get; set;}
    }

    public class OpenAIPossibleAnswer {
      public string Answer {get; set;}
      public bool IsCorrect {get; set;}
    }
}
