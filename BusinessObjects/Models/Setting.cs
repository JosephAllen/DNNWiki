﻿using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_Settings")]
    //setup the primary key for table
    [PrimaryKey("SettingId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_Settings", CacheItemPriority.Default, 20)]
    public class Setting
    {
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
        public string ContentEditorRoles { get; set; }

        ///<summary>
        /// A boolean value that indicates if discussions is allowed in the setting
        ///</summary>
        public bool AllowDiscussions { get; set; }

        ///<summary>
        /// A boolean value that indicates if rattings is allowed in the setting
        ///</summary>
        public bool AllowRattings { get; set; }

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
        public string CommentNotifyRoles { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Nullable<bool> CommentNotifyUsers { get; set; }
    }
}