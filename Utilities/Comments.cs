using DotNetNuke.Modules.Wiki.BusinessObjects;
using System;
using System.ComponentModel;
using System.Web.UI;

namespace DotNetNuke.Modules.Wiki.Utilities
{
    [DefaultProperty("ID"), ToolboxData("<{0}:Comments runat=server></{0}:Comments>")]
    public class Comments : System.Web.UI.WebControls.Table
    {
        private string sharedResources = string.Empty;

        /// <summary>
        /// Gets an instance of the unit of work object
        /// </summary>
        internal UnitOfWork Uof
        {
            get
            {
                if (this.uof == null)
                {
                    uof = new UnitOfWork();
                }
                return uof;
            }
        }

        /// <summary>
        /// Gets an instance of the comment business object
        /// </summary>
        public CommentBO CommentBo
        {
            get
            {
                if (commentBo == null)
                {
                    commentBo = new CommentBO(uof);
                }
                return commentBo;
            }
        }

        public bool IsAdmin = false;

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            if (this.uof != null)
            {
                this.uof.Dispose();
                this.uof = null;
            }

            if (this.commentBo != null)
            {
                this.commentBo = null;
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            sharedResources = this.TemplateSourceDirectory + "/" + "DesktopModules/Wiki/App_LocalResources/SharedResources.resx";

            base.OnLoad(e);

            //'Check for delete flag on query string via CommentId (cid)

            if (Context.Request.QueryString["cid"] != null & IsAdmin)
            {
                int commentId = Convert.ToInt32(Context.Request.QueryString["cid"]);
                commentBo.DeleteComment(Convert.ToInt32(commentId));
                this.Context.Cache.Remove("WikiComments" + _parentId.ToString());

                this.Context.Response.Redirect(ReconstructQueryStringWithoutId());
            }
        }

