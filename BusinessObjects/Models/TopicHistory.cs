using DotNetNuke.ComponentModel.DataAnnotations;
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
        public string UpdatedBy { get; set; }

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

        /// <summary>
        /// The user name of the user that made the last update
        /// </summary>
        [IgnoreColumn]
        public string UpdatedByUsername { get; set; }
    }
}