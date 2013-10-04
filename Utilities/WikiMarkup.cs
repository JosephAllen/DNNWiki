#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="WikIMarkup.cs" company="DNN Corp®">
//      DNN Corp® - http://www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
//
//      Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//      associated documentation files (the "Software"), to deal in the Software without restriction,
//      including without limitation the rights to use, copy, modify, merge, publish, distribute,
//      sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//      furnished to do so, subject to the following conditions:
//
//      The above copyright notice and this permission notice shall be included in all copies or
//      substantial portions of the Software.
//
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
//      NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//      NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//      DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//      OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
////------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// The Class for Decoding and Encoding WikiMarkup Syntax
    /// </summary>
    public abstract class WikiMarkup
    {
        #region Variables

        protected const RegexOptions CCOptions = RegexOptions.Compiled | RegexOptions.Multiline;
        public const string CloseBracket = "]]";
        public const string OpenBracket = "[[";

        #endregion Variables

        #region Properties

        public DotNetNuke.Entities.Portals.PortalSettings PortalSettings;
        public int TabID = -9999;

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [IgnoreColumn]
        public abstract string Content { get; set; }

        /// <summary>
        /// Gets the content of the rendered.
        /// </summary>
        /// <value>The content of the rendered.</value>
        [IgnoreColumn]
        public string RenderedContent
        {
            get { return this.WikiText(this.Content); }
        }

        /// <summary>
        /// Gets a value indicating whether [can use wiki text].
        /// </summary>
        /// <value><c>true</c> if [can use wiki text]; otherwise, /c>.</value>
        [IgnoreColumn]
        public bool CanUseWikiText
        {
            get
            {
                if (this.TabID != -9999 & (this.PortalSettings != null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Wikis the text.
        /// </summary>
        /// <param name="RawText">The raw text.</param>
        /// <returns>Parsed Text</returns>
        protected string WikiText(string RawText)
        {
            System.Text.StringBuilder ParsedText = new System.Text.StringBuilder(RawText);
            bool parsing = false;
            int firstOpenBracket = 0;
            int secondOpenBracket = 0;
            int firstClosedBracket = 0;
            string workingText = string.Empty;
            int NextSearchSpot = 0;
            parsing = true;
            while (parsing)
            {
                if (secondOpenBracket < 1)
                {
                    firstOpenBracket = RawText.IndexOf(OpenBracket, firstClosedBracket);
                }
                else
                {
                    firstOpenBracket = secondOpenBracket;
                }

                if (firstOpenBracket != -1)
                {
                    NextSearchSpot = firstOpenBracket + OpenBracket.Length;
                    secondOpenBracket = RawText.IndexOf(OpenBracket, NextSearchSpot);
                    firstClosedBracket = RawText.IndexOf(CloseBracket, NextSearchSpot);
                    if (firstClosedBracket != -1 & (secondOpenBracket == -1 | firstClosedBracket < secondOpenBracket))
                    {
                        workingText = RawText.Substring(firstOpenBracket, firstClosedBracket - firstOpenBracket + OpenBracket.Length);
                        RawText = RawText.Replace(workingText, new string('-', workingText.Length));
                        ParsedText.Replace(workingText, this.EvaluateCamelCaseWord(workingText.Substring(2, workingText.Length - 4)));
                        if (secondOpenBracket == -1)
                        {
                            parsing = false;
                        }
                    }
                    else if (secondOpenBracket == -1)
                    {
                        parsing = false;
                    }
                }
                else
                {
                    parsing = false;
                }
            }

            return ParsedText.ToString();
        }

        /// <summary>
        /// TODO: do we need this anymore? Processes the line breaks.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>Parsed String</returns>
        protected string ProcessLineBreaks(string val)
        {
            val.Replace("\\b\\r\\n", "<br />");
            val = Regex.Replace(val, "([^\\>])\\r\\n", "$1<br />", CCOptions);
            //// ...not prefixed with a > (because we don't want to add it after most HTML closing
            //// tags)
            val = Regex.Replace(val, "([biua]\\>)\\r\\n", "$1<br />", CCOptions);
            //// ...prefixed by b> i> u> a> (because we do want it after these HTML tags)
            val = Regex.Replace(val, "\\<br\\>\\r\\n", "<br /><br />", CCOptions);
            //// ...prefixed by <br /> (and this other one)
            return val;
        }

        /// <summary>
        /// Evaluates the camel case word.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>Parsed String</returns>
        protected string EvaluateCamelCaseWord(string val)
        {
            string[] Vals = val.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            // TODO: we need to remove all non-ascii characters from the page links, allow them in
            //       the Title
            switch (Vals.Length)
            {
                case 1:
                    return "<a href=\"" + RemoveHost(DotNetNuke.Common.Globals.NavigateURL(this.TabID, this.PortalSettings, string.Empty, "topic=" + EncodeTitle(HttpUtility.HtmlDecode(Vals[0])))) + "\">" + Vals[0].Replace("<", "<").Replace(">", ">") + "</a>";

                case 2:
                    return "<a href=\"" + RemoveHost(DotNetNuke.Common.Globals.NavigateURL(this.TabID, this.PortalSettings, string.Empty, "topic=" + EncodeTitle(HttpUtility.HtmlDecode(Vals[0])))) + "\">" + Vals[1].Replace("<", "<").Replace(">", ">") + "</a>";

                case 3:
                    int value;
                    if (int.TryParse(Vals[2], out value))
                    {
                        if (Vals[1].Trim().Length < 1)
                        {
                            return "<a href=\"" + RemoveHost(DotNetNuke.Common.Globals.NavigateURL(
                                Convert.ToInt32(Vals[2]),
                                this.PortalSettings,
                                string.Empty,
                                "topic=" + EncodeTitle(HttpUtility.HtmlDecode(Vals[0]))))
                                + "\">" + Vals[0].Replace("<", "<").Replace(">", ">") + "</a>";
                        }
                        else
                        {
                            return "<a href=\"" + RemoveHost(DotNetNuke.Common.Globals.NavigateURL(
                                Convert.ToInt32(Vals[2]),
                                this.PortalSettings,
                                string.Empty,
                                "topic=" + EncodeTitle(HttpUtility.HtmlDecode(Vals[0]))))
                                + "\">" + Vals[1].Replace("<", "<").Replace(">", ">") + "</a>";
                        }
                    }
                    else
                    {
                        if ((Vals[1].Trim().Length < 1))
                        {
                            return "<a href=\"" + RemoveHost(DotNetNuke.Common.Globals.NavigateURL(
                                this.TabID,
                                this.PortalSettings,
                                string.Empty,
                                "topic=" + EncodeTitle(HttpUtility.HtmlDecode(Vals[0]))))
                                + "\">" + Vals[0].Replace("<", "<").Replace(">", ">") + "</a>";
                        }
                        else
                        {
                            return "<a href=\"" + RemoveHost(DotNetNuke.Common.Globals.NavigateURL(
                                this.TabID,
                                this.PortalSettings,
                                string.Empty,
                                "topic=" + EncodeTitle(HttpUtility.HtmlDecode(Vals[0]))))
                                + "\">" + Vals[1].Replace("<", "<").Replace(">", ">") + "</a>";
                        }
                    }

                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Removes the host.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>Parsed String</returns>
        public static string RemoveHost(string val)
        {
            if (val.ToLower().StartsWith("http://"))
            {
                string returnval = val.Substring(7);
                returnval = returnval.Replace(returnval.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0], string.Empty);
                return returnval;
            }
            else if (val.ToLower().StartsWith("https://"))
            {
                string returnval = val.Substring(8);
                returnval = returnval.Replace(returnval.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0], string.Empty);
                return returnval;
            }
            else
            {
                return val;
            }
        }

        /// <summary>
        /// Encodes the title.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>URL Encoded string</returns>
        public static string EncodeTitle(string val)
        {
            return HttpUtility.UrlEncode(val);

            ////Dim encoding As New System.Text.ASCIIEncoding
            ////Dim character As Char
            ////Dim returnval As String
            ////Dim encoded As Boolean

            ////For Each character In val.ToCharArray()

            //// Select Case character Case "+", "=", "~", "#", "%", "&", "*", "\", ":", """",
            //// "<", ">", ".", "?", "/", "-" returnval = returnval + "--" +
            //// Convert.ToByte(character).ToString() + "-" Case Else returnval = returnval +
            //// character End Select

            ////Next
            ////Return returnval
        }

        /// <summary>
        /// Decodes the title.
        /// </summary>
        /// <param name="val">The value.</param>
        /// <returns>Decoded URL</returns>
        public static string DecodeTitle(string val)
        {
            return HttpUtility.UrlDecode(val);
            ////If (val.IndexOf("-") > -1) Then
            ////    Dim encoding As New System.Text.ASCIIEncoding
            ////    Dim returnval As String
            ////    Dim splitup As String() = val.Split("-")
            ////    Dim section As String
            ////    Dim nextIsByte As Boolean
            ////    For Each section In splitup
            ////        If nextIsByte = True Then
            ////            nextIsByte = False
            ////            If section.Length = 0 Then
            ////                nextIsByte = True
            ////            Else
            ////                Dim bytes(0) As Byte
            ////                bytes(0) = Convert.ToByte(section)
            ////                returnval = returnval + encoding.GetString(bytes)
            ////            End If
            ////        ElseIf section.Length = 0 Then
            ////            nextIsByte = True
            ////        Else
            ////            returnval = returnval + section
            ////        End If
            ////    Next
            ////    Return returnval
            ////Else
            ////    Return val
            ////End If
        }

        #endregion Methods
    }
}