#region Copyright

//
// DotNetNuke� - http://www.dotnetnuke.com Copyright (c) 2002-2013 by DotNetNuke Corporation
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

#endregion Copyright

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