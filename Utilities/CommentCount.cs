#region Copyright

//
// DotNetNuke� - http://www.dotnetnuke.com Copyright (c) 2002-2013 by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion Copyright

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects;
using System.ComponentModel;
using System.Web.UI;

namespace DotNetNuke.Wiki.Utilities
{
    [DefaultProperty("ID"), ToolboxData("<{0}:CommentCount runat=server></{0}:CommentCount>")]
    public class CommentCount : System.Web.UI.WebControls.Label
    {
        private string sharedResources = string.Empty;

        [Description("The id of the parent (page) the comment count is for."), Category("Data")]
        public int ParentId
        {
            get { return this._parentId; }
            set { this._parentId = value; }
        }

        [Description("The text for the link. {0} will be replaced with the number of comments."), Category("Appearance")]
        public new string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }

        private string _text;

        private int _parentId;

        protected override void RenderContents(HtmlTextWriter writer)
        {
            using (UnitOfWork uof = new UnitOfWork())
            {
                var commentBo = new CommentBO(uof);

                int commentCount = commentBo.GetCommentCount(this._parentId);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                if (commentCount > 0)
                {
                    // writer.Write(String.Format(Me._text, commentCount))

                    if (this._text == null)
                    {
                        writer.Write(string.Format(Localization.GetString("FeedBack.Text", sharedResources), commentCount));
                    }
                    else
                    {
                        writer.Write(string.Format(this._text, commentCount));
                    }
                }
                else
                {
                    writer.Write(Localization.GetString("NoComments.Text", sharedResources));
                }
                writer.RenderEndTag();
            }
        }
    }
}