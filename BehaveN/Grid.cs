// <copyright file="Grid.cs" company="Jason Diamond">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a grid.
    /// </summary>
    public class Grid : IBlock
    {
        /// <summary>
        /// Regex that matches lines in a grid.
        /// </summary>
        private static readonly Regex GridRegex = new Regex(@"^\s*\|", RegexOptions.IgnoreCase);

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
        /// Determines if the nexts the line is the beginning of a grid.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="currentIndex">Index of the current line.</param>
        /// <returns><c>true</c> if the next line is the beginning of a grid; otherwise <c>false</c></returns>
        public static bool NextLineIsGrid(IList<string> lines, int currentIndex)
        {
            return (currentIndex + 1) < lines.Count && GridRegex.IsMatch(lines[currentIndex + 1]);
        }

        /// <summary>
        /// Parses the text into a <see cref="Grid">Grid</see> object.
        /// </summary>
        /// <param name="text">The text containing the grid.</param>
        /// <returns>A new <see cref="Grid">Grid</see> object.</returns>
        public static Grid Parse(string text)
        {
            return ParseGrid(TextParser.GetLines(text), 0);
        }

        /// <summary>
        /// Parses a sequence of lines into a <see cref="Grid">Grid</see> object.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="i">The current line index.</param>
        /// <returns>A new <see cref="Grid">Grid</see> object.</returns>
        public static Grid ParseGrid(IList<string> lines, int i)
        {
            Grid grid = new Grid();

            List<string> headers = SplitCells(lines[i]);
            grid.SetHeaders(headers);

            i++;

            while (i < lines.Count && GridRegex.IsMatch(lines[i]))
            {
                grid.AddValues(SplitCells(lines[i]));
                i++;
            }

            return grid;
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
            Type itemType = BlockType.GetCollectionItemType(type);

            if (itemType == null)
            {
                return null;
            }

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            for (int i = 0; i < this.RowCount; i++)
            {
                object item = Activator.CreateInstance(itemType);

                for (int j = 0; j < this.ColumnCount; j++)
                {
                    string header = this.GetHeader(j);
                    ValueSetter setter = ValueSetter.GetValueSetter(item, header);

                    if (setter.CanSetValue())
                    {
                        setter.SetFormattedValue(this.GetValue(i, j));
                    }
                    else
                    {
                        throw new Exception(string.Format("Could not set {0} on type {1}.", header, type.FullName));
                    }
                }

                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Formats this instance.
        /// </summary>
        /// <returns>
        /// A <c>string</c> representing the grid.
        /// </returns>
        public string Format()
        {
            StringBuilder sb = new StringBuilder();

            List<int> columnWidths = new List<int>();

            for (int i = 0; i < this.ColumnCount; i++)
            {
                int width = this.GetHeader(i).Length;

                for (int j = 0; j < this.RowCount; j++)
                {
                    width = Math.Max(width, this.GetValue(j, i).Length);
                }

                columnWidths.Add(width);
            }

            sb.Append("    |");

            for (int i = 0; i < this.ColumnCount; i++)
            {
                sb.Append(" " + this.GetHeader(i).PadLeft(columnWidths[i]) + " ");
                sb.Append("|");
            }

            sb.AppendLine();

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

            IEnumerator enumerator = ((IEnumerable)actual).GetEnumerator();

            int i;

            for (i = 0; i < this.RowCount; i++)
            {
                if (enumerator.MoveNext())
                {
                    object current = enumerator.Current;

                    for (int j = 0; j < this.ColumnCount; j++)
                    {
                        string header = this.GetHeader(j);

                        PropertyInfo pi = this.GetPropertyInfo(current.GetType(), header);

                        if (pi != null)
                        {
                            object actualValue = pi.GetValue(current, null);
                            object expectedValue = ValueParser.ParseValue(this.GetValue(i, j), pi.PropertyType);

                            if (!object.Equals(actualValue, expectedValue))
                            {
                                this.rows[i][j] = string.Format("{0} (was {1})", expectedValue, actualValue);
                                passed = false;
                            }
                        }
                        else
                        {
                            this.rows[i][j] = string.Format("{0} (unknown)", this.GetValue(i, j));
                            passed = false;
                        }
                    }
                }
                else
                {
                    passed = false;

                    string expectedValue = this.GetValue(i, 0);
                    this.rows[i][0] = string.Format("(missing) {0}", expectedValue);
                }
            }

            while (enumerator.MoveNext())
            {
                passed = false;

                this.AddValues(new List<string>(new string[this.ColumnCount]));

                object current = enumerator.Current;

                for (int j = 0; j < this.ColumnCount; j++)
                {
                    string header = this.GetHeader(j);

                    PropertyInfo pi = this.GetPropertyInfo(current.GetType(), header);

                    if (pi != null)
                    {
                        object actualValue = pi.GetValue(current, null);

                        if (j == 0)
                        {
                            this.rows[i][j] = string.Format("(unexpected) {0}", actualValue);
                        }
                        else
                        {
                            this.rows[i][j] = string.Format("{0}", actualValue);
                        }
                    }
                }

                i++;
            }

            return passed;
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
        /// Converts a list of objects into a grid.
        /// </summary>
        /// <param name="list">The list of objects to convert.</param>
        /// <param name="itemType">Type of the objects in the list.</param>
        /// <returns>A new Grid object.</returns>
        internal static Grid FromList(IEnumerable list, Type itemType)
        {
            Grid grid = new Grid();

            List<string> propertyNames = new List<string>();

            foreach (PropertyInfo pi in itemType.GetProperties())
            {
                if (pi.CanRead && pi.GetGetMethod().GetParameters().Length == 0)
                {
                    propertyNames.Add(pi.Name);
                }
            }

            grid.SetHeaders(propertyNames);

            foreach (object item in list)
            {
                List<string> propertyValues = new List<string>();

                foreach (string propertyName in propertyNames)
                {
                    object propertyValue = itemType.GetProperty(propertyName).GetValue(item, null);

                    propertyValues.Add(propertyValue != null ? propertyValue.ToString() : "null");
                }

                grid.AddValues(propertyValues);
            }

            return grid;
        }

        /// <summary>
        /// Splits the cells in a line.
        /// </summary>
        /// <param name="line">The line of text to split.</param>
        /// <returns>A list of values in the cells.</returns>
        private static List<string> SplitCells(string line)
        {
            line = line.Trim();
            line = line.Trim('|');

            List<string> cells = new List<string>();

            foreach (string cell in line.Split('|'))
            {
                cells.Add(cell.Trim());
            }

            return cells;
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
