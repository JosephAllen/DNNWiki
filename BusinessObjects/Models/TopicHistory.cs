using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_TopicHistory")]
    //setup the primary key for table
    [PrimaryKey("TopicHistoryId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_TopicHistory", CacheItemPriority.Default, 20)]
    public class TopicHistory
    {
        ///<summary>
        /// The ID of the Topic History
        ///</summary>
        public int TopicHistoryId { get; set; }

        /// <summary>
        /// The topic id to where the topic history is acossiated to
        /// </summary>
        public int TopicId { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string Content { get; set; }

        ///<summary>
        ///
        ///</summary>
        public string Cache { get; set; }

        ///<summary>
        ///
        ///</summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// The date the topic was last updated
        /// </summary>
        public DateTime UpdateDate { get; set; }

        ///<summary>
        ///
        ///</summary>
        [Required]
        [StringLength(101)]
        public int UpdatedBy { get; set; }

        ///<summary>
        /// An integer for the user id of the user who last updated the object
        ///</summary>
        public int UpdatedByUserID { get; set; }

        /// <summary>
        /// The topic title
        /// </summary>
        [StringLength(256)]
        public string Title { get; set; }

        /// <summary>
        /// The topic description
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// The topic keywords
        /// </summary>
        [StringLength(500)]
        public string Keywords { get; set; }
    }
}