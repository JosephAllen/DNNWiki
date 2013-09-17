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

        private string _linkClass;
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