///////////////////////////////////////////////////////////////////////////////////////
//
//  TwainWorkingGroup.TWAIN
//
//  This is a wrapper class for basic TWAIN functionality.  It establishes
//  behavior that every application should adhere to.  It also hides OS
//  specific details, so that toolkits or applications can use one unified
//  interface to TWAIN.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            TWAIN       Comment
//  M.McLaughlin    13-Mar-2019     2.4.0.3     Add language code page support for strings
//  M.McLaughlin    13-Nov-2015     2.4.0.0     Updated to latest spec
//  M.McLaughlin    13-Sep-2015     2.3.1.2     DsmMem bug fixes
//  M.McLaughlin    26-Aug-2015     2.3.1.1     Log fix and sync with TWAIN Direct
//  M.McLaughlin    13-Mar-2015     2.3.1.0     Numerous fixes
//  M.McLaughlin    13-Oct-2014     2.3.0.4     Added logging
//  M.McLaughlin    24-Jun-2014     2.3.0.3     Stability fixes
//  M.McLaughlin    21-May-2014     2.3.0.2     64-Bit Linux
//  M.McLaughlin    27-Feb-2014     2.3.0.1     AnyCPU support
//  M.McLaughlin    21-Oct-2013     2.3.0.0     Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2013-2020 Kodak Alaris Inc.
//
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////

using System;

namespace TWAINWorkingGroup
{
    /// <summary>
    /// A quick and dirty CSV reader/writer...
    /// </summary>
    public class CSV
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Public Functions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Functions...

        /// <summary>
        /// Start with an empty string...
        /// </summary>
        public CSV()
        {
            m_szCsv = "";
        }

        /// <summary>
        /// Add an item to a CSV string...
        /// </summary>
        /// <param name="a_szItem">Something to add to the CSV string</param>
        public void Add(string a_szItem)
        {
            // If the item has commas, we need to do work...
            if (a_szItem.Contains(","))
            {
                // If the item has quotes, replace them with paired quotes, then
                // quote it and add it...
                if (a_szItem.Contains("\""))
                {
                    m_szCsv += ((m_szCsv != "") ? "," : "") + "\"" + a_szItem.Replace("\"", "\"\"") + "\"";
                }

                // Otherwise, just quote it and add it...
                else
                {
                    m_szCsv += ((m_szCsv != "") ? "," : "") + "\"" + a_szItem + "\"";
                }
            }

            // If the item has quotes, replace them with escaped quotes, then
            // quote it and add it...
            else if (a_szItem.Contains("\""))
            {
                m_szCsv += ((m_szCsv != "") ? "," : "") + "\"" + a_szItem.Replace("\"", "\"\"") + "\"";
            }

            // Otherwise, just add it...
            else
            {
                m_szCsv += ((m_szCsv != "") ? "," : "") + a_szItem;
            }
        }

        /// <summary>
        /// Clear the record...
        /// </summary>
        public void Clear()
        {
            m_szCsv = "";
        }

        /// <summary>
        /// Get the current CSV string...
        /// </summary>
        /// <returns>The current value of the CSV string</returns>
        public string Get()
        {
            return (m_szCsv);
        }

        /// <summary>
        /// Parse a CSV string...
        /// </summary>
        /// <param name="a_szCsv">A CSV string to parse</param>
        /// <returns>An array if items (some can be CSV themselves)</returns>
        public static string[] Parse(string a_szCsv)
        {
            int ii;
            bool blEnd;
            string[] aszCsv;
            string[] aszLeft;
            string[] aszRight;

            // Validate...
            if ((a_szCsv == null) || (a_szCsv == ""))
            {
                return (new string[] { "" });
            }

            // If there are no quotes, then parse it fast...
            if (!a_szCsv.Contains("\""))
            {
                return (a_szCsv.Split(new char[] { ',' }));
            }

            // There's no opening quote, so split and recurse...
            if (a_szCsv[0] != '"')
            {
                aszLeft = new string[] { a_szCsv.Substring(0, a_szCsv.IndexOf(',')) };
                aszRight = Parse(a_szCsv.Remove(0, a_szCsv.IndexOf(',') + 1));
                aszCsv = new string[aszLeft.Length + aszRight.Length];
                aszLeft.CopyTo(aszCsv, 0);
                aszRight.CopyTo(aszCsv, aszLeft.Length);
                return (aszCsv);
            }

            // Handle the quoted string...
            else
            {
                // Find the terminating quote...
                blEnd = true;
                for (ii = 0; ii < a_szCsv.Length; ii++)
                {
                    if (a_szCsv[ii] == '"')
                    {
                        blEnd = !blEnd;
                    }
                    else if (blEnd && (a_szCsv[ii] == ','))
                    {
                        break;
                    }
                }
                ii -= 1;

                // We have a problem...
                if (!blEnd)
                {
                    throw new Exception("Error in CSV string...");
                }

                // This is the last item, remove any escaped quotes and return it...
                if (((ii + 1) >= a_szCsv.Length))
                {
                    return (new string[] { a_szCsv.Substring(1, a_szCsv.Length - 2).Replace("\"\"", "\"") });
                }

                // We have more data...
                if (a_szCsv[ii + 1] == ',')
                {
                    aszLeft = new string[] { a_szCsv.Substring(1, ii - 1).Replace("\"\"", "\"") };
                    aszRight = Parse(a_szCsv.Remove(0, ii + 2));
                    aszCsv = new string[aszLeft.Length + aszRight.Length];
                    aszLeft.CopyTo(aszCsv, 0);
                    aszRight.CopyTo(aszCsv, aszLeft.Length);
                    return (aszCsv);
                }

                // We have a problem...
                throw new Exception("Error in CSV string...");
            }
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// Our working string for creating or parsing...
        /// </summary>
        private string m_szCsv;

        #endregion
    }
}