        private string ReconstructQueryStringWithoutId()
        {
            //TODO: make this use NavigateURL
            StringBuilder url = new StringBuilder(128);

            url.Append(Context.Request.Url.Scheme);
            url.Append("://");
            url.Append(Context.Request.Url.Authority);
            url.Append(Context.Request.ApplicationPath);
            url.Append("/Default.aspx?");
            int i = 0;
            for (i = 0; i <= Context.Request.QueryString.Keys.Count - 1; i++)
            {
                string key = Context.Request.QueryString.Keys[i].ToString();
                if (key != "cid")
                {
                    url.Append(key);
                    url.Append("=");
                    url.Append(Context.Request.QueryString[i]);
                    url.Append("&");
                }
            }
            //strip last &
            url.Remove(url.Length - 1, 1);

            return url.ToString();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if ((this.Context != null))
            {
                DataTable dataTable = null;
                if (this._cacheItems)
                {
                    if ((this.Context.Cache["WikiComments" + this._parentId.ToString()] != null))
                    {
                        dataTable = (DataTable)this.Context.Cache["WikiComments" + this._parentId.ToString()];
                    }
                    else
                    {
                        dataTable = commentBo.GetCommentsByParent(this._parentId);
                        this.Context.Cache.Insert("WikiComments" + this._parentId.ToString(), dataTable);
                    }
                }
                else
                {
                    dataTable = commentBo.GetCommentsByParent(this._parentId);
                }

                if ((dataTable != null))
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            this.renderRow(writer, Convert.ToInt32(dataRow["CommentId"]), Convert.ToString(dataRow["Name"]), Convert.ToString(dataRow["Email"]), Convert.ToString(dataRow["Comment"]), (DateTime)dataRow["Datetime"]);
                        }
                    }
                    else
                    {
                        CssClass = "WikiTable";
                        writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
                        writer.RenderBeginTag(HtmlTextWriterTag.Td);

                        writer.Write(Localization.GetString("NoComments.Text", sharedResources));
                        dynamic breakcount2 = _breakCount;
                        _breakCount = 0;
                        writer.RenderEndTag();

                        this.RenderEndTag(writer);
                        _breakCount = breakcount2;
                    }
                }
                else
                {
                    this.renderRow(writer, 1, Localization.GetString("ExampleName.Text", sharedResources), Localization.GetString("ExampleEmail.Text", sharedResources), Localization.GetString("ExampleComments.Text", sharedResources), DateTime.Now);
                }
            }
        }

        private void renderRow(HtmlTextWriter writer, int commentId, string name, string email, string comments, DateTime postDate)
        {
            //Delete the comments
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            if (this._hideEmailAddress)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format(this._hideEmailUrl, commentId));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(name);
                writer.RenderEndTag();
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "mailto:" + email);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(name);
                writer.RenderEndTag();
            }

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            comments = System.Web.HttpUtility.HtmlDecode(comments);
            comments = comments.Replace("" + Strings.Chr(10) + "", "<br />");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(comments);

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (isValidDateFormat(this._dateFormat))
            {
                writer.Write(Localization.GetString("PostedAt", sharedResources) + " " + postDate.ToString(this._dateFormat));
            }
            else
            {
                writer.Write(Localization.GetString("PostedAt", sharedResources) + " " + postDate.ToString(CultureInfo.CurrentCulture));
            }
            writer.RenderEndTag();
            writer.RenderEndTag();

            if (IsAdmin)
            {
                //add delete link here.
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, BuildDeleteQueryString(commentId));
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                //writer.Write("Delete Comment")
                writer.Write(Localization.GetString("DeleteComment", sharedResources));
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "Normal");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write("<hr align=\"left\" width=\"90%\" size=\"1\" noshade=\"noshade\" />");
            writer.RenderEndTag();
            writer.RenderEndTag();
            //Me.RenderEndTag(writer)
            //Me.RenderBeginTag(writer)
        }

        private string BuildDeleteQueryString(int commentId)
        {
            //Build correct url
            string href = Context.Request.Url.ToString();
            href = href + "&cid=" + commentId.ToString();

            return href;
        }

        private bool isValidDateFormat(string format)
        {
            try
            {
                string f = DateTime.Now.ToString(format);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Me.CellSpacing.ToString)
            //writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Me.CellPadding.ToString)
            //writer.AddAttribute(HtmlTextWriterAttribute.Width, Me.Width.ToString)
            //writer.AddAttribute(HtmlTextWriterAttribute.Height, Me.Height.ToString)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass.ToString());
            //writer.AddAttribute(HtmlTextWriterAttribute.Bgcolor, ColorTranslator.ToHtml(Me.BackColor))
            //writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Me.BorderWidth.ToString)
            //writer.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, Me.BorderStyle.ToString)
            //writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, Me.BorderWidth.ToString)
            //writer.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(Me.BorderColor))
            writer.RenderBeginTag(this.TagName);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            if ((this.Context != null))
            {
                int i = 0;
                while (i < this._breakCount)
                {
                    writer.Write("<br />");
                    writer.WriteLine("");
                    System.Math.Min(System.Threading.Interlocked.Increment(ref i), i - 1);
                }
            }
        }

        public static void WriteErrors(HtmlTextWriter writer, string message)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalRed");
            //writer.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial,helvetica")
            //writer.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10pt")
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(message);
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            writer.RenderEndTag();
        }

        #region "Properties"

        [Browsable(false)]
        public override TableRowCollection Rows
        {
            get { return base.Rows; }
        }

        [Browsable(false)]
        public override string BackImageUrl
        {
            get { return base.BackImageUrl; }

            set { }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Table; }
        }

        private int _parentId;

        [Description("The id of the parent (page) the comments are for."), Category("Data")]
        public int ParentId
        {
            get { return this._parentId; }
            set { this._parentId = value; }
        }

        private bool _hideEmailAddress;

        [Description("Whether to suppress displaying the email address. This option should be used in conjunction with the HideEmailUrl property."), Category("Behaviour")]
        public bool HideEmailAddress
        {
            get { return this._hideEmailAddress; }
            set { this._hideEmailAddress = value; }
        }

        private string _hideEmailUrl = "http://localhost/getemail.aspx?commentid={0}";

        [Description("The url that the email address will point to. This enables you to create a page that will show the email address (after a turing test is performed), to stop the email address being 'spam harvested'."), Category("Behaviour")]
        public string HideEmailUrl
        {
            get { return this._hideEmailUrl; }
            set { this._hideEmailUrl = value; }
        }

        private int _breakCount = 1;

        [Description("The number of breaks (<br />) tags between each table."), Category("Appearance")]
        public int BreakCount
        {
            get { return this._breakCount; }
            set { this._breakCount = value; }
        }

        private bool _cacheItems;

        [Description("Caches the comments indefinitely using ASP.NET's caching mechanism. The cache is cleared when a new item is added, but NOT when a comment is deleted in the Manager application."), Category("Behaviour")]
        public bool CacheItems
        {
            get { return this._cacheItems; }
            set { this._cacheItems = value; }
        }

        //Private _dateFormat As String = "dd/MM/yyyy HH:mm"
        //TODO: create a module setting for the date format
        private string _dateFormat = "dd/MM/yyyy HH:mm";

        private UnitOfWork uof;
        private CommentBO commentBo;

        [Description("The format that the date the comment was posted displays in. See the DateTimeFormatInfo for details of the tokens available."), Category("Behaviour")]
        public string DateFormat
        {
            get { return this._dateFormat; }
            set { this._dateFormat = value; }
        }

        #endregion "Properties"
    }
}