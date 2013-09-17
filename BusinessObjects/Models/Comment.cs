using DotNetNuke.ComponentModel.DataAnnotations;
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
        ///<summary>
        /// The ID of the Comment
        ///</summary>
        public int CommentId { get; set; }

        ///<summary>
        /// The name of the comment
        ///</summary>
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        ///<summary>
        ///
        ///</summary>
        [Required]
        [StringLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// The comment data
        /// </summary>

        [Required]
        [StringLength(1024)]
        [ColumnName("Comment")]
        public string CommentText { get; set; }

        /// <summary>
        /// The date when it was created
        /// </summary>
        public DateTime Datetime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Ip { get; set; }

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