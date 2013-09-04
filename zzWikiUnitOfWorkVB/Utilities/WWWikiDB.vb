'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2013
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'

Imports System
Imports System.Text.RegularExpressions

Namespace DotNetNuke.Modules.Wiki.Entities

    Public MustInherit Class WikiData
        Public Const OPEN_BRACKET As String = "[["
        Public Const CLOSE_BRACKET As String = "]]"
        Public TabID As Integer = -9999
        Public PortalSettings As DotNetNuke.Entities.Portals.PortalSettings
        Protected Const C_C_Options As RegexOptions = RegexOptions.Compiled Or RegexOptions.Multiline

        Public MustOverride Property Content() As String
        Public ReadOnly Property RenderedContent() As String
            Get
                Return WikiText(Content)
            End Get
        End Property

        Public ReadOnly Property CanUseWikiText()
            Get
                If TabID <> -9999 And Not PortalSettings Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Protected Function WikiText(ByVal RawText As String) As String
            Dim ParsedText As New System.Text.StringBuilder(RawText)
            Dim parsing As Boolean
            Dim fob As Integer = 0
            Dim sob As Integer = 0
            Dim fcb As Integer = 0
            Dim workingText As String = ""
            Dim NextSearchSpot As Integer
            parsing = True
            While parsing
                If (sob < 1) Then
                    fob = RawText.IndexOf(OPEN_BRACKET, fcb)
                Else
                    fob = sob
                End If
                If fob <> -1 Then
                    NextSearchSpot = fob + OPEN_BRACKET.Length
                    sob = RawText.IndexOf(OPEN_BRACKET, NextSearchSpot)
                    fcb = RawText.IndexOf(CLOSE_BRACKET, NextSearchSpot)
                    If fcb <> -1 And (sob = -1 Or fcb < sob) Then
                        workingText = RawText.Substring(fob, fcb - fob + OPEN_BRACKET.Length)
                        RawText = RawText.Replace(workingText, New String("-", workingText.Length))
                        ParsedText.Replace(workingText, EvaluateCamelCaseWord(workingText.Substring(2, workingText.Length - 4)))
                        If sob = -1 Then
                            parsing = False
                        End If
                    ElseIf sob = -1 Then
                        parsing = False
                    End If
                Else
                    parsing = False
                End If
            End While
            Return ParsedText.ToString()
        End Function

        'TODO: do we need this anymore?
        Protected Function ProcessLineBreaks(ByVal val As String) As String
            val.Replace("\b\r\n", "<br />")
            val = Regex.Replace(val, "([^\>])\r\n", "$1<br />", C_C_Options)  ' ...not prefixed with a > (because we don't want to add it after most HTML closing tags)
            val = Regex.Replace(val, "([biua]\>)\r\n", "$1<br />", C_C_Options) ' ...prefixed by b> i> u> a> (because we do want it after these HTML tags)
            val = Regex.Replace(val, "\<br\>\r\n", "<br /><br />", C_C_Options)  ' ...prefixed by <br /> (and this other one)
            Return val
        End Function

        Protected Function EvaluateCamelCaseWord(ByVal val As String) As String
            Dim Vals() As String = val.Split("|")
            'TODO: we need to remove all non-ascii characters from the page links, allow them in the Title
            Select Case Vals.Length
                Case 1
                    Return "<a href=""" + RemoveHost(DotNetNuke.Common.NavigateURL(Me.TabID, Me.PortalSettings, String.Empty, "topic=" & EncodeTitle(HttpUtility.HtmlDecode(Vals(0))))) + """>" + Vals(0).Replace("<", "&lt;").Replace(">", "&gt;") + "</a>"
                Case 2
                    Return "<a href=""" + RemoveHost(DotNetNuke.Common.NavigateURL(Me.TabID, Me.PortalSettings, String.Empty, "topic=" & EncodeTitle(HttpUtility.HtmlDecode(Vals(0))))) + """>" + Vals(1).Replace("<", "&lt;").Replace(">", "&gt;") + "</a>"
                Case 3
                    If IsNumeric(Vals(2)) Then
                        If (Vals(1).Trim().Length < 1) Then
                            Return "<a href=""" + RemoveHost(DotNetNuke.Common.NavigateURL(Convert.ToInt32(Vals(2)), Me.PortalSettings, String.Empty, "topic=" & EncodeTitle(HttpUtility.HtmlDecode(Vals(0))))) + """>" + Vals(0).Replace("<", "&lt;").Replace(">", "&gt;") + "</a>"
                        Else
                            Return "<a href=""" + RemoveHost(DotNetNuke.Common.NavigateURL(Convert.ToInt32(Vals(2)), Me.PortalSettings, String.Empty, "topic=" & EncodeTitle(HttpUtility.HtmlDecode(Vals(0))))) + """>" + Vals(1).Replace("<", "&lt;").Replace(">", "&gt;") + "</a>"
                        End If
                    Else
                        If (Vals(1).Trim().Length < 1) Then
                            Return "<a href=""" + RemoveHost(DotNetNuke.Common.NavigateURL(Me.TabID, Me.PortalSettings, String.Empty, "topic=" & EncodeTitle(HttpUtility.HtmlDecode(Vals(0))))) + """>" + Vals(0).Replace("<", "&lt;").Replace(">", "&gt;") + "</a>"
                        Else
                            Return "<a href=""" + RemoveHost(DotNetNuke.Common.NavigateURL(Me.TabID, Me.PortalSettings, String.Empty, "topic=" & EncodeTitle(HttpUtility.HtmlDecode(Vals(0))))) + """>" + Vals(1).Replace("<", "&lt;").Replace(">", "&gt;") + "</a>"
                        End If
                    End If
            End Select
        End Function

        Public Shared Function make255(ByVal title As String) As String
            If (title.Length > 255) Then
                Return title.Substring(0, 255)
            Else
                Return title
            End If
        End Function
        Public Shared Function RemoveHost(ByVal val As String) As String
            If (val.ToLower().StartsWith("http://")) Then
                Dim returnval As String = val.Substring(7)
                returnval = returnval.Replace(returnval.Split("/")(0), "")
                Return returnval
            ElseIf (val.ToLower().StartsWith("https://")) Then
                Dim returnval As String = val.Substring(8)
                returnval = returnval.Replace(returnval.Split("/")(0), "")
                Return returnval
            Else
                Return val
            End If
        End Function
        Public Shared Function EncodeTitle(ByVal val As String) As String
            Return HttpUtility.UrlEncode(val)

            'Dim encoding As New System.Text.ASCIIEncoding
            'Dim character As Char
            'Dim returnval As String
            'Dim encoded As Boolean

            'For Each character In val.ToCharArray()

            '    Select Case character
            '        Case "+", "=", "~", "#", "%", "&", "*", "\", ":", """", "<", ">", ".", "?", "/", "-"
            '            returnval = returnval + "--" + Convert.ToByte(character).ToString() + "-"
            '        Case Else
            '            returnval = returnval + character
            '    End Select

            'Next
            'Return returnval
        End Function

        Public Shared Function DecodeTitle(ByVal val As String) As String

            Return HttpUtility.UrlDecode(val)
            'If (val.IndexOf("-") > -1) Then
            '    Dim encoding As New System.Text.ASCIIEncoding
            '    Dim returnval As String
            '    Dim splitup As String() = val.Split("-")
            '    Dim section As String
            '    Dim nextIsByte As Boolean
            '    For Each section In splitup
            '        If nextIsByte = True Then
            '            nextIsByte = False
            '            If section.Length = 0 Then
            '                nextIsByte = True
            '            Else
            '                Dim bytes(0) As Byte
            '                bytes(0) = Convert.ToByte(section)
            '                returnval = returnval + encoding.GetString(bytes)
            '            End If
            '        ElseIf section.Length = 0 Then
            '            nextIsByte = True
            '        Else
            '            returnval = returnval + section
            '        End If
            '    Next
            '    Return returnval
            'Else
            '    Return val
            'End If
        End Function
    End Class


End Namespace
