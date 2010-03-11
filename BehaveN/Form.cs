// <copyright file="Form.cs" company="Jason Diamond">
//
// Copyright (c) 2009-2010 Jason Diamond
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
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a form.
    /// </summary>
    public class Form : IBlock
    {
        /// <summary>
        /// Regex that matches lines in a form.
        /// </summary>
        private static readonly Regex FormRegex = new Regex(@"^\s*:\s*([^:]+?)\s*:\s*(.+)\s*", RegexOptions.IgnoreCase);

        /// <summary>
        /// The list of labels in this form.
        /// </summary>
        private readonly List<string> labels = new List<string>();

        /// <summary>
        /// The list of values in this form.
        /// </summary>
        private readonly List<string> values = new List<string>();

        /// <summary>
        /// Gets the size of the form.
        /// </summary>
        /// <value>The size of the form.</value>
        public int Size
        {
            get { return this.labels.Count; }
        }

        /// <summary>
        /// Determines if the next line is the beginning of a form.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="currentIndex">Index of the current line.</param>
        /// <returns><c>true</c> if the next line is the beginning of a form; otherwise <c>false</c></returns>
        public static bool NextLineIsForm(List<string> lines, int currentIndex)
        {
            return (currentIndex + 1) < lines.Count && FormRegex.IsMatch(lines[currentIndex + 1]);
        }

        /// <summary>
        /// Parses the form.
        /// </summary>
        /// <param name="text">The text containing the form.</param>
        /// <returns>A new <see cref="Form">Form</see> object.</returns>
        public static Form Parse(string text)
        {
            return ParseForm(TextParser.GetLines(text), 0);
        }

        /// <summary>
        /// Parses the form.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="i">The current line index.</param>
        /// <returns>A new <see cref="Form">Form</see> object.</returns>
        public static Form ParseForm(List<string> lines, int i)
        {
            Form form = new Form();

            Match m;

            while (i < lines.Count && (m = FormRegex.Match(lines[i])).Success)
            {
                string label = m.Groups[1].Value;
                string value = m.Groups[2].Value;

                form.Add(label, value);

                i++;
            }

            return form;
        }

        /// <summary>
        /// Converts the block into an object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <returns>The new object.</returns>
        public object ConvertTo(Type type)
        {
            if (type == null)
            {
                return null;
            }

            object theObject = Activator.CreateInstance(type);

            for (int i = 0; i < this.Size; i++)
            {
                string label = this.GetLabel(i);
                ValueSetter setter = ValueSetter.GetValueSetter(theObject, label);

                if (setter.CanSetValue())
                {
                    setter.SetFormattedValue(this.GetValue(i));
                }
                else
                {
                    throw new Exception(string.Format("Could not set {0} on type {1}.", label, type.FullName));
                }
            }

            return theObject;
        }

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns>
        /// A <c>string</c> representing the form.
        /// </returns>
        public string Format()
        {
            int width = 0;

            foreach (string label in this.labels)
            {
                width = Math.Max(width, label.Length);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.labels.Count; i++)
            {
                sb.AppendFormat("    : {0} : {1}", this.labels[i].PadLeft(width), this.values[i]);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks that all of the values are on the specified object.
        /// </summary>
        /// <param name="actual">The object to check against.</param>
        /// <returns>True if all checks pass.</returns>
        public bool Check(object actual)
        {
            bool passed = true;

            Type type = actual.GetType();

            for (int i = 0; i < this.Size; i++)
            {
                string label = this.GetLabel(i);

                string propertyName = NameComparer.NormalizeName(label);

                PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (pi != null)
                {
                    object actualValue = pi.GetValue(actual, null);
                    object expectedValue = ValueParser.ParseValue(this.GetValue(i), pi.PropertyType);

                    if (!object.Equals(actualValue, expectedValue))
                    {
                        this.values[i] = string.Format("{0} (was {1})", expectedValue, actualValue);
                        passed = false;
                    }
                }
                else
                {
                    this.values[i] = string.Format("{0} (unknown)", this.GetValue(i));
                    passed = false;
                }
            }

            return passed;
        }

        /// <summary>
        /// Gets the suggested type for the parameter.
        /// </summary>
        /// <returns>The suggested type.</returns>
        public string GetSuggestedParameterType()
        {
            return "Foo";
        }

        /// <summary>
        /// Gets the suggested name for the parameter.
        /// </summary>
        /// <returns>The suggested name.</returns>
        public string GetSuggestedParameterName()
        {
            return "foo";
        }

        /// <summary>
        /// Reports to the reporter.
        /// </summary>
        /// <param name="reporter">The reporter.</param>
        public void ReportTo(Reporter reporter)
        {
            if (reporter is IBlockReporter<Form>)
            {
                ((IBlockReporter<Form>)reporter).ReportBlock(this);
            }
            else
            {
                throw new Exception(string.Format("{0} doesn't support reporting forms.", reporter.GetType().FullName));
            }
        }

        /// <summary>
        /// Adds the specified label and value to the form.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        public void Add(string label, string value)
        {
            this.labels.Add(label.Trim());
            this.values.Add(value.Trim());
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The label.</returns>
        public string GetLabel(int index)
        {
            return this.labels[index];
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value.</returns>
        public string GetValue(int index)
        {
            return this.values[index];
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>The value.</returns>
        public string GetValue(string label)
        {
            return this.GetValue(this.labels.FindIndex(delegate(string s)
            {
                return string.Equals(s, label, StringComparison.OrdinalIgnoreCase);
            }));
        }

        /// <summary>
        /// Converts an object into a form.
        /// </summary>
        /// <param name="item">The object to convert.</param>
        /// <param name="itemType">Type of the object to convert.</param>
        /// <returns>A new Form object.</returns>
        internal static IBlock FromObject(object item, Type itemType)
        {
            Form form = new Form();

            foreach (PropertyInfo pi in itemType.GetProperties())
            {
                if (pi.CanRead && pi.GetGetMethod().GetParameters().Length == 0)
                {
                    string name = pi.Name;
                    object value = pi.GetValue(item, null);
                    form.Add(name, value != null ? value.ToString() : "null");
                }
            }

            return form;
        }
    }
}
