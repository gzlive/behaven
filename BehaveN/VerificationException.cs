// <copyright file="VerificationException.cs" company="Jason Diamond">
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

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BehaveN
{
    /// <summary>
    /// Represents a failed specification.
    /// </summary>
    public class VerificationException : Exception
    {
        private Regex _stackTraceFilter = new Regex(@"BehaveN(?!\.(Example|Tests))\.");
        private string _message;
        private string _stackTrace;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerificationException"/> class.
        /// </summary>
        /// <param name="originalException">The original exception.</param>
        public VerificationException(Exception originalException)
        {
            _message = originalException.Message;

            if (_message.Contains("\n") && !(_message.StartsWith("\r") || _message.StartsWith("\n")))
            {
                _message = Environment.NewLine + _message;
            }

            _stackTrace = originalException.StackTrace;
        }

        private string FilterStackTrace()
        {
            StringBuilder sb = new StringBuilder();

            StringReader sr;
            string line;

            if (_stackTrace != null)
            {
                sr = new StringReader(_stackTrace);

                while ((line = sr.ReadLine()) != null)
                {
                    if (!_stackTraceFilter.IsMatch(line))
                    {
                        sb.AppendLine(line);
                    }
                }
            }

            sr = new StringReader(base.StackTrace);

            while ((line = sr.ReadLine()) != null)
            {
                if (!_stackTraceFilter.IsMatch(line))
                {
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        /// </returns>
        public override string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets a string representation of the frames on the call stack at the time the current exception was thrown.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// A string that describes the contents of the call stack, with the most recent method call appearing first.
        /// </returns>
        /// <PermissionSet>
        ///     <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        public override string StackTrace
        {
            get { return FilterStackTrace(); }
        }
    }
}
