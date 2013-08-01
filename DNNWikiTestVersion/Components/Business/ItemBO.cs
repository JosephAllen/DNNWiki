﻿using Christoc.Modules.DNNWikiTestVersion.Components.Business;
using Christoc.Modules.DNNWikiTestVersion.Components.Data;
using DotNetNuke.Data;
using System.Collections.Generic;

namespace Christoc.Modules.DNNWikiTestVersion.Components.Business
{
    internal class ItemBO : AbstractBusinessObject<Item, string>
    {
        #region Enums

        /// <summary>
        /// The possible controlled errors generated by this class
        /// </summary>
        public enum ItemError
        {
            Error1 = 1,
            Error2 = 2,
            Error3 = 3
        }

        #endregion Enums
    }
}