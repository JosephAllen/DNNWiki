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

    partial class RecentChanges : WikiControlBase
    {

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
            if (!this.IsPostBack())
            {
                HitTable.Text = CreateRecentChangeTable(1);
            }
        }

        private void LoadLocalization()
        {
            TitleLbl.Text = Localization.GetString("RCTitle", RouterResourceFile);
            cmdLast24Hrs.Text = Localization.GetString("RCLast24h", RouterResourceFile);
            cmdLast7Days.Text = Localization.GetString("RCLast7d", RouterResourceFile);
            cmdLastMonth.Text = Localization.GetString("RCLastMonth", RouterResourceFile);

        }
        private void cmdLast24Hrs_Click(System.Object sender, System.EventArgs e)
        {
            HitTable.Text = CreateRecentChangeTable(1);
        }

        private void cmdLast7Days_Click(System.Object sender, System.EventArgs e)
        {
            HitTable.Text = CreateRecentChangeTable(7);
        }

        private void cmdLastMonth_Click(System.Object sender, System.EventArgs e)
        {
            HitTable.Text = CreateRecentChangeTable(31);
        }
        public RecentChanges()
        {
            Load += Page_Load;
            Init += Page_Init;
        }
    }
}