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


// description: Allows users to search for topics. 
//              Needs to be rewriten but for now this is OK.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Wiki.Views
{

    partial class SearchPage : WikiControlBase
    {

        protected System.Web.UI.WebControls.Label lblPageContent;


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

        public new void Page_Load(System.Object sender, System.EventArgs e)
        {
            LoadLocalization();
        }

        private void cmdSearch_Click(System.Object sender, System.EventArgs e)
        {
            this.SearchTopics();
        }

        private void LoadLocalization()
        {
            Label2.Text = Localization.GetString("SearchTitleBasic", RouterResourceFile);
            Label2.Text = Localization.GetString("SearchFieldLabel", RouterResourceFile);
            cmdSearch.Text = Localization.GetString("SearchExec", RouterResourceFile);
        }


        private void SearchTopics()
        {
            HitTable.Text = CreateTable(Search("%" + this.txtTextToSearch.Text + "%"));

        }
        public SearchPage()
        {
            Load += Page_Load;
            Init += Page_Init;
        }

    }
}