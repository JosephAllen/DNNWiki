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
using DotNetNuke.Wiki.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_TopicHistory")]
    //setup the primary key for table
    [PrimaryKey("TopicHistoryId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_TopicHistory", CacheItemPriority.Default, 20)]
    public class TopicHistory : WikiMarkup
    {
        private string _content;
        private string _name;
        private string _updatedBy;
        private string _title;
        private string _description;
        private string _keywords;

        ///<summary>
        /// The ID of the Topic History
        ///</summary>
        public int TopicHistoryId { get; set; }

        /// <summary>
        /// The topic id to where the topic history is associated to
        /// </summary>
        public int TopicId { get; set; }

        ///<summary>
        ///
        ///</summary>
        public override string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                if (CanUseWikiText)
                {
                    Cache = RenderedContent;
                }
            }
        }

        ///<summary>
        ///
        ///</summary>
        public string Cache { get; set; }

        ///<summary>
        ///The topic name
        ///</summary>
        [StringLength(255)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value.TruncateString(255);
            }
        }

        /// <summary>
        /// The date the topic was last updated
        /// </summary>
        public DateTime UpdateDate { get; set; }

        ///<summary>
        ///
        ///</summary>
        [Required]
        [StringLength(101)]
        public string UpdatedBy
        {
            get
            {
                return _updatedBy;
            }
            set
            {
                _updatedBy = value.TruncateString(101);
            }
        }

        ///<summary>
        /// An integer for the user id of the user who last updated the object
        ///</summary>
        public int UpdatedByUserID { get; set; }

        /// <summary>
        /// The topic title
        /// </summary>
        [StringLength(256)]
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value.TruncateString(256);
            }
        }

        /// <summary>
        /// The topic description
        /// </summary>
        [StringLength(500)]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value.TruncateString(500);
            }
        }

        /// <summary>
        /// The topic keywords
        /// </summary>
        [StringLength(500)]
        public string Keywords
        {
            get
            {
                return _keywords;
            }
            set
            {
                _keywords = value.TruncateString(500);
            }
        }

        /// <summary>
        /// The user name of the user that made the last update
        /// </summary>
        [IgnoreColumn]
        public string UpdatedByUsername { get; set; }
    }
}