<%@ Import Namespace = "System.Data" %>
<%@ Import Namespace = "System.Data.SqlClient" %>
<%@ Import Namespace = "System.Drawing" %>
<%@ Import Namespace = "System.Drawing.Imaging" %>
<Script runat="Server">
	dim foreColor as Color = Color.blue
	dim backColor as Color = Color.silver
	dim ratingBackColor as Color = Color.White
	Sub Page_Load(Sender As Object, e As EventArgs)
		if not ((Request.Querystring("rating") is nothing) or Request.Querystring("rating") = "NaN") then
			Dim ratingpoints As double = cdbl(Request.Querystring("rating"))
			Dim MaxImageLength As integer
			Dim MaxImageHeight As integer
			MaxImageHeight = 10 ' pixels
			MaxImageLength = 112 ' pixels
			Dim Rating as Integer
			Rating = Cint(((ratingpoints) / 5) * MaxImageLength)
			Dim objBitmap as Bitmap = new Bitmap(MaxImageLength, MaxImageHeight)
 			Dim objGraphics as Graphics = Graphics.FromImage(objBitmap)
			Dim objBrushRating as SolidBrush = new SolidBrush(foreColor)
			Dim objBrushBorder as SolidBrush = new SolidBrush(backColor)
			Dim objBrushNoRate as SolidBrush = new SolidBrush(ratingBackColor)
			Dim objBrushRatingHighBorder as SolidBrush = new SolidBrush(Color.FromArgb(maxInt(cInt(foreColor.R)+150),maxInt(cint(foreColor.G)+150),maxInt(cint(foreColor.B)+150)))
			Dim objBrushRatingLowBorder as SolidBrush = new SolidBrush(Color.FromArgb(maxInt(cInt(foreColor.R)-60),maxInt(cint(foreColor.G)-60),maxInt(cint(foreColor.B)-60)))
			Dim objBrushBorderDark as SolidBrush = new SolidBrush(Color.FromArgb(maxInt(cInt(backColor.R)-60),maxInt(cint(backColor.G)-60),maxInt(cint(backColor.B)-60)))
			objGraphics.FillRectangle(objBrushBorderDark, 0, 0, MaxImageLength, MaxImageHeight)
			objGraphics.FillRectangle(objBrushBorder, 1, 1, MaxImageLength -2, MaxImageHeight -2)
			objGraphics.FillRectangle(objBrushNoRate, 2, 2, MaxImageLength-3, MaxImageHeight - 4)
			objGraphics.FillRectangle(objBrushRating, 2, 2, Rating - 3, MaxImageHeight - 4)
			objGraphics.FillRectangle(objBrushRatingHighBorder, 2, 2, Rating - 3, 1)
			objGraphics.FillRectangle(objBrushRatingHighBorder, 2, 2, 1, MaxImageHeight - 5)
			objGraphics.FillRectangle(objBrushRatingLowBorder, 2, MaxImageHeight - 3, Rating - 3, 1)
			objGraphics.FillRectangle(objBrushRatingLowBorder, Rating -2, 3, 1, MaxImageHeight - 5)
			'objGraphics.FillRectangle(objBrushBorder, 0, 0, 2 , MaxImageHeight)
			objGraphics.FillRectangle(objBrushBorder, 22, 1, 2 , MaxImageHeight - 2)
			objGraphics.FillRectangle(objBrushBorder, 44, 1, 2 , MaxImageHeight - 2)
			objGraphics.FillRectangle(objBrushBorder, 66, 1, 2 , MaxImageHeight - 2)
			objGraphics.FillRectangle(objBrushBorder, 88, 1, 2 , MaxImageHeight - 2)
			objGraphics.FillRectangle(objBrushBorder, 110, 1, 1 , MaxImageHeight - 2)
			Response.ContentType = "image/png"
			Dim imageStream as new System.IO.MemoryStream
			objBitmap.Save(imageStream, ImageFormat.png)
			imageStream.WriteTo(Response.OutputStream)
			objBitmap.Dispose()
			objGraphics.Dispose()
		else
			Dim objBitmap as Bitmap = new Bitmap(1,1)
 			Dim objGraphics as Graphics = Graphics.FromImage(objBitmap)
 			Dim objBrushRating as SolidBrush = new SolidBrush(Color.Transparent)
 			objGraphics.FillRectangle(objBrushRating,0,0,1,1)
 			Response.ContentType = "image/png"
			Dim imageStream as new System.IO.MemoryStream
			objBitmap.Save(imageStream, ImageFormat.png)
			imageStream.WriteTo(Response.OutputStream)
 			objBitmap.Dispose()
			objGraphics.Dispose()
		end if 
End Sub

Function maxInt(byval value as integer) as Integer
	if (value > 255) then
		return 255
	else
		if (value < 0) then
			return 0
		else
			return value
		end if
	end if
End Function

</Script>