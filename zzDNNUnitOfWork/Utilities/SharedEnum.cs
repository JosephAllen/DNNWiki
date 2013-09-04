using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.DNNUnitOfWork.Utilities
{
    public class SharedEnum
    {
        /// <summary>
        /// Sql crud operations enum
        /// </summary>
        public enum CrudOperation
        {
            Insert, Update, Delete
        }
    }
}