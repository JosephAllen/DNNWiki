//
// DotNetNuke® - http://www.dotnetnuke.com Copyright (c) 2002-2013 by DotNetNuke Corporation
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
//

using DotNetNuke.Services.Localization;
using DotNetNuke.Wiki.Utilities;
using System.Web;

namespace DotNetNuke.Wiki.Views
{
    partial class Start : WikiModuleBase
    {
        protected System.Web.UI.WebControls.Button cmdHistory;
        protected PageRatings pageRating;
        protected Ratings ratings;

        protected UI.UserControls.SectionHeadControl WikiTextDirections;

        #region " Web Form Designer Generated Code "

        //This call is required by the Web Form Designer.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
        }

        private void Page_Init(System.Object sender, System.EventArgs e)
        {
            //CODEGEN: This method call is required by the Web Form Designer
            //Do not modify it using the code editor.
            InitializeComponent();
        }

        #endregion " Web Form Designer Generated Code "

        #region "Form Events"

        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            if (UserId == -1)
            {
                UserName = "Anonymous";
            }
            else
            {
                UserName = this.UserInfo.Username;
            }

            this.AddCommentsForm1.EnableNotify = WikiSettings.CommentNotifyUsers == true;

            if (Request.IsAuthenticated)
            {
                if (this.UserInfo.IsSuperUser | this.UserInfo.IsInRole(PortalSettings.AdministratorRoleName))
                {
                    IsAdmin = true;
                }
            }

            LoadLocalization();
            this.AddCommentPane.Visible = false;
            this.Comments2.Visible = true;
            this.AddCommentCommand.Visible = true;
            //Me.DeleteCommentCommand.Visible = True

            this.lblPageTopic.Text = this.PageTopic.Replace(WikiHomeName, Localization.GetString("Home", this.RouterResourceFile));

            if (_Topic.Title.Trim().Length > 0)
            {
                this.lblPageTopic.Text = _Topic.Title;
            }

            this.AddCommentsForm1.ParentId = _Topic.TopicID;
            CommentCount1.ParentId = _Topic.TopicID;

            Comments2.IsAdmin = this.IsAdmin;
            Comments2.ParentId = _Topic.TopicID;

            //CommentsSec.IsExpanded = False
            DisplayTopic();
        }

        #endregion "Form Events"

        private void LoadLocalization()
        {
            AddCommentCommand.Text = Localization.GetString("StartAddComment", this.RouterResourceFile);

            CommentCount1.Text = Localization.GetString("StartCommentCount", this.RouterResourceFile);
            CommentsSec.Text = Localization.GetString("StartCommentsSection", this.RouterResourceFile);

            PostCommentLbl.Text = Localization.GetString("StartPostComment", this.RouterResourceFile);

            RatingSec.Text = Localization.GetString("StartRatingSec.Text", this.RouterResourceFile);
        }

        private void DisplayTopic()
        {
            this.lblPageContent.Visible = true;
            string Content = ReadTopic();
            this.lblPageContent.Text = HttpUtility.HtmlDecode(Content).ToString();
            this.ratingTbl.Visible = _Topic.AllowRatings && WikiSettings.AllowRatings;
            this.RatingSec.Visible = _Topic.AllowRatings && WikiSettings.AllowRatings;
            this.pageRating.Visible = _Topic.AllowRatings && WikiSettings.AllowRatings;
            this.ratings.Visible = _Topic.AllowRatings;
            this.AddCommentCommand.Visible = _Topic.AllowDiscussions && WikiSettings.AllowDiscussions;
            this.CommentCount1.Visible = false;
            this.Comments2.Visible = _Topic.AllowDiscussions && WikiSettings.AllowDiscussions;
            this.CommentsSec.Visible = _Topic.AllowDiscussions && WikiSettings.AllowDiscussions;
            this.CommentsTbl.Visible = _Topic.AllowDiscussions && WikiSettings.AllowDiscussions;

            DotNetNuke.Framework.CDefault p = default(DotNetNuke.Framework.CDefault);
            p = (DotNetNuke.Framework.CDefault)this.Page;

            //set the page title, check for the Topic.Title, Topic.Name, then use PageTopic parameter if all else fails
            if (_Topic.Title.Trim().Length > 0)
            {
                p.Title = p.Title + " > " + _Topic.Title;
            }
            else if (_Topic.Name.Trim().Length > 0)
            {
                p.Title = p.Title + " > " + _Topic.Name;
            }
            else
            {
                p.Title = p.Title + " > " + this.PageTopic;
            }

            if ((_Topic.Description != null) & !(_Topic.Description == string.Empty))
            {
                p.Description = _Topic.Description + " " + p.Description;
            }

            if ((_Topic.Keywords != null) & !(_Topic.Keywords == string.Empty))
            {
                p.KeyWords = _Topic.Keywords + " " + p.KeyWords;
            }
        }

        private void EditPage()
        {
            //Me.chkPageInEditMode.Checked = True
            this.DisplayTopic();
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (ratings.HasVoted)
            {
                RatingSec.IsExpanded = false;
            }
            this.CommentCount1.Visible = false;
            //CommentsSec.IsExpanded = False
            if (Request.IsAuthenticated)
            {
                this.AddCommentsForm1.NameText = UserInfo.DisplayName;
                this.AddCommentsForm1.EnableName = false;
                this.AddCommentsForm1.EmailText = UserInfo.Email;
                this.AddCommentsForm1.EnableEmail = false;

                //Dim lstEmails As List(Of String) = New Entities.CommentsController().GetNotificationEmails(Me.Topic)
                //Me.AddCommentsForm1.CommentText = "Total: " & lstEmails.Count & vbCrLf
                //Me.AddCommentsForm1.CommentText = Me.AddCommentsForm1.CommentText & "-----------" & vbCrLf
                //For Each s As String In lstEmails
                //Me.AddCommentsForm1.CommentText = Me.AddCommentsForm1.CommentText & s & vbCrLf
                //Next
            }
        }

        private void AddCommentCommand_Click(System.Object sender, System.EventArgs e)
        {
            this.AddCommentPane.Visible = true;
            this.Comments2.Visible = false;
            this.AddCommentCommand.Visible = false;
            CommentsSec.IsExpanded = true;
        }

        private void AddCommentsForm1_PostCanceled(ref object s)
        {
            this.AddCommentPane.Visible = false;
            this.Comments2.Visible = true;
            this.AddCommentCommand.Visible = true;
        }

        private void AddCommentsForm1_PostSubmitted(ref object s)
        {
            this.AddCommentPane.Visible = false;
            this.Comments2.Visible = true;
            this.AddCommentCommand.Visible = true;
        }

        public Start()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
            Init += Page_Init;
        }
    }
}