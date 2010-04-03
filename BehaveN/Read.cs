// <copyright file="Read.cs" company="Jason Diamond">
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
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Utility class to read strings from various sources.
    /// </summary>
    public static class Read
    {
        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="path">The path to read from.</param>
        public static string File(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        /// <summary>
        /// Reads the embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="name">The name of the embedded resource.</param>
        public static string EmbeddedResource(Assembly assembly, string name)
        {
            string actualName = null;

            foreach (string possibleName in assembly.GetManifestResourceNames())
            {
                if (possibleName == name || possibleName.EndsWith("." + name, StringComparison.OrdinalIgnoreCase))
                {
                    actualName = possibleName;
                    break;
                }
            }

            if (actualName == null)
            {
                throw new Exception(string.Format("Could not find embedded resource named \"{0}\" in assembly named \"{1}\".", name, assembly.FullName));
            }

            using (var stream = assembly.GetManifestResourceStream(actualName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
