﻿using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Wiki.Extensions;
using DotNetNuke.Wiki.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_Topic")]
    //setup the primary key for table
    [PrimaryKey("TopicID", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_Topics", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    public class Topic : WikiMarkup
    {
        private string _content;
        private string _name;
        private string _updateUserBy;
        private string _description;
        private string _keywords;
        private string _title;

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

        /// <summary>
        ///
        /// </summary>
        public string Cache { get; set; }

        /// <summary>
        /// The topic name
        /// </summary>
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

        ///<summary>
        /// The date the topic was last updated
        ///</summary>
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
                return _updateUserBy;
            }
            set
            {
                _updateUserBy = value.TruncateString(101);
            }
        }

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

        [IgnoreColumn]
        public int TenPointRatingsRecorded
        {
            get { return RatingOneCount + RatingTwoCount + RatingThreeCount + RatingFourCount + RatingFiveCount + RatingSixCount + RatingSevenCount + RatingEightCount + RatingNineCount + RatingTenCount; }
        }

        [IgnoreColumn]
        public int FivePointRatingsRecorded
        {
            get { return RatingOneCount + RatingTwoCount + RatingThreeCount + RatingFourCount + RatingFiveCount; }
        }

        [IgnoreColumn]
        public double FivePointAverage
        {
            get { return (Convert.ToDouble(RatingOneCount) + Convert.ToDouble(RatingTwoCount * 2) + Convert.ToDouble(RatingThreeCount * 3) + Convert.ToDouble(RatingFourCount * 4) + Convert.ToDouble(RatingFiveCount * 5)) / Convert.ToDouble(FivePointRatingsRecorded); }
        }

        [IgnoreColumn]
        public double TenPointAverage
        {
            get { return (Convert.ToDouble(RatingOneCount) + Convert.ToDouble(RatingTwoCount * 2) + Convert.ToDouble(RatingThreeCount * 3) + Convert.ToDouble(RatingFourCount * 4) + Convert.ToDouble(RatingFiveCount * 5) + Convert.ToDouble(RatingSixCount * 6) + Convert.ToDouble(RatingSevenCount * 7) + Convert.ToDouble(RatingEightCount * 8) + Convert.ToDouble(RatingNineCount * 9) + Convert.ToDouble(RatingTenCount * 10)) / Convert.ToDouble(TenPointRatingsRecorded); }
        }
    }
}