using System.Text;

namespace BehaveN.Examples
{
    public class TextExampleStepDefinitions
    {
        private string _text;

        public void Given_this_text(StringBuilder text)
        {
            _text = text.ToString();
        }

        public void When_converting_numbers_into_words()
        {
            _text = _text.Replace("1", "one")
                         .Replace("2", "two");
        }

        public void Then_the_text_should_look_like_this(out StringBuilder text)
        {
            text = new StringBuilder(_text);
        }
    }
}
