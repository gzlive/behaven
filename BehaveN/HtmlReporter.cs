// <copyright file="HtmlReporter.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
//
// This source code is released under the MIT License.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// </copyright>

namespace BehaveN
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents a reporter that outputs HTML.
    /// </summary>
    public class HtmlReporter
        : Reporter,
          IBlockReporter<Form>,
          IBlockReporter<Grid>,
          IBlockReporter<Text>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlReporter"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public HtmlReporter(TextWriter writer)
        {
            _writer = writer;
        }

        private TextWriter _writer;

        /// <summary>
        /// Reports the specifications file.
        /// </summary>
        /// <param name="specificationsFile">The specifications file.</param>
        /// <remarks>This reports all scenarios in the file and their
        /// undefined steps.</remarks>
        public override void ReportSpecificationsFile(SpecificationsFile specificationsFile)
        {
            _writer.WriteLine(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"" ""http://www.w3.org/TR/html4/strict.dtd"">");
            _writer.WriteLine("<html>");
            _writer.WriteLine("<head>");
            _writer.WriteLine(CSS);
            _writer.WriteLine("<head>");
            _writer.WriteLine("<body>");
            _writer.WriteLine(@"<div id=""wrap"">");

            if (!string.IsNullOrEmpty(specificationsFile.Title))
            {
                _writer.WriteLine("<h1>{0}</h1>", Escape(specificationsFile.Title));
            }

            if (!string.IsNullOrEmpty(specificationsFile.Description))
            {
                _writer.WriteLine("<p>{0}</p>", Escape(specificationsFile.Description));
            }

            foreach (Scenario scenario in specificationsFile.Scenarios)
            {
                ReportScenario(scenario);
            }

            ReportUndefinedSteps(specificationsFile.GetUndefinedSteps());

            _writer.WriteLine("</div>");
            _writer.WriteLine("</body>");
            _writer.WriteLine("</html>");

            _writer.Flush();
        }

        /// <summary>
        /// Reports the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public override void ReportScenario(Scenario scenario)
        {
            _writer.WriteLine("<h2>{0}</h2>", Escape(scenario.Name));

            foreach (Step step in scenario.Steps)
            {
                switch (step.Result)
                {
                    case StepResult.Passed:
                        ReportPassed(step);
                        break;
                    case StepResult.Failed:
                        ReportFailed(step);
                        break;
                    case StepResult.Undefined:
                        ReportUndefined(step);
                        break;
                    case StepResult.Pending:
                        ReportPending(step);
                        break;
                    case StepResult.Skipped:
                        ReportSkipped(step);
                        break;
                }

                if (step.Block != null)
                    ReportBlock(step.Block);
            }

            ReportException(scenario);

            _writer.Flush();
        }

        private void ReportException(Scenario scenario)
        {
            if (scenario.Exception != null)
            {
                _writer.WriteLine("<p class='error'>{0}</p>", Escape(scenario.Exception.Message));
            }
        }

        /// <summary>
        /// Reports the undefined steps.
        /// </summary>
        /// <param name="undefinedSteps">The undefined steps.</param>
        public override void ReportUndefinedSteps(ICollection<Step> undefinedSteps)
        {
        }

        private void ReportUndefined(Step step)
        {
            ReportStatus(step, "red", QuestionMark);
        }

        private void ReportPending(Step step)
        {
            ReportStatus(step, "red", XMark);
        }

        private void ReportPassed(Step step)
        {
            ReportStatus(step, "green", CheckMark);
        }

        private void ReportFailed(Step step)
        {
            ReportStatus(step, "red", XMark);
        }

        private void ReportSkipped(Step step)
        {
            ReportStatus(step, "grey", XMark);
        }

        private void ReportStatus(Step step, string color, string symbol)
        {
            _writer.WriteLine(@"<p><span style=""color:{1};"">{2}</span> {0}</p>", Escape(step.Text), color, symbol);
        }

        private const string QuestionMark = "&#65311;";
        private const string CheckMark = "&#10003;";
        private const string XMark = "&#10007;";

        private void ReportBlock(IBlock block)
        {
            block.ReportTo(this);
        }

        private static string Escape(string text)
        {
            return text.Replace("&", "&amp;").Replace("<", "&lt;");
        }

        void IBlockReporter<Form>.ReportBlock(Form block)
        {
            _writer.WriteLine(@"<table>");

            for (int i = 0; i < block.Size; i++)
            {
                _writer.WriteLine("<tr>");

                string label = block.GetLabel(i);
                _writer.WriteLine("<th>{0}</th>", Escape(label));

                string value = block.GetValue(i);
                _writer.WriteLine(@"<td>{0}</td>", Escape(value));

                _writer.WriteLine("</tr>");
            }

            _writer.WriteLine("</table>");
        }

        void IBlockReporter<Grid>.ReportBlock(Grid block)
        {
            _writer.WriteLine("<table>");

            _writer.WriteLine("<tr>");

            for (int i = 0; i < block.ColumnCount; i++)
            {
                string header = block.GetHeader(i);
                _writer.WriteLine("<th>{0}</th>", Escape(header));
            }

            _writer.WriteLine("</tr>");

            for (int i = 0; i < block.RowCount; i++)
            {
                _writer.WriteLine("<tr>");

                for (int j = 0; j < block.ColumnCount; j++)
                {
                    string value = block.GetValue(i, j);
                    _writer.WriteLine("<td>{0}</td>", Escape(value));
                }

                _writer.WriteLine("</tr>");
            }

            _writer.WriteLine("</table>");
        }

        void IBlockReporter<Text>.ReportBlock(Text block)
        {
            _writer.WriteLine("<blockquote>");

            foreach (var line in TextParser.GetLines(block.StringBuilder.ToString()))
            {
                _writer.WriteLine(line);
            }

            _writer.WriteLine("</blockquote>");
        }

        private static string CSS = @"<style type=""text/css"">
/**
 * SenCSS - Sensible Standards CSS framework
 *
 * Copyright (c) 2008-2010 Kilian Valkhof (kilianvalkhof.com)
 * Visit sencss.kilianvalkhof.com for more information and changelogs.
 * Licensed under the MIT license. http://www.opensource.org/licenses/mit-license.php
 *
 */
html,body,div,span,object,iframe,blockquote,pre,abbr,address,cite,code,del,dfn,em,img,ins,kbd,q,samp,small,strong,var,fieldset,form,table,caption,tbody,tfoot,thead,tr,th,td,article,aside,dialog,figure,footer,header,hgroup,menu,nav,section,time,mark,audio,video{vertical-align:baseline;margin:0;padding:0}
body{background:#fff;color:#000;font:75%/1.5em Arial, Helvetica, ""Liberation sans"", ""Bitstream Vera Sans"", sans-serif;position:relative}
textarea{font:100%/1.5em Arial, Helvetica, ""Liberation sans"", ""Bitstream Vera Sans"", sans-serif;border:1px solid #ccc;border-bottom-color:#eee;border-right-color:#eee;box-sizing:border-box;-moz-box-sizing:border-box;-webkit-box-sizing:border-box;-ms-box-sizing:border-box;width:100%;margin:0;padding:.29em 0}
blockquote,q{quotes:none}
blockquote:before,blockquote:after,q:before,q:after{content:none}
:focus{outline:none}
a{text-decoration:underline}
a:hover,a:focus{text-decoration:none}
abbr,acronym{border-bottom:1px dotted;cursor:help;font-variant:small-caps}
address,cite,em,i{font-style:italic}
blockquote p{margin:0 1.5em 1.5em;padding:.75em}
code,kbd,tt{font-family:""Courier New"", Courier, monospace, serif;line-height:1.5}
del{text-decoration:line-through}
dfn{border-bottom:1px dashed;font-style:italic}
dl{margin:0 0 1.5em}
dd{margin-left:1.5em}
h1,h2,h3,h4,h5,h6{font-weight:700;padding:0}
h1{font-size:2em;margin:0 0 .75em}
h2{font-size:1.5em;margin:0 0 1em}
h3{font-size:1.1666em;margin:0 0 1.285em}
h4{font-size:1em;margin:0 0 1.5em}
h5{font-size:.8333em;margin:0 0 1.8em}
h6{font-size:.666em;margin:0 0 2.25em}
img{display:inline-block;vertical-align:text-bottom}
ins{text-decoration:overline}
mark{background-color:#ff9;color:#000;font-style:italic;font-weight:700}
ol{list-style:outside decimal}
p{font-weight:300;margin:0 0 1.5em}
pre{font-family:""Courier New"", Courier, monospace, serif;margin:0 0 1.5em}
sub{top:.4em;font-size:.85em;line-height:1;position:relative;vertical-align:baseline}
sup{font-size:.85em;line-height:1;position:relative;bottom:.5em;vertical-align:baseline}
ul{list-style:outside disc}
ul,ol{margin:0 0 1.5em 1.5em;padding:0}
li ul,li ol{margin:0 0 1.5em 1.5em;padding:0}
table{border-collapse:collapse;border-spacing:0;margin:0 0 1.5em;padding:0}
caption{font-style:italic;text-align:left}
tr.alt td{background:#eee}
td{border:1px solid #000;vertical-align:middle;padding:.333em}
th{font-weight:700;vertical-align:middle;padding:.333em}
button{cursor:pointer;display:block;font-size:1em;height:2em;line-height:1.5em;margin:1.75em 0 0;padding:0 .5em}
button::-moz-focus-inner{border:0}
fieldset{border:0;position:relative;margin:0 0 1.5em;padding:1.5em 0 0}
fieldset fieldset{clear:both;margin:0 0 1.5em;padding:0 0 0 1.5em}
input{border:1px solid #ccc;border-bottom-color:#eee;border-right-color:#eee;box-sizing:border-box;-moz-box-sizing:border-box;-webkit-box-sizing:border-box;-ms-box-sizing:border-box;font-size:1em;height:1.5em;line-height:1.5em;width:100%;margin:0 0 .75em;padding:.29em 0}
input[type=file]{height:2.25em;padding:0}
select{border:1px solid #ccc;border-bottom-color:#eee;border-right-color:#eee;font-size:1em;height:2.25em;_margin:0 0 1.3em;margin:0 0 .8em;padding:.2em 0 0}
optgroup{font-weight:700;font-style:normal;text-indent:.2em}
optgroup + optgroup{margin-top:1em}
option{font-size:1em;height:1.5em;text-indent:1em;padding:0}
label{cursor:pointer;display:block;height:auto;line-height:1.4em;width:100%;margin:0;padding:0}
label input{background:0;border:0;height:1.5em;line-height:1.5em;width:auto;margin:0 .5em 0 0;padding:0}
legend{font-size:1.1666em;font-weight:700;left:0;margin:0;padding:0}
dt,strong,b{font-weight:700}
.amp{font-family:Baskerville, ""Goudy Old Style"", Palatino, ""Book Antiqua"", ""URW Chancery L"", Gentium, serif;font-style:italic}
.quo{font-family:Georgia, Gentium, ""Times New Roman"", Times, serif}
.lquo{font-family:Georgia, Gentium, ""Times New Roman"", Times, serif;margin:0 0 0 -.55em}
.introParagraphArticle:first-letter{float:left;font-size:3.2em;font-weight:700;line-height:1em;margin:0 0 -.2em;padding:.125em .1em 0 0}
.message{background:#eee;border:1px solid #999;margin:1.5em;padding:.666em}
.error{background:#fee;border:1px solid red;margin:1.5em;padding:.666em}
.notice{background:#eef;border:1px solid #00f;margin:1.5em;padding:.666em}
.success{background:#efe;border:1px solid #0f0;margin:1.5em;padding:.666em}
.warning{background:#ffe;border:1px solid #ff0;margin:1.5em;padding:.666em}
.aside-left{clear:left;float:left;overflow:hidden;margin:0 1.5em 1.5em 0}
.aside-right{clear:right;float:right;overflow:hidden;margin:0 0 1.5em 1.5em}
.horizontalForm button{clear:left;float:left;margin:.25em 0 0}
.horizontalForm input,.horizontalForm textarea{float:left;width:49%;margin:0 0 .8em}
.horizontalForm select{float:left;_margin:0 0 1.25em;margin:0 0 .75em}
.horizontalForm label{clear:left;float:left;width:49%;padding:.375em 0}
.horizontalForm label input{height:1em;line-height:1.5em;width:auto;margin:.25em .5em 0 0}
.horizontalForm label.singleLine{clear:both;float:none;height:1.5em;width:100%;padding:0}
#wrap {
	width:760px;
	margin:auto;
	padding:1.5em 10px;
}
</style>";
    }
}
