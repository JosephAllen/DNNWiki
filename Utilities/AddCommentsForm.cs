#region Copyright

// <copyright file="AddCommentsForm.cs" company="DNN Corp®"> DNN Corp® - http://www.dnnsoftware.com
// Copyright (c) 2002-2013 by DNN Corp®
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
// </copyright>
////

#endregion Copyright

using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.BusinessObjects;
using DotNetNuke.Wiki.BusinessObjects.Models;
using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// Cancel Handler
    /// </summary>
    /// <param name="s">The arguments.</param>
    public delegate void CancelHandler(object s);

    /// <summary>
    /// Submit Handler
    /// </summary>
    /// <param name="s">The arguments.</param>
    public delegate void SubmitHandler(object s);

    /// <summary>
    /// Web Controls for the Comments form
    /// </summary>
    [ToolboxData("<{0}:AddCommentsForm runat=server></{0}:AddCommentsForm>")]
    public class AddCommentsForm : WebControl
    {
        #region Variables

        private bool mCheckCommentsValue = true;
        private bool mCheckEmailValue = true;
        private bool mCheckNameValue = true;
        private int mCommentsMaxLengthValue = 500;
        private bool mSuccessValue = true;

        #endregion Variables

        #region Events

        /// <summary>
        /// Occurs when [post canceled].
        /// </summary>
        public event CancelHandler PostCanceled;

        /// <summary>
        /// Occurs when [post submitted].
        /// </summary>
        public event SubmitHandler PostSubmitted;

        #endregion Events

        #region Controls

        /// <summary>
        /// The comments control
        /// </summary>
        protected System.Web.UI.WebControls.TextBox Comment;

        /// <summary>
        /// The email control
        /// </summary>
        protected System.Web.UI.WebControls.TextBox Email;

        /// <summary>
        /// The label parent unique identifier
        /// </summary>
        protected System.Web.UI.WebControls.Label LblParentID;

        /// <summary>
        /// The name control
        /// </summary>
        protected System.Web.UI.WebControls.TextBox Name;

        /// <summary>
        /// The subscribe automatic notifications
        /// </summary>
        protected System.Web.UI.WebControls.CheckBox SubscribeToNotifications;

        /// <summary>
        /// The with events field_ cancel button
        /// </summary>
        private System.Web.UI.WebControls.LinkButton withEventsField_CancelButton;

        /// <summary>
        /// The with events field_ submit button
        /// </summary>
        private System.Web.UI.WebControls.LinkButton withEventsField_SubmitButton;

        #endregion Controls

        //// private Entities.CommentsController commentBo = new Entities.CommentsController();

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [check comments].
        /// </summary>
        /// <value><c>true</c> if [check comments]; otherwise, /c>.</value>
        [Description("Whether to check if the comments field is empty before submitting."), Category("Behaviour")]
        public bool CheckComments
        {
            get { return this.mCheckCommentsValue; }
            set { this.mCheckCommentsValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [check email].
        /// </summary>
        /// <value><c>true</c> if [check email]; otherwise, /c>.</value>
        [Description("Whether to check (clientside) the email address is valid before the user can submit the form."), Category("Behaviour")]
        public bool CheckEmail
        {
            get { return this.mCheckEmailValue; }
            set { this.mCheckEmailValue = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [check name].
        /// </summary>
        /// <value><c>true</c> if [check name]; otherwise, /c>.</value>
        [Description("Whether to check if the name field is empty before submitting."), Category("Behaviour")]
        public bool CheckName
        {
            get { return this.mCheckNameValue; }
            set { this.mCheckNameValue = value; }
        }

        /// <summary>
        /// Gets or sets the maximum length of the comments.
        /// </summary>
        /// <value>The maximum length of the comments.</value>
        [Description("The maximum length (in characters) the comment can be. Enter 0 for unlimited length."), Category("Behaviour")]
        public int CommentsMaxLength
        {
            get { return this.mCommentsMaxLengthValue; }
            set { this.mCommentsMaxLengthValue = value; }
        }

        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        public string CommentText
        {
            get { return this.Comment.Text; }
            set { this.Comment.Text = value; }
        }

        /// <summary>
        /// Gets or sets the email text.
        /// </summary>
        /// <value>The email text.</value>
        public string EmailText
        {
            get { return this.Email.Text; }
            set { this.Email.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable comment].
        /// </summary>
        /// <value><c>true</c> if [enable comment]; otherwise, /c>.</value>
        public bool EnableComment
        {
            get { return this.Comment.Enabled; }
            set { this.Comment.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable email].
        /// </summary>
        /// <value><c>true</c> if [enable email]; otherwise, /c>.</value>
        public bool EnableEmail
        {
            get { return this.Email.Enabled; }
            set { this.Email.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable name].
        /// </summary>
        /// <value><c>true</c> if [enable name]; otherwise, /c>.</value>
        public bool EnableName
        {
            get { return this.Name.Enabled; }
            set { this.Name.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable notify].
        /// </summary>
        /// <value><c>true</c> if [enable notify]; otherwise, /c>.</value>
        public bool EnableNotify
        {
            get { return this.SubscribeToNotifications.Visible; }
            set { this.SubscribeToNotifications.Visible = value; }
        }

        /// <summary>
        /// Gets or sets the name text.
        /// </summary>
        /// <value>The name text.</value>
        public string NameText
        {
            get { return this.Name.Text; }
            set { this.Name.Text = value; }
        }

        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        /// <value>The parent unique identifier.</value>
        [Description("The id of the parent (page) the comment is being added to."), Category("Data")]
        public int ParentId
        {
            get { return Convert.ToInt32(this.LblParentID.Text); }
            set { this.LblParentID.Text = value.ToString(); }
        }

        /// <summary>
        /// Gets the shared resources.
        /// </summary>
        /// <value>The shared resources.</value>
        public string SharedResources
        {
            get
            {
                return this.TemplateSourceDirectory + "/" + "App_LocalResources/SharedResources.resx";
            }
        }

        /// <summary>
        /// Gets or sets the cancel button.
        /// </summary>
        /// <value>The cancel button.</value>
        protected System.Web.UI.WebControls.LinkButton CancelButton
        {
            get
            {
                return this.withEventsField_CancelButton;
            }

            set
            {
                if (this.withEventsField_CancelButton != null)
                {
                    this.withEventsField_CancelButton.Click -= this.CancelButton_Click;
                }

                this.withEventsField_CancelButton = value;

                if (this.withEventsField_CancelButton != null)
                {
                    this.withEventsField_CancelButton.Click += this.CancelButton_Click;
                }
            }
        }

        /// <summary>
        /// Gets or sets the submit button.
        /// </summary>
        /// <value>The submit button.</value>
        protected System.Web.UI.WebControls.LinkButton SubmitButton
        {
            get
            {
                return this.withEventsField_SubmitButton;
            }

            set
            {
                if (this.withEventsField_SubmitButton != null)
                {
                    this.withEventsField_SubmitButton.Click -= this.SubmitButton_Click;
                }

                this.withEventsField_SubmitButton = value;

                if (this.withEventsField_SubmitButton != null)
                {
                    this.withEventsField_SubmitButton.Click += this.SubmitButton_Click;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> value that corresponds to this
        /// Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag" /> enumeration
        /// values.</returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Table; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Renders the HTML opening tag of the control to the specified writer. This method is used
        /// primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the
        /// output stream to render HTML content on the client.</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (this.mCheckCommentsValue || this.mCheckEmailValue || this.mCheckNameValue || this.mCommentsMaxLengthValue > 0)
            {
                writer.WriteLine("<script language=\"JavaScript\">");
                writer.WriteLine("function wikiFormCheck(form)");
                writer.WriteLine("{");
                string clause = string.Empty;
                if (this.mCheckNameValue)
                {
                    writer.WriteLine("\tif ( form." + this.Name.ClientID + ".value == \"\" )");
                    writer.WriteLine("\t{");

                    writer.WriteLine("\t\talert(\"" + Localization.GetString("EnterAName.Text", this.SharedResources) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                    clause = "else ";
                }

                if (this.mCheckEmailValue)
                {
                    writer.WriteLine("\t" + clause + "if ( form." + this.Email.ClientID + ".value == \"\" )");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\talert(\"" + Localization.GetString("EnterAnEmailAddress.Text", this.SharedResources) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                    clause = "else ";
                }

                if (this.mCheckCommentsValue)
                {
                    writer.WriteLine("\t" + clause + "if ( form." + this.Comment.ClientID + ".value == \"\" )");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\talert(\"" + Localization.GetString("EnterSomeComments.Text", this.SharedResources) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                    clause = "else ";
                }

                if (this.mCommentsMaxLengthValue > 0)
                {
                    writer.WriteLine("\t" + clause + "if ( form." + this.Comment.ClientID + ".value.length > " + this.mCommentsMaxLengthValue + " )");
                    writer.WriteLine("\t{");
                    writer.WriteLine("\t\talert(\"" + string.Format(Localization.GetString("EnterAName.Text", this.SharedResources), this.mCommentsMaxLengthValue) + "\");");
                    writer.WriteLine("\t\treturn false;");
                    writer.WriteLine("\t}");
                }

                writer.WriteLine("\t");
                writer.WriteLine("\treturn true;");
                writer.WriteLine("}");
                writer.WriteLine("</script>");
            }
            ////MyBase.RenderBeginTag(writer)
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used
        /// primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the
        /// output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
            if (this.mCheckCommentsValue || this.mCheckEmailValue || this.mCheckNameValue)
            {
                this.SubmitButton.Attributes.Add(HtmlTextWriterAttribute.Onclick.ToString(), "return wikiFormCheck(this.form)");
            }

            if (!this.mSuccessValue)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalRed");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write(Localization.GetString("Failed.Text", this.SharedResources));
                writer.RenderEndTag();
                writer.RenderEndTag();
                writer.RenderEndTag();
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(Localization.GetString("Name", this.SharedResources));
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.Name.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            ////writer.RenderEndTag()

            ////writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(Localization.GetString("Email", this.SharedResources));
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.Email.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "2");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "NormalBold");

            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(Localization.GetString("Comments", this.SharedResources));
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.Comment.RenderControl(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Br);
            this.SubscribeToNotifications.RenderControl(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write(" | ");
            this.SubmitButton.RenderControl(writer);
            writer.Write(" | ");
            this.CancelButton.RenderControl(writer);
            writer.Write(" | ");
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        /// <summary>
        /// Handles the Initialize event of the AddCommentsForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void AddCommentsForm_Init(object sender, System.EventArgs e)
        {
            this.Name = new System.Web.UI.WebControls.TextBox();
            this.Name.ID = "Name";
            this.Name.EnableViewState = true;
            this.Name.CssClass = "NormalTextBox";
            this.Email = new System.Web.UI.WebControls.TextBox();
            this.Email.ID = "Email";
            this.Email.EnableViewState = true;
            this.Email.CssClass = "NormalTextBox";
            this.Comment = new System.Web.UI.WebControls.TextBox();
            this.Comment.ID = "Comment";
            this.Comment.EnableViewState = true;
            this.Comment.CssClass = "NormalTextBox";
            this.Comment.TextMode = TextBoxMode.MultiLine;
            this.Comment.Width = new System.Web.UI.WebControls.Unit(350);
            this.Comment.Height = new System.Web.UI.WebControls.Unit(50);
            this.Comment.MaxLength = this.CommentsMaxLength;
            this.SubmitButton = new System.Web.UI.WebControls.LinkButton();
            this.SubmitButton.CssClass = "CommandButton";
            this.SubmitButton.Text = Localization.GetString("PostComment", this.SharedResources);
            ////"Post Comment"
            this.CancelButton = new System.Web.UI.WebControls.LinkButton();
            this.CancelButton.CssClass = "CommandButton";
            this.CancelButton.Text = Localization.GetString("Cancel", this.SharedResources);
            ////"Cancel"
            this.LblParentID = new System.Web.UI.WebControls.Label();
            this.LblParentID.ID = "CurrParent";
            this.LblParentID.Visible = false;
            this.LblParentID.EnableViewState = true;

            this.SubscribeToNotifications = new System.Web.UI.WebControls.CheckBox();
            this.SubscribeToNotifications.ID = "EmailNotify";
            this.SubscribeToNotifications.Text = Localization.GetString("CommentsNotification", this.SharedResources);

            ////If EnableNotify = False Then SubscribeToNotifications.Visible = False Else SubscribeToNotifications.Visible = True

            this.Controls.Add(this.Name);
            this.Controls.Add(this.Email);
            this.Controls.Add(this.Comment);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.LblParentID);
            this.Controls.Add(this.SubscribeToNotifications);
        }

        /// <summary>
        /// Handles the Click event of the CancelButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            this.Name.Text = string.Empty;
            this.Email.Text = string.Empty;
            this.Comment.Text = string.Empty;
            if (this.PostCanceled != null)
            {
                this.PostCanceled(this);
            }
        }

        /// <summary>
        /// Handles the Click event of the SubmitButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event
        /// data.</param>
        private void SubmitButton_Click(object sender, System.EventArgs e)
        {
            using (UnitOfWork uOw = new UnitOfWork())
            {
                var commentBo = new CommentBO(uOw);

                string CommentText = this.Comment.Text;
                DotNetNuke.Security.PortalSecurity objSec = new DotNetNuke.Security.PortalSecurity();

                if (CommentText.Length > this.CommentsMaxLength)
                {
                    CommentText = CommentText.Substring(0, this.CommentsMaxLength);
                }
                ////4.8.3 has better control for NoMarkup
                var comment = new Comment
                {
                    ParentId = this.ParentId,
                    Name = objSec.InputFilter(this.Name.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup),
                    Email = objSec.InputFilter(this.Email.Text, DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup),
                    CommentText = objSec.InputFilter(CommentText, PortalSecurity.FilterFlag.NoMarkup),
                    Ip = objSec.InputFilter(this.Context.Request.ServerVariables["REMOTE_ADDR"], DotNetNuke.Security.PortalSecurity.FilterFlag.NoMarkup),
                    EmailNotify = this.SubscribeToNotifications.Checked,
                    Datetime = DateTime.UtcNow
                };
                comment = commentBo.Add(comment);

                ////send the notification
                var topic = new TopicBO(uOw).Get(this.ParentId);
                DNNUtils.SendNotifications(uOw, topic, comment.Name, comment.Email, comment.CommentText, comment.Ip);
                this.mSuccessValue = comment.CommentId > 0;

                if (this.mSuccessValue)
                {
                    this.Name.Text = string.Empty;
                    this.Email.Text = string.Empty;
                    this.Comment.Text = string.Empty;
                    this.Context.Cache.Remove("WikiComments" + this.ParentId.ToString());
                    if (this.PostSubmitted != null)
                    {
                        this.PostSubmitted(this);
                    }
                }
            }
        }

        #endregion Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCommentsForm"/> class.
        /// </summary>
        public AddCommentsForm()
        {
            this.Init += this.AddCommentsForm_Init;
        }
    }
}