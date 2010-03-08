using System;
using WatiN.Core;

namespace BehaveN.Examples
{
    public class WatiNStepDefinitions : IDisposable
    {
        private IE _browser;

        public void given_a_new_browser()
        {
            _browser = new IE();
        }

        public void when_navigating_to_arg1(string url)
        {
            _browser.GoTo(url);
        }

        public void when_typing_arg1_into_the_text_box_named_arg2(string text, string name)
        {
            _browser.TextField(Find.ByName(name)).TypeText(text);
        }

        public void when_clicking_the_button_named_arg1(string name)
        {
            _browser.Button(Find.ByName(name)).Click();
        }

        public void then_a_link_to_arg1_should_appear_on_the_page(string href)
        {
            foreach (var link in _browser.Links)
                if (link.Url == href)
                    return;

            throw new Exception("Could not find link to " + href);
        }

        public void Dispose()
        {
            _browser.Dispose();
        }
    }
}
