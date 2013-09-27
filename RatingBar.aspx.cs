#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="RatingBar.aspx.cs" company="DNN Corp®">
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
//--------------------------------------------------------------------------------------------------------

#endregion Copyright

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DotNetNuke.Wiki
{
    public partial class RatingBar : System.Web.UI.Page
    {
        #region Variables

        private Color foreColor = Color.Blue;
        private Color backColor = Color.Silver;
        private Color ratingBackColor = Color.White;

        #endregion Variables

        #region Events

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!((Request.QueryString["rating"] == null) | Request.QueryString["rating"].Equals("NaN")))
            {
                double ratingpoints = Convert.ToDouble(Request.QueryString["rating"]);
                int MaxImageLength = 0;
                int MaxImageHeight = 0;
                MaxImageHeight = 10;
                // pixels
                MaxImageLength = 112;
                // pixels
                int Rating = 0;
                Rating = Convert.ToInt32(((ratingpoints) / 5) * MaxImageLength);
                Bitmap objBitmap = new Bitmap(MaxImageLength, MaxImageHeight);
                Graphics objGraphics = Graphics.FromImage(objBitmap);
                SolidBrush objBrushRating = new SolidBrush(foreColor);
                SolidBrush objBrushBorder = new SolidBrush(backColor);
                SolidBrush objBrushNoRate = new SolidBrush(ratingBackColor);
                SolidBrush objBrushRatingHighBorder = new SolidBrush(Color.FromArgb(maxInt(Convert.ToInt32(foreColor.R) + 150), maxInt(Convert.ToInt32(foreColor.G) + 150), maxInt(Convert.ToInt32(foreColor.B) + 150)));
                SolidBrush objBrushRatingLowBorder = new SolidBrush(Color.FromArgb(maxInt(Convert.ToInt32(foreColor.R) - 60), maxInt(Convert.ToInt32(foreColor.G) - 60), maxInt(Convert.ToInt32(foreColor.B) - 60)));
                SolidBrush objBrushBorderDark = new SolidBrush(Color.FromArgb(maxInt(Convert.ToInt32(backColor.R) - 60), maxInt(Convert.ToInt32(backColor.G) - 60), maxInt(Convert.ToInt32(backColor.B) - 60)));
                objGraphics.FillRectangle(objBrushBorderDark, 0, 0, MaxImageLength, MaxImageHeight);
                objGraphics.FillRectangle(objBrushBorder, 1, 1, MaxImageLength - 2, MaxImageHeight - 2);
                objGraphics.FillRectangle(objBrushNoRate, 2, 2, MaxImageLength - 3, MaxImageHeight - 4);
                objGraphics.FillRectangle(objBrushRating, 2, 2, Rating - 3, MaxImageHeight - 4);
                objGraphics.FillRectangle(objBrushRatingHighBorder, 2, 2, Rating - 3, 1);
                objGraphics.FillRectangle(objBrushRatingHighBorder, 2, 2, 1, MaxImageHeight - 5);
                objGraphics.FillRectangle(objBrushRatingLowBorder, 2, MaxImageHeight - 3, Rating - 3, 1);
                objGraphics.FillRectangle(objBrushRatingLowBorder, Rating - 2, 3, 1, MaxImageHeight - 5);
                //objGraphics.FillRectangle(objBrushBorder, 0, 0, 2 , MaxImageHeight)
                objGraphics.FillRectangle(objBrushBorder, 22, 1, 2, MaxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 44, 1, 2, MaxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 66, 1, 2, MaxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 88, 1, 2, MaxImageHeight - 2);
                objGraphics.FillRectangle(objBrushBorder, 110, 1, 1, MaxImageHeight - 2);
                Response.ContentType = "image/png";
                System.IO.MemoryStream imageStream = new System.IO.MemoryStream();
                objBitmap.Save(imageStream, ImageFormat.Png);
                imageStream.WriteTo(Response.OutputStream);
                objBitmap.Dispose();
                objGraphics.Dispose();
            }
            else
            {
                Bitmap objBitmap = new Bitmap(1, 1);
                Graphics objGraphics = Graphics.FromImage(objBitmap);
                SolidBrush objBrushRating = new SolidBrush(Color.Transparent);
                objGraphics.FillRectangle(objBrushRating, 0, 0, 1, 1);
                Response.ContentType = "image/png";
                System.IO.MemoryStream imageStream = new System.IO.MemoryStream();
                objBitmap.Save(imageStream, ImageFormat.Png);
                imageStream.WriteTo(Response.OutputStream);
                objBitmap.Dispose();
                objGraphics.Dispose();
            }
        }

        #endregion Events

        #region Aux Functions

        /// <summary>
        /// Maximums the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int maxInt(int value)
        {
            if ((value > 255))
            {
                return 255;
            }
            else
            {
                if ((value < 0))
                {
                    return 0;
                }
                else
                {
                    return value;
                }
            }
        }

        #endregion Aux Functions
    }
}