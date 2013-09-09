﻿using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_Topic")]
    //setup the primary key for table
    [PrimaryKey("TopicID", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_Topics", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    public class Topic
    {
        ///<summary>
        /// The ID of the topic
        ///</summary>
        public int TopicID { get; set; }

        ///<summary>
        /// The ModuleId of where the topic where created and gets displayed
        ///</summary>
        public int ModuleId { get; set; }

        ///<summary>
        /// A string with the topic contents
        ///</summary>
        public string Content { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Cache { get; set; }

        /// <summary>
        /// The topic name
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        ///<summary>
        /// The date the topic was last updated
        ///</summary>
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
        /// A boolean value that indicates if discussions is allowed in the topic
        /// </summary>
        public bool AllowDiscussions { get; set; }

        /// <summary>
        /// A boolean value that indicates if ratings is allowed in the topic
        /// </summary>
        public bool AllowRatings { get; set; }

        /// <summary>
        /// The number of times rating one was set
        /// </summary>
        public int RatingOneCount { get; set; }

        /// <summary>
        /// The number of times rating two was set
        /// </summary>
        public int RatingTwoCount { get; set; }

        /// <summary>
        /// The number of times rating three was set
        /// </summary>
        public int RatingThreeCount { get; set; }

        /// <summary>
        /// The number of times rating four was set
        /// </summary>
        public int RatingFourCount { get; set; }

        /// <summary>
        /// The number of times rating five was set
        /// </summary>
        public int RatingFiveCount { get; set; }

        /// <summary>
        /// The number of times rating six was set
        /// </summary>
        public int RatingSixCount { get; set; }

        /// <summary>
        /// The number of times rating seven was set
        /// </summary>
        public int RatingSevenCount { get; set; }

        /// <summary>
        /// The number of times rating eight was set
        /// </summary>
        public int RatingEightCount { get; set; }

        /// <summary>
        /// The number of times rating nine was set
        /// </summary>
        public int RatingNineCount { get; set; }

        /// <summary>
        /// The number of times rating ten was set
        /// </summary>
        public int RatingTenCount { get; set; }

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