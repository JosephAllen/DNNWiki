﻿using DotNetNuke.Modules.DNNWikiUnitOfWork.Models;
using DotNetNuke.Data;
using System.Collections.Generic;

namespace DotNetNuke.Modules.DNNWikiUnitOfWork.UnitOfWork
{
    public class ItemBO : AbstractBusinessObject<Item, string>
    {
        private UnitOfWork _uof;

        public ItemBO(UnitOfWork uof)
            : base(uof.Context)
        {
            this._uof = uof;
        }

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

        public override void Entity_EvaluateSqlException(System.Data.SqlClient.SqlException exc, Components.SharedEnum.CrudOperation crudOperation)
        {
            throw new System.NotImplementedException();
        }
    }
}