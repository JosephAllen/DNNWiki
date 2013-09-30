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
using System.ComponentModel.DataAnnotations;
using System.Web.Caching;

namespace DotNetNuke.Wiki.BusinessObjects.Models
{
    [TableName("Wiki_CommentParents")]
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