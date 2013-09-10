﻿using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Modules.Wiki.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DotNetNuke.Modules.Wiki.BusinessObjects
{
    public class TopicBO : _AbstractBusinessObject<Topic, int>
    {
        private UnitOfWork _uof;

        public TopicBO(UnitOfWork uof)
            : base(uof.Context)
        {
            this._uof = uof;
        }

        #region Enums

        /// <summary>
        /// The possible controlled errors generated by this class
        /// </summary>
        public enum TopicError
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

        /// <summary>
        /// Gets all topics associated to the module id
        /// </summary>
        /// <param name="moduleId">the module id the topics are associated to</param>
        /// <returns>returns collection of Topics</returns>
        internal IEnumerable<Topic> GetAllByModuleID(int moduleId)
        {
            return this.db.ExecuteQuery<Topic>(CommandType.StoredProcedure, "Wiki_TopicGetAllByModuleID", moduleId);
        }
    }
}