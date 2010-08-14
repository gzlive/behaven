// <copyright file="Grid.cs" company="Jason Diamond">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Represents a grid.
    /// </summary>
    public class Grid : IBlock
    {
        /// <summary>
        /// The list of header names.
        /// </summary>
        private readonly List<string> headers = new List<string>();

        /// <summary>
        /// The list of row values.
        /// </summary>
        private readonly List<List<string>> rows = new List<List<string>>();

        /// <summary>
        /// Gets the column count.
        /// </summary>
        /// <value>The column count.</value>
        public int ColumnCount
        {
            get { return this.headers.Count; }
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>The row count.</value>
        public int RowCount
        {
            get { return this.rows.Count; }
        }

        /// <summary>
        /// Gets the header at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The header.</returns>
        public string GetHeader(int index)
        {
            return this.headers[index];
        }

        /// <summary>
        /// Sets the headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public void SetHeaders(List<string> headers)
        {
            this.headers.Clear();
            this.headers.AddRange(headers);
        }

        /// <summary>
        /// Gets the value at the specified row and column indexes.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>The value in the cell.</returns>
        public string GetValue(int rowIndex, int columnIndex)
        {
            return this.rows[rowIndex][columnIndex];
        }

        /// <summary>
        /// Gets the value at the specified row index with the specified header name.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <returns>The value in the cell.</returns>
        public string GetValue(int rowIndex, string headerName)
        {
            return this.GetValue(
                rowIndex,
                this.headers.FindIndex(s => string.Equals(s, headerName, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// Adds a row of values.
        /// </summary>
        /// <param name="values">The values.</param>
        public void AddValues(List<string> values)
        {
            this.rows.Add(new List<string>(values));
        }

        /// <summary>
        /// Converts the block into an object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <returns>The new object.</returns>
        public object ConvertTo(Type type)
        {
            Type itemType = TypeExtensions.GetCollectionItemType(type);

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++)
            {
                object item = CreateItem(itemType, rowIndex);

                list.Add(item);
            }

            return list;
        }

        private object CreateItem(Type itemType, int rowIndex)
        {
            object item = Activator.CreateInstance(itemType);

            for (int columnIndex = 0; columnIndex < this.ColumnCount; columnIndex++)
            {
                string header = this.GetHeader(columnIndex);
                ValueSetter setter = ValueSetter.GetValueSetter(item, header);

                if (setter.CanSetValue())
                {
                    setter.SetFormattedValue(this.GetValue(rowIndex, columnIndex));
                }
                else
                {
                    throw new Exception(string.Format("Could not set {0} on type {1}.", header, itemType.FullName));
                }
            }

            return item;
        }

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns>
        /// A <c>string</c> representing the grid.
        /// </returns>
        public string Format()
        {
            List<int> columnWidths = GetColumnWidths();

            var sb = new StringBuilder();

            FormatHeader(sb, columnWidths);
            FormatRows(sb, columnWidths);

            return sb.ToString();
        }

        private List<int> GetColumnWidths()
        {
            var columnWidths = new List<int>();

            for (int i = 0; i < this.ColumnCount; i++)
            {
                int width = this.GetHeader(i).Length;

                for (int j = 0; j < this.RowCount; j++)
                {
                    width = Math.Max(width, this.GetValue(j, i).Length);
                }

                columnWidths.Add(width);
            }

            return columnWidths;
        }

        private void FormatHeader(StringBuilder sb, List<int> columnWidths)
        {
            sb.Append("    |");

            for (int i = 0; i < this.ColumnCount; i++)
            {
                sb.Append(" " + this.GetHeader(i).PadLeft(columnWidths[i]) + " ");
                sb.Append("|");
            }

            sb.AppendLine();
        }

        private void FormatRows(StringBuilder sb, List<int> columnWidths)
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                sb.Append("    |");

                for (int j = 0; j < this.ColumnCount; j++)
                {
                    sb.Append(" " + this.GetValue(i, j).PadLeft(columnWidths[j]) + " ");
                    sb.Append("|");
                }

                sb.AppendLine();
            }
        }

        /// <summary>
        /// Checks that all of the values are on the specified object.
        /// </summary>
        /// <param name="actual">The object to check against.</param>
        /// <returns>True if all checks pass.</returns>
        public bool Check(object actual)
        {
            IEnumerator enumerator = ((IEnumerable)actual).GetEnumerator();

            bool passed = CheckExpectedRows(enumerator);

            passed &= AddUnexpectedRows(enumerator);

            return passed;
        }

        private bool CheckExpectedRows(IEnumerator enumerator)
        {
            bool passed = true;

            for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++)
            {
                if (enumerator.MoveNext())
                {
                    passed &= CheckExpectedRow(enumerator, rowIndex);
                }
                else
                {
                    string expectedValue = this.GetValue(rowIndex, 0);
                    this.rows[rowIndex][0] = string.Format("(missing) {0}", expectedValue);
                    passed = false;
                }
            }

            return passed;
        }

        private bool CheckExpectedRow(IEnumerator enumerator, int rowIndex)
        {
            bool passed = true;

            object current = enumerator.Current;

            for (int columnIndex = 0; columnIndex < this.ColumnCount; columnIndex++)
            {
                passed &= CheckExpectedCell(columnIndex, current, rowIndex);
            }

            return passed;
        }

        private bool CheckExpectedCell(int columnIndex, object current, int rowIndex)
        {
            bool passed = true;

            string header = this.GetHeader(columnIndex);
            PropertyInfo pi = this.GetPropertyInfo(current.GetType(), header);

            if (pi != null)
            {
                object actualValue = pi.GetValue(current, null);
                object expectedValue = ValueParser.ParseValue(this.GetValue(rowIndex, columnIndex), pi.PropertyType);

                if (!object.Equals(actualValue, expectedValue))
                {
                    this.rows[rowIndex][columnIndex] = string.Format("{0} (was {1})", expectedValue, actualValue);
                    passed = false;
                }
            }
            else
            {
                this.rows[rowIndex][columnIndex] = string.Format("{0} (unknown)", this.GetValue(rowIndex, columnIndex));
                passed = false;
            }

            return passed;
        }

        private bool AddUnexpectedRows(IEnumerator enumerator)
        {
            bool passed = true;

            while (enumerator.MoveNext())
            {
                List<string> values = GetUnexpectdValues(enumerator);

                this.AddValues(values);

                passed = false;
            }

            return passed;
        }

        private List<string> GetUnexpectdValues(IEnumerator enumerator)
        {
            var values = new List<string>();

            object current = enumerator.Current;

            for (int columnIndex = 0; columnIndex < this.ColumnCount; columnIndex++)
            {
                string header = this.GetHeader(columnIndex);
                PropertyInfo pi = this.GetPropertyInfo(current.GetType(), header);

                object actualValue = pi != null ? pi.GetValue(current, null) : "(unknown)";

                values.Add(string.Format("{0}", actualValue));
            }

            values[0] = string.Format("(unexpected) {0}", values[0]);

            return values;
        }

        /// <summary>
        /// Gets the suggested type for the parameter.
        /// </summary>
        /// <returns>The suggested type.</returns>
        public string GetSuggestedParameterType()
        {
            return "List<Foo>";
        }

        /// <summary>
        /// Gets the suggested name for the parameter.
        /// </summary>
        /// <returns>The suggested name.</returns>
        public string GetSuggestedParameterName()
        {
            return "foos";
        }

        /// <summary>
        /// Gets the suggested type definition for the parameter.
        /// </summary>
        /// <returns>The suggested type definition.</returns>
        public string GetSuggestedParameterTypeDefinition()
        {
            string code = "public class Foo\r\n" +
                          "{\r\n";

            foreach (string header in this.headers)
            {
                code += string.Format("    public string {0} {{ get; set; }}\r\n", header);
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
            if (reporter is IBlockReporter<Grid>)
            {
                ((IBlockReporter<Grid>)reporter).ReportBlock(this);
            }
            else
            {
                throw new Exception(string.Format("{0} doesn't support reporting grids.", reporter.GetType().FullName));
            }
        }

        /// <summary>
        /// Gets the property info specified by the header.
        /// </summary>
        /// <param name="type">The type containing the property.</param>
        /// <param name="header">The header.</param>
        /// <returns>The property info.</returns>
        private PropertyInfo GetPropertyInfo(Type type, string header)
        {
            string propertyName = NameComparer.NormalizeName(header);
            return type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }
    }
}
