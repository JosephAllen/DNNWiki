using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_CommentParent")]
    //setup the primary key for table
    [PrimaryKey("CommentParentId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_CommentParents", CacheItemPriority.Default, 20)]
    public class CommentParent
    {
        ///<summary>
        /// The ID of the CommentParent
        ///</summary>
        public int CommentParentId { get; set; }

        ///<summary>
        /// The name of the comment parent
        ///</summary>
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        ///<summary>
        /// The id of the parent
        ///</summary>
        public int ParentId { get; set; }
    }
}