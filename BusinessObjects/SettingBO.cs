﻿using DotNetNuke.Modules.Wiki.BusinessObjects.Models;
using DotNetNuke.Modules.Wiki.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace DotNetNuke.Modules.Wiki.BusinessObjects
{
    public class SettingBO : _AbstractBusinessObject<Setting, int>
    {
        #region Variables

        private UnitOfWork _uof;

        #endregion Variables

        #region Ctor

        public SettingBO(UnitOfWork uof)
            : base(uof.Context)
        {
            this._uof = uof;
        }

        #endregion Ctor

        #region Enums

        /// <summary>
        /// The possible controlled errors generated by this class
        /// </summary>
        public enum SettingError
        {
            Error1 = 1,
            Error2 = 2,
            Error3 = 3
        }

        #endregion Enums

        #region Methods

        /// <summary>
        /// Gets a setting entity based on the module id passed
        /// </summary>
        /// <param name="moduleId">the module id the setting is associated to</param>
        /// <returns>returns a setting</returns>
        internal Setting GetByModuleID(int moduleId)
        {
            return this.db.ExecuteScalar<Setting>(CommandType.StoredProcedure, "Wiki_SettingsGetByModuleID", moduleId);
        }

        internal override void Entity_EvaluateSqlException(
                    SqlException exc,
                    SharedEnum.CrudOperation crudOperation)
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods
    }
}