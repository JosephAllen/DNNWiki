using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Wiki.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_Comment")]
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