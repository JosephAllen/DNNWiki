using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Wiki.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_Settings")]
    //setup the primary key for table
    [PrimaryKey("SettingId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_Settings", CacheItemPriority.Default, 20)]
    public class Setting
    {
        private string _commentNotifyRoles;
        private string _contentEditorRoles;

        ///<summary>
        /// The ID of the Setting
        ///</summary>
        public int SettingId { get; set; }

        ///<summary>
        /// The ModuleId of where the setting where created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }

        ///<summary>
        ///
        ///</summary>
        [Required]
        [StringLength(255)]
        public string ContentEditorRoles
        {
            get
            {
                return _contentEditorRoles;
            }
            set
            {
                _contentEditorRoles = value.TruncateString(255);
            }
        }

        ///<summary>
        /// A boolean value that indicates if discussions is allowed in the setting
        ///</summary>
        public bool AllowDiscussions { get; set; }

        ///<summary>
        /// A boolean value that indicates if rattings is allowed in the setting
        ///</summary>
        public bool AllowRatings { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Nullable<bool> DefaultDiscussionMode { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Nullable<bool> DefaultRatingMode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [StringLength(255)]
        public string CommentNotifyRoles
        {
            get
            {
                return _commentNotifyRoles;
            }
            set
            {
                _commentNotifyRoles = value.TruncateString(255);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Nullable<bool> CommentNotifyUsers { get; set; }
    }
}