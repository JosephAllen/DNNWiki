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

Namespace DotNetNuke.Modules.Wiki.Utilities
    Public Class HexEncoding

        Public Sub New()
        End Sub

        'Public Shared Function GetByteCount(ByVal hexString As String) As Integer
        '    Dim numHexChars As Integer = 0
        '    Dim c As Char
        '    Dim i As Integer = 0
        '    While i < hexString.Length
        '        c = hexString.ToCharArray()(i)
        '        If IsHexDigit(c) Then
        '            System.Math.Min(System.Threading.Interlocked.Increment(numHexChars), numHexChars - 1)
        '        End If
        '        System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        '    End While
        '    If Not (numHexChars Mod 2 = 0) Then
        '        System.Math.Max(System.Threading.Interlocked.Decrement(numHexChars), numHexChars + 1)
        '    End If
        '    Return numHexChars / 2
        'End Function

        'Public Shared Function GetBytes(ByVal hexString As String, ByRef discarded As Integer) As Byte()
        '    discarded = 0
        '    Dim newString As String = ""
        '    Dim c As Char
        '    Dim i As Integer = 0
        '    While i < hexString.Length
        '        c = hexString.ToCharArray()(i)
        '        If IsHexDigit(c) Then
        '            newString += c
        '        Else
        '            System.Math.Min(System.Threading.Interlocked.Increment(discarded), discarded - 1)
        '        End If
        '        System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        '    End While
        '    If Not (newString.Length Mod 2 = 0) Then
        '        System.Math.Min(System.Threading.Interlocked.Increment(discarded), discarded - 1)
        '        newString = newString.Substring(0, newString.Length - 1)
        '    End If
        '    Dim byteLength As Integer = newString.Length / 2
        '    Dim bytes(byteLength - 1) As Byte
        '    Dim hex As String
        '    Dim j As Integer = 0
        '    i = 0
        '    While i < bytes.Length
        '        hex = New String(New Char() {newString.ToCharArray()(j), newString.ToCharArray()(j + 1)})
        '        bytes(i) = HexToByte(hex)
        '        j = j + 2
        '        System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        '    End While
        '    Return bytes
        'End Function

        'Public Overloads Shared Function ToString(ByVal bytes As Byte()) As String
        '    Dim hexString As String = ""
        '    Dim i As Integer = 0
        '    While i < bytes.Length
        '        hexString += bytes(i).ToString("X2")
        '        System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
        '    End While
        '    Return hexString
        'End Function

        'Public Shared Function InHexFormat(ByVal hexString As String) As Boolean
        '    Dim hexFormat As Boolean = True
        '    For Each digit As Char In hexString
        '        If Not IsHexDigit(digit) Then
        '            hexFormat = False
        '            ' break 
        '        End If
        '    Next
        '    Return hexFormat
        'End Function

        'Public Shared Function IsHexDigit(ByVal c As Char) As Boolean
        '    Dim numChar As Integer
        '    Dim numA As Integer = Convert.ToInt32("A"c)
        '    Dim num1 As Integer = Convert.ToInt32("0"c)
        '    c = Char.ToUpper(c)
        '    numChar = Convert.ToInt32(c)
        '    If numChar >= numA AndAlso numChar < (numA + 6) Then
        '        Return True
        '    End If
        '    If numChar >= num1 AndAlso numChar < (num1 + 10) Then
        '        Return True
        '    End If
        '    Return False
        'End Function

        'Private Shared Function HexToByte(ByVal hex As String) As Byte
        '    If hex.Length > 2 OrElse hex.Length <= 0 Then
        '        Throw New ArgumentException("hex must be 1 or 2 characters in length")
        '    End If
        '    Dim newByte As Byte = Byte.Parse(hex, System.Globalization.NumberStyles.HexNumber)
        '    Return newByte
        'End Function
    End Class


End Namespace
