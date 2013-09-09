﻿using DotNetNuke.Data;
using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Modules.Wiki.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DotNetNuke.Modules.Wiki.BusinessObjects
{
    public class CommentBO : _AbstractBusinessObject<Comment, int>
    {
        private UnitOfWork _uof;

        public CommentBO(UnitOfWork uof)
            : base(uof.Context)
        {
            this._uof = uof;
        }

        #region Enums

        /// <summary>
        /// The possible controlled errors generated by this class
        /// </summary>
        public enum CommentError
        {
            Error1 = 1,
            Error2 = 2,
            Error3 = 3
        }

        #endregion Enums

        public override void Entity_EvaluateSqlException(
            SqlException exc,
            SharedEnum.CrudOperation crudOperation)
        {
            throw new System.NotImplementedException();
        }
    }
}