using DotNetNuke.Entities.Modules;
using System;
using System.ComponentModel;

namespace DotNetNuke.Wiki.Utilities
{
    /// <summary>
    /// Central class for managing DnnWiki module settings
    /// </summary>
    public class WikiModuleSettings : IDisposable
    {
        /// <summary>
        /// The setting for allow discussions
        /// </summary>
        public const string SAllowDiscussions = "AllowDiscussions";

        /// <summary>
        /// The setting for allow ratings
        /// </summary>
        public const string SAllowRatings = "AllowRatings";

        /// <summary>
        /// The setting for comment notify roles
        /// </summary>
        public const string SCommentNotifyRoles = "CommentNotifyRoles";

        /// <summary>
        /// The setting for comment notify users
        /// </summary>
        public const string SCommentNotifyUsers = "CommentNotifyUsers";

        /// <summary>
        /// The setting for content editor roles
        /// </summary>
        public const string SContentEditorRoles = "ContentEditorRoles";

        /// <summary>
        /// The setting for default discussion mode
        /// </summary>
        public const string SDefaultDiscussionMode = "DefaultDiscussionMode";

        /// <summary>
        /// The setting for default rating mode
        /// </summary>
        public const string SDefaultRatingMode = "DefaultRatingMode";

        /// <summary>
        /// The setting for use wiki settings
        /// </summary>
        public const string SUseWikiSettings = "UseWikiSettings";

        /// <summary>
        /// The string use DNN settings, indicates that DNN settings should be used instead
        /// </summary>
        public const string StrUseDNNSettings = "UseDNNSettings";

        /// <summary>
        /// The module identifier
        /// </summary>
        private int mModuleId;

        /// <summary>
        /// The module settings
        /// </summary>
        private System.Collections.Hashtable mSettings = null;

        /// <summary>
        /// Indicates whether it was disposed or not
        /// </summary>
        private bool mDisposed = false;

        /// <summary>
        /// Gets or sets a value indicating whether allows discussions.
        /// </summary>
        /// <value><c>true</c> if allows discussions; otherwise, <c>false</c>.</value>
        public bool AllowDiscussions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether allows ratings.
        /// </summary>
        /// <value><c>true</c> if allows ratings; otherwise, <c>false</c>.</value>
        public bool AllowRatings { get; set; }

        /// <summary>
        /// Gets or sets the comment notify roles.
        /// </summary>
        /// <value>The comment notify roles.</value>
        public string CommentNotifyRoles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether comment notify users.
        /// </summary>
        /// <value><c>true</c> if comment notify users; otherwise, <c>false</c>.</value>
        public bool CommentNotifyUsers { get; set; }

        /// <summary>
        /// Gets or sets the content editor roles.
        /// </summary>
        /// <value>The content editor roles.</value>
        public string ContentEditorRoles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [default discussion mode].
        /// </summary>
        /// <value><c>true</c> if [default discussion mode]; otherwise, <c>false</c>.</value>
        public bool DefaultDiscussionMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [default rating mode].
        /// </summary>
        /// <value><c>true</c> if [default rating mode]; otherwise, <c>false</c>.</value>
        public bool DefaultRatingMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use Wiki settings].
        /// </summary>
        /// <value><c>true</c> if [use Wiki settings]; otherwise, <c>false</c>.</value>
        public bool UseWikiSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WikiModuleSettings" /> class.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        public WikiModuleSettings(int moduleId)
        {
            this.mModuleId = moduleId;
            ModuleController moduleController = new ModuleController();
            mSettings = moduleController.GetModuleSettings(moduleId);

            AllowDiscussions = GetValueFromSetting<bool>(SAllowDiscussions);
            AllowRatings = GetValueFromSetting<bool>(SAllowRatings);
            CommentNotifyRoles = GetValueFromSetting<string>(SCommentNotifyRoles) ?? string.Empty;
            CommentNotifyUsers = GetValueFromSetting<bool>(SCommentNotifyUsers);
            ContentEditorRoles = GetValueFromSetting<string>(SContentEditorRoles) ?? string.Empty;
            DefaultDiscussionMode = GetValueFromSetting<bool>(SDefaultDiscussionMode);
            DefaultRatingMode = GetValueFromSetting<bool>(SDefaultRatingMode);
            UseWikiSettings = GetValueFromSetting<bool>(SUseWikiSettings);
        }

        /// <summary>
        /// Gets the value from setting.
        /// </summary>
        /// <typeparam name="T">the type of the value to return</typeparam>
        /// <param name="settingName">Name of the setting.</param>
        /// <returns>returns the setting value</returns>
        private T GetValueFromSetting<T>(string settingName)
        {
            if (mSettings != null && mSettings[settingName] != null)
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    return (T)converter.ConvertFromString(mSettings[settingName].ToString());
                }

                return (T)mSettings[settingName];
            }

            return default(T);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            ModuleController moduleController = new ModuleController();
            moduleController.UpdateModuleSetting(this.mModuleId, SAllowDiscussions, this.AllowDiscussions.ToString());
            moduleController.UpdateModuleSetting(this.mModuleId, SAllowRatings, this.AllowRatings.ToString());
            moduleController.UpdateModuleSetting(this.mModuleId, SCommentNotifyRoles, this.CommentNotifyRoles);
            moduleController.UpdateModuleSetting(this.mModuleId, SCommentNotifyUsers, this.CommentNotifyUsers.ToString());
            moduleController.UpdateModuleSetting(this.mModuleId, SContentEditorRoles, this.ContentEditorRoles);
            moduleController.UpdateModuleSetting(this.mModuleId, SDefaultDiscussionMode, this.DefaultDiscussionMode.ToString());
            moduleController.UpdateModuleSetting(this.mModuleId, SDefaultRatingMode, this.DefaultRatingMode.ToString());
            moduleController.UpdateModuleSetting(this.mModuleId, SUseWikiSettings, this.UseWikiSettings.ToString());
        }

        /// <summary>
        /// Indicates whether Settings the are set or not
        /// </summary>
        /// <returns>returns true if set, false otherwise</returns>
        public bool SettingsAreSet()
        {
            return mSettings.ContainsKey(SUseWikiSettings);
        }

        /// <summary>
        /// Uses the DNN settings.
        /// </summary>
        /// <returns>returns true if uses DNN Settings</returns>
        public bool UsesDnnSettings()
        {
            return string.IsNullOrWhiteSpace(this.ContentEditorRoles) ? true : this.ContentEditorRoles.Equals(StrUseDNNSettings);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // This object will be cleaned up by the Dispose method. Therefore, you should call
            // GC.SupressFinalize to take this object off the finalization queue and prevent
            // finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.mDisposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Clean up all managed resources
                    if (this.mSettings != null)
                    {
                        this.mSettings = null;
                    }
                }

                // Clean up all native resources

                // Note disposing has been done.
                this.mDisposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnitOfWork" /> class.
        /// </summary>
        ~WikiModuleSettings()
        {
            this.Dispose(false);
        }

        #endregion IDisposable Members
    }
}