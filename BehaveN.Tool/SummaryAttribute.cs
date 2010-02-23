using System;

namespace BehaveN.Tool
{
    public class SummaryAttribute : Attribute
    {
        public SummaryAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
