﻿#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="SettingBO.cs" company="DNN Corp®">
//      DNN Corp® - http://www.dnnsoftware.com Copyright (c) 2002-2013 by DNN Corp®
//
//      Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
//      associated documentation files (the "Software"), to deal in the Software without restriction,
//      including without limitation the rights to use, copy, modify, merge, publish, distribute,
//      sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
//      furnished to do so, subject to the following conditions:
//
//      The above copyright notice and this permission notice shall be included in all copies or
//      substantial portions of the Software.
//
//      THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
//      NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//      NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//      DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//      OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
////------------------------------------------------------------------------------------------------------

#endregion Copyright

using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DotNetNuke.Wiki.BusinessObjects
{
    /// <summary>
    /// The Settings Business Object
    /// </summary>
    public class SettingBO : _AbstractBusinessObject<Setting, int>
    {
        #region Variables

        private UnitOfWork currentUnitOfWork;

        #endregion Variables

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingBO"/> class.
        /// </summary>
        /// <param name="uOw">The Unit of Work.</param>
        public SettingBO(UnitOfWork uOw)
            : base(uOw.Context)
        {
            this.currentUnitOfWork = uOw;
        }

        #endregion Ctor

        #region Enums

        /// <summary>
        /// The possible controlled errors generated by this class
        /// </summary>
        public enum SettingError
        {
            /// <summary>
            /// The error1
            /// </summary>
            Error1 = 1,

            /// <summary>
            /// The error2
            /// </summary>
            Error2 = 2,

            /// <summary>
            /// The error3
            /// </summary>
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
            return this.DatabaseContext.ExecuteQuery<Setting>(CommandType.StoredProcedure, "Wiki_SettingsGetByModuleID", moduleId).FirstOrDefault();
        }

        /// <summary>
        /// Entity_s the evaluate SQL exception.
        /// </summary>
        /// <param name="exc">The exception.</param>
        /// <param name="crudOperation">The crud operation.</param>
        /// <exception cref="System.NotImplementedException">CRUD Operation Exception</exception>
        internal override void Entity_EvaluateSqlException(
                    SqlException exc,
                    SharedEnum.CrudOperation crudOperation)
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods
    }
}