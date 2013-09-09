using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Models
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
        //Test
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
        public string Comment { get; set; }

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