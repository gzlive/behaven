using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents a grid.
    /// </summary>
    public class Grid : IBlock
    {
        private List<string> _headers = new List<string>();
        private List<List<string>> _rows = new List<List<string>>();

        /// <summary>
        /// Gets the column count.
        /// </summary>
        /// <value>The column count.</value>
        public int ColumnCount
        {
            get { return _headers.Count; }
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>The row count.</value>
        public int RowCount
        {
            get { return _rows.Count; }
        }

        /// <summary>
        /// Gets the header at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public string GetHeader(int index)
        {
            return _headers[index];
        }

        /// <summary>
        /// Sets the headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        public void SetHeaders(List<string> headers)
        {
            _headers.Clear();
            _headers.AddRange(headers);
        }

        /// <summary>
        /// Gets the value at the specified row and column indexes.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="columnIndex">Index of the column.</param>
        /// <returns>The value in the cell.</returns>
        public string GetValue(int rowIndex, int columnIndex)
        {
            return _rows[rowIndex][columnIndex];
        }

        /// <summary>
        /// Gets the value at the specified row index with the specified header name.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <returns>The value in the cell.</returns>
        public string GetValue(int rowIndex, string headerName)
        {
            return GetValue(rowIndex, _headers.FindIndex(delegate(string s)
            {
                return string.Equals(s, headerName, StringComparison.OrdinalIgnoreCase);
            }));
        }

        /// <summary>
        /// Adds a row of values.
        /// </summary>
        /// <param name="values">The values.</param>
        public void AddValues(List<string> values)
        {
            _rows.Add(new List<string>(values));
        }

        /// <summary>
        /// Converts the block into an object.
        /// </summary>
        /// <param name="type">The type of object to convert to.</param>
        /// <returns>The new object.</returns>
        public object ConvertTo(Type type)
        {
            Type itemType = BlockType.GetCollectionItemType(type);

            if (itemType == null) return null;

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            for (int i = 0; i < this.RowCount; i++)
            {
                object item = Activator.CreateInstance(itemType);

                for (int j = 0; j < this.ColumnCount; j++)
                {
                    ValueSetter setter = ValueSetter.GetValueSetter(item, this.GetHeader(j));

                    if (setter.CanSetValue())
                    {
                        setter.SetFormattedValue(this.GetValue(i, j));
                    }
                }

                list.Add(item);
            }

            return list;
        }

        private static readonly Regex _gridRegex = new Regex(@"^\s*\|", RegexOptions.IgnoreCase);

        /// <summary>
        /// Determines if the nexts the line is the beginning of a grid.
        /// </summary>
        /// <param name="lines">The lines.</param>
        /// <param name="currentIndex">Index of the current line.</param>
        /// <returns><c>true</c> if the next line is the beginning of a grid; otherwise <c>false</c></returns>
        public static bool NextLineIsGrid(IList<string> lines, int currentIndex)
        {
            return (currentIndex + 1) < lines.Count && _gridRegex.IsMatch(lines[currentIndex + 1]);
        }

        /// <summary>
        /// Parses the text into a <see cref="Grid">Grid</see> object.
        /// </summary>
        /// <param name="text">The text.</param>
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

            while (i < lines.Count && _gridRegex.IsMatch(lines[i]))
            {
                grid.AddValues(SplitCells(lines[i]));
                i++;
            }

            return grid;
        }

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
        /// Formats this instance.
        /// </summary>
        /// <returns>
        /// A <c>string</c> representing the grid.
        /// </returns>
        public string Format()
        {
            StringBuilder sb = new StringBuilder();

            List<int> columnWidths = new List<int>();

            for (int i = 0; i < ColumnCount; i++)
            {
                int width = GetHeader(i).Length;

                for (int j = 0; j < RowCount; j++)
                {
                    width = Math.Max(width, GetValue(j, i).Length);
                }

                columnWidths.Add(width);
            }

            sb.Append("    |");

            for (int i = 0; i < ColumnCount; i++)
            {
                sb.Append(" " + GetHeader(i).PadLeft(columnWidths[i]) + " ");
                sb.Append("|");
            }

            sb.AppendLine();

            for (int i = 0; i < RowCount; i++)
            {
                sb.Append("    |");

                for (int j = 0; j < ColumnCount; j++)
                {
                    sb.Append(" " + GetValue(i, j).PadLeft(columnWidths[j]) + " ");
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
        public bool Check(object actual)
        {
            bool passed = true;

            IEnumerator enumerator = ((IEnumerable)actual).GetEnumerator();

            int i;

            for (i = 0; i < RowCount; i++)
            {
                if (enumerator.MoveNext())
                {
                    object current = enumerator.Current;

                    for (int j = 0; j < ColumnCount; j++)
                    {
                        string header = GetHeader(j);

                        PropertyInfo pi = GetPropertyInfo(current.GetType(), header);

                        if (pi != null)
                        {
                            object actualValue = pi.GetValue(current, null);
                            object expectedValue = ValueParser.ParseValue(GetValue(i, j), pi.PropertyType);

                            if (!object.Equals(actualValue, expectedValue))
                            {
                                _rows[i][j] = string.Format("{0} (was {1})", expectedValue, actualValue);
                                passed = false;
                            }
                        }
                    }
                }
                else
                {
                    passed = false;

                    string expectedValue = GetValue(i, 0);
                    _rows[i][0] = string.Format("(missing) {0}", expectedValue);
                }
            }

            while (enumerator.MoveNext())
            {
                passed = false;

                AddValues(new List<string>(new string[ColumnCount]));

                object current = enumerator.Current;

                for (int j = 0; j < ColumnCount; j++)
                {
                    string header = GetHeader(j);

                    PropertyInfo pi = GetPropertyInfo(current.GetType(), header);

                    if (pi != null)
                    {
                        object actualValue = pi.GetValue(current, null);

                        if (j == 0)
                            _rows[i][j] = string.Format("(unexpected) {0}", actualValue);
                        else
                            _rows[i][j] = string.Format("{0}", actualValue);
                    }
                }

                i++;
            }

            return passed;
        }

        /// <summary>
        /// Gets the suggested type for the parameter.
        /// </summary>
        /// <returns>The type.</returns>
        public string GetSuggestedParameterType()
        {
            return "List<Foo>";
        }

        /// <summary>
        /// Gets the suggested name for the parameter.
        /// </summary>
        /// <returns>The name.</returns>
        public string GetSuggestedParameterName()
        {
            return "foos";
        }

        private PropertyInfo GetPropertyInfo(Type type, string header)
        {
            string propertyName = NameComparer.NormalizeName(header);
            return type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        }

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
    }
}
