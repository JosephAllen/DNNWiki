using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Wiki.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_CommentParent")]
    //setup the primary key for table
    [PrimaryKey("CommentParentId", AutoIncrement = true)]
    //configure caching using PetaPoco
    [Cacheable("Wiki_CommentParents", CacheItemPriority.Default, 20)]
    public class CommentParent
    {
        private string _name;

        ///<summary>
        /// The ID of the CommentParent
        ///</summary>
        public int CommentParentId { get; set; }

        ///<summary>
        /// The name of the comment parent
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
                _name = value.TruncateString(50);
            }
        }

        ///<summary>
        /// The id of the parent
        ///</summary>
        public int ParentId { get; set; }
    }
}