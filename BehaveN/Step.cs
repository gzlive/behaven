namespace BehaveN
{
    public class Step
    {
        public string Keyword;
        public string Text;
        public IBlock Block;
        public StepResult Result;

        public override string ToString()
        {
            return Text;
        }
    }
}