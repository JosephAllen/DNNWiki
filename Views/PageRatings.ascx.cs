//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2013
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Wiki.Views
{
    partial class PageRatings : WikiControlBase
    {
        private DotNetNuke.Modules.Wiki.WikiControlBase mModule;
        private DotNetNuke.Modules.Wiki.Entities.TopicInfo mTopic;
        public DotNetNuke.Modules.Wiki.Entities.TopicInfo Topic
        {
            get
            {
                if (mTopic == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!uplevel is DotNetNuke.Modules.Wiki.WikiControlBase)
                    {
                        uplevel = uplevel.Parent;
                    }
                    mTopic = ((DotNetNuke.Modules.Wiki.WikiControlBase)uplevel).Topic;
                }
                return mTopic;
            }
        }

        public DotNetNuke.Modules.Wiki.WikiControlBase ParentModule
        {
            get
            {
                if (mModule == null)
                {
                    System.Web.UI.Control uplevel = default(System.Web.UI.Control);
                    uplevel = this.Parent;
                    while (!uplevel is DotNetNuke.Modules.Wiki.WikiControlBase)
                    {
                        uplevel = uplevel.Parent;
                    }
                    mModule = (DotNetNuke.Modules.Wiki.WikiControlBase)uplevel;
                    mModule.ModuleConfiguration = this.ModuleConfiguration;
                }
                return mModule;
            }
        }

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

        #endregion

        protected void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();
        }

        private void Page_PreRender(object sender, System.EventArgs e)
        {
            if (Topic.FivePointRatingsRecorded == 0)
            {
                RatingBar.Visible = false;
                NoRating.Visible = true;
            }
            else
            {
                RatingBar.Visible = true;
                NoRating.Visible = false;
                RatingBar.Src = this.TemplateSourceDirectory + "/RatingBar.aspx?rating=" + Topic.FivePointAverage.ToString("#.#");
                RatingBar.Alt = Topic.FivePointAverage.ToString("#.#");
            }
        }

        private void LoadLocalization()
        {
            NoRating.Text = Localization.GetString("PageRatingsNotRatedYet", RouterResourceFile);
            RatingLbl.Text = Localization.GetString("PageRatingsTitle", RouterResourceFile);
        }
        public PageRatings()
        {
            PreRender += Page_PreRender;
            Load += Page_Load;
            Init += Page_Init;
        }
    }
}