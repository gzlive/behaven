// <copyright file="Form.cs" company="Jason Diamond">
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
                passed &= CheckProperty(actual, i, type);
            }

            return passed;
        }

        private bool CheckProperty(object actual, int i, Type type)
        {
            bool passed = true;

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
        /// Gets the suggested type definition for the parameter.
        /// </summary>
        /// <returns>The suggested type definition.</returns>
        public string GetSuggestedParameterTypeDefinition()
        {
            string code = "public class Foo\r\n" +
                          "{\r\n";

            foreach (string label in this.labels)
            {
                code += string.Format("    public string {0} {{ get; set; }}\r\n", label);
            }

            code += "}";

            return code;
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
