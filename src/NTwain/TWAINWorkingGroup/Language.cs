///////////////////////////////////////////////////////////////////////////////////////
//
//  TwainWorkingGroup.TWAIN
//
//  These are the definitions for TWAIN.  They're essentially the C/C++
//  TWAIN.H file contents translated to C#, with modifications that
//  recognize the differences between Windows, Linux and Mac OS X.
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
using System.Text;

namespace TWAINWorkingGroup
{
    /// <summary>
    /// Handle encoding between C# and what the DS is currently set to.
    /// NOTE: this is static for users of this object do not have to track
    ///       the encoding (i.e. let TWAIN.cs deal with all of this). This
    ///       means there is one language for all open DSes, so the last one wins.
    /// </summary>
    static class Language
    {
        /// <summary>
        /// The encoding to use for strings to/from the DS
        /// </summary>
        /// <returns></returns>
        public static Encoding GetEncoding()
        {
            return (m_encoding);
        }

        /// <summary>
        /// The current language of the DS
        /// </summary>
        /// <returns></returns>
        public static void Set(TWLG a_twlg)
        {
            switch (a_twlg)
            {
                default:
                    // NOTE: can only get here if a TWLG was added, but it wasn't added here
                    m_encoding = Encoding.GetEncoding(1252);
                    break;

                case TWLG.USERLOCALE:
                    // NOTE: this should never come back from the DS. only here for completeness
                    m_encoding = Encoding.GetEncoding(1252);
                    break;

                case TWLG.THAI:
                    m_encoding = Encoding.GetEncoding(874);
                    break;

                case TWLG.JAPANESE:
                    m_encoding = Encoding.GetEncoding(932);
                    break;

                case TWLG.CHINESE:
                case TWLG.CHINESE_PRC:
                case TWLG.CHINESE_SINGAPORE:
                case TWLG.CHINESE_SIMPLIFIED:
                    m_encoding = Encoding.GetEncoding(936);
                    break;

                case TWLG.KOREAN:
                case TWLG.KOREAN_JOHAB:
                    m_encoding = Encoding.GetEncoding(949);
                    break;

                case TWLG.CHINESE_HONGKONG:
                case TWLG.CHINESE_TAIWAN:
                case TWLG.CHINESE_TRADITIONAL:
                    m_encoding = Encoding.GetEncoding(950);
                    break;

                case TWLG.ALBANIA:
                case TWLG.CROATIA:
                case TWLG.CZECH:
                case TWLG.HUNGARIAN:
                case TWLG.POLISH:
                case TWLG.ROMANIAN:
                case TWLG.SERBIAN_LATIN:
                case TWLG.SLOVAK:
                case TWLG.SLOVENIAN:
                    m_encoding = Encoding.GetEncoding(1250);
                    break;

                case TWLG.BYELORUSSIAN:
                case TWLG.BULGARIAN:
                case TWLG.RUSSIAN:
                case TWLG.SERBIAN_CYRILLIC:
                case TWLG.UKRANIAN:
                    m_encoding = Encoding.GetEncoding(1251);
                    break;

                case TWLG.AFRIKAANS:
                case TWLG.BASQUE:
                case TWLG.CATALAN:
                case TWLG.DAN: // DANISH
                case TWLG.DUT: // DUTCH
                case TWLG.DUTCH_BELGIAN:
                case TWLG.ENG: // ENGLISH
                case TWLG.ENGLISH_AUSTRALIAN:
                case TWLG.ENGLISH_CANADIAN:
                case TWLG.ENGLISH_IRELAND:
                case TWLG.ENGLISH_NEWZEALAND:
                case TWLG.ENGLISH_SOUTHAFRICA:
                case TWLG.ENGLISH_UK:
                case TWLG.USA:
                case TWLG.FAEROESE:
                case TWLG.FIN: // FINNISH
                case TWLG.FRN: // FRENCH
                case TWLG.FRENCH_BELGIAN:
                case TWLG.FCF: // FRENCH_CANADIAN
                case TWLG.FRENCH_LUXEMBOURG:
                case TWLG.FRENCH_SWISS:
                case TWLG.GER: // GERMAN
                case TWLG.GERMAN_AUSTRIAN:
                case TWLG.GERMAN_LIECHTENSTEIN:
                case TWLG.GERMAN_LUXEMBOURG:
                case TWLG.GERMAN_SWISS:
                case TWLG.ICE: // ICELANDIC
                case TWLG.INDONESIAN:
                case TWLG.ITN: // ITALIAN
                case TWLG.ITALIAN_SWISS:
                case TWLG.NOR: // NORWEGIAN
                case TWLG.NORWEGIAN_BOKMAL:
                case TWLG.NORWEGIAN_NYNORSK:
                case TWLG.POR: // PORTUGUESE
                case TWLG.PORTUGUESE_BRAZIL:
                case TWLG.SPA: // SPANISH
                case TWLG.SPANISH_MEXICAN:
                case TWLG.SPANISH_MODERN:
                case TWLG.SWE: // SWEDISH
                case TWLG.SWEDISH_FINLAND:
                    m_encoding = Encoding.GetEncoding(1252);
                    break;

                case TWLG.GREEK:
                    m_encoding = Encoding.GetEncoding(1253);
                    break;

                case TWLG.TURKISH:
                    m_encoding = Encoding.GetEncoding(1254);
                    break;

                case TWLG.HEBREW:
                    m_encoding = Encoding.GetEncoding(1255);
                    break;

                case TWLG.ARABIC:
                case TWLG.ARABIC_ALGERIA:
                case TWLG.ARABIC_BAHRAIN:
                case TWLG.ARABIC_EGYPT:
                case TWLG.ARABIC_IRAQ:
                case TWLG.ARABIC_JORDAN:
                case TWLG.ARABIC_KUWAIT:
                case TWLG.ARABIC_LEBANON:
                case TWLG.ARABIC_LIBYA:
                case TWLG.ARABIC_MOROCCO:
                case TWLG.ARABIC_OMAN:
                case TWLG.ARABIC_QATAR:
                case TWLG.ARABIC_SAUDIARABIA:
                case TWLG.ARABIC_SYRIA:
                case TWLG.ARABIC_TUNISIA:
                case TWLG.ARABIC_UAE:
                case TWLG.ARABIC_YEMEN:
                case TWLG.FARSI:
                case TWLG.URDU:
                    m_encoding = Encoding.GetEncoding(1256);
                    break;

                case TWLG.ESTONIAN:
                case TWLG.LATVIAN:
                case TWLG.LITHUANIAN:
                    m_encoding = Encoding.GetEncoding(1257);
                    break;

                case TWLG.VIETNAMESE:
                    m_encoding = Encoding.GetEncoding(1258);
                    break;

                case TWLG.ASSAMESE:
                case TWLG.BENGALI:
                case TWLG.BIHARI:
                case TWLG.BODO:
                case TWLG.DOGRI:
                case TWLG.GUJARATI:
                case TWLG.HARYANVI:
                case TWLG.HINDI:
                case TWLG.KANNADA:
                case TWLG.KASHMIRI:
                case TWLG.MALAYALAM:
                case TWLG.MARATHI:
                case TWLG.MARWARI:
                case TWLG.MEGHALAYAN:
                case TWLG.MIZO:
                case TWLG.NAGA:
                case TWLG.ORISSI:
                case TWLG.PUNJABI:
                case TWLG.PUSHTU:
                case TWLG.SIKKIMI:
                case TWLG.TAMIL:
                case TWLG.TELUGU:
                case TWLG.TRIPURI:
                    // NOTE: there are no known code pages for these, so we will use English
                    m_encoding = Encoding.GetEncoding(1252);
                    break;
            }
        }

        private static Encoding m_encoding = MakeDefault();

        private static Encoding MakeDefault()
        {
#if !NETFRAMEWORK
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            return Encoding.GetEncoding(1252);
        }
    }
}
