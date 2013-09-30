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
    [TableName("Wiki_Comments")]
    //setup the primary key for table
    [PrimaryKey("CommentId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_Comments", CacheItemPriority.Default, 20)]
    public class Comment
    {
        private string _name;
        private string _emailAddress;
        private string _commentText;
        private string _ip;

        ///<summary>
        /// The ID of the Comment
        ///</summary>
        public int CommentId { get; set; }

        ///<summary>
        /// The name of the comment
        ///</summary>
        [Required]
        [StringLength(64)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value.TruncateString(64);
            }
        }

        ///<summary>
        ///
        ///</summary>
        [Required]
        [StringLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get
            {
                return _emailAddress;
            }
            set
            {
                _emailAddress = value.TruncateString(64);
            }
        }

        /// <summary>
        /// The comment data
        /// </summary>

        [Required]
        [StringLength(1024)]
        [ColumnName("Comment")]
        public string CommentText
        {
            get
            {
                return _commentText;
            }
            set
            {
                _commentText = value.TruncateString(1024);
            }
        }

        /// <summary>
        /// The date when it was created
        /// </summary>
        public DateTime Datetime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Ip
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value.TruncateString(50);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public bool EmailNotify { get; set; }
    }
}