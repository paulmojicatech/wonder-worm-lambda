namespace pmt_story_time.Models
{
    public class StoryTimeHttpPostResponse
    {
        public string Story { get; set; }
        public IEnumerable<OpenAIQuestionAndAnswer> QuestionsAndAnswers { get; set; }

    }
}
