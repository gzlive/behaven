using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents a form.
    /// </summary>
    public class Form : IConvertibleObject
    {
        /// <summary>
        /// Converts the convertible object into an object.
        /// </summary>
        /// <typeparam name="T">The type of object to convert to.</typeparam>
        /// <returns>The new object.</returns>
        public T ToObject<T>()
        {
            return (T)this.ToObject(typeof(T));
        }

        /// <summary>
        /// Converts the convertible object into an object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <returns>The new object.</returns>
        public object ToObject(Type type)
        {
            if (type == null) return null;

            object theObject = Activator.CreateInstance(type);

            for (int i = 0; i < this.Size; i++)
            {
                ValueSetter setter = ValueSetter.GetValueSetter(theObject, this.GetLabel(i));

                if (setter.CanSetValue())
                {
                    setter.SetFormattedValue(this.GetValue(i));
                }
            }

            return theObject;
        }

        /// <summary>
        /// Converts the convertible object into a list of objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to convert to.</typeparam>
        /// <returns>A list of objects.</returns>
        public List<T> ToList<T>()
        {
            return (List<T>)this.ToList(typeof(T));
        }

        /// <summary>
        /// Converts the convertible object into a list of objects.
        /// </summary>
        /// <param name="itemType">The type of objects to convert to.</param>
        /// <returns>A list of objects.</returns>
        public IList ToList(Type itemType)
        {
            if (itemType == null) return null;

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            list.Add(this.ToObject(itemType));

            return list;
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

            foreach (string label in _labels)
            {
                width = Math.Max(width, label.Length);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _labels.Count; i++)
            {
                sb.AppendFormat("    : {0} : {1}", _labels[i].PadLeft(width), _values[i]);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks that all of the values are on the specified object.
        /// </summary>
        /// <param name="actual">The object to check against.</param>
        public bool Check(object actual)
        {
            bool passed = true;

            Type type = actual.GetType();

            for (int i = 0; i < Size; i++)
            {
                string label = GetLabel(i);
                string expectedValue = GetValue(i);

                string propertyName = NameComparer.NormalizeName(label);

                PropertyInfo pi = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (pi != null)
                {
                    object actualValue = pi.GetValue(actual, null);

                    if (expectedValue != string.Format("{0}", actualValue))
                    {
                        _values[i] = string.Format("{0} (was {1})", expectedValue, actualValue);
                        passed = false;
                    }
                }
            }

            return passed;
        }

        private static readonly Regex _formRegex = new Regex(@"^\s*:\s*([^:]+?)\s*:\s*(.+)\s*", RegexOptions.IgnoreCase);

        /// <summary>
        /// Parses the form.
        /// </summary>
        /// <param name="text">The text.</param>
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

            while (i < lines.Count && (m = _formRegex.Match(lines[i])).Success)
            {
                string label = m.Groups[1].Value;
                string value = m.Groups[2].Value;

                form.Add(label, value);

                i++;
            }

            return form;
        }

        /// <summary>
        /// Adds the specified label and value to the form.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="value">The value.</param>
        public void Add(string label, string value)
        {
            _labels.Add(label.Trim());
            _values.Add(value.Trim());
        }

        private List<string> _labels = new List<string>();
        private List<string> _values = new List<string>();

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size
        {
            get { return _labels.Count; }
        }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The label.</returns>
        public string GetLabel(int index)
        {
            return _labels[index];
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The value.</returns>
        public string GetValue(int index)
        {
            return _values[index];
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>The value.</returns>
        public string GetValue(string label)
        {
            return GetValue(_labels.FindIndex(delegate(string s)
            {
                return string.Equals(s, label, StringComparison.OrdinalIgnoreCase);
            }));
        }

        /// <summary>
        /// Determines if the next line is the beginning of a form.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="currentIndex">Index of the current line.</param>
        /// <returns><c>true</c> if the next line is the beginning of a form; otherwise <c>false</c></returns>
        public static bool NextLineIsForm(List<string> lines, int currentIndex)
        {
            return (currentIndex + 1) < lines.Count && _formRegex.IsMatch(lines[currentIndex + 1]);
        }

        internal static IConvertibleObject FromObject(object item, Type itemType)
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
