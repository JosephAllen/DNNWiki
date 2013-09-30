namespace DotNetNuke.Wiki.Views.SharedControls
{
    /// <summary>
    /// Presents user friendly messages to the user
    /// </summary>
    public partial class Messages : System.Web.UI.UserControl
    {
        internal const string ERRORCLASS = "dnnFormMessage dnnFormError";
        internal const string WARNINGCLASS = "dnnFormMessage dnnFormWarning";
        internal const string SUCCESSCLASS = "dnnFormMessage dnnFormSuccess";
        internal const string INFOCLASS = "dnnFormMessage dnnFormInfo";

        #region Aux Functions

        /// <summary>
        /// Clears message labels, for warning and errors
        /// </summary>
        public void ClearMessages()
        {
            pnl_message.CssClass = string.Empty;
            pnl_message.Visible = false;
            lt_message.Text = string.Empty;
        }

        /// <summary>
        /// Shows an error message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowError(string message)
        {
            pnl_message.CssClass = ERRORCLASS;
            pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            lt_message.Text = message;
        }

        /// <summary>
        /// Shows an warning message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowWarning(string message)
        {
            pnl_message.CssClass = WARNINGCLASS;
            pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            lt_message.Text = message;
        }

        /// <summary>
        /// Shows an success message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowSuccess(string message)
        {
            pnl_message.CssClass = SUCCESSCLASS;
            pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            lt_message.Text = message;
        }

        /// <summary>
        /// Shows an information message
        /// </summary>
        /// <param name="message">if empty hides the message control, else, shows the
        /// message</param>
        public void ShowInfo(string message)
        {
            pnl_message.CssClass = INFOCLASS;
            pnl_message.Visible = !string.IsNullOrWhiteSpace(message);
            lt_message.Text = message;
        }

        #endregion Aux Functions
    }
}