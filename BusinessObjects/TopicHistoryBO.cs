﻿#region Copyright

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

using DotNetNuke.Wiki.BusinessObjects.Models;
using DotNetNuke.Wiki.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DotNetNuke.Wiki.BusinessObjects
{
    public class TopicHistoryBO : _AbstractBusinessObject<TopicHistory, int>
    {
        #region Variables

        private UnitOfWork _uof;

        #endregion Variables

        #region Ctor

        public TopicHistoryBO(UnitOfWork uof)
            : base(uof.Context)
        {
            this._uof = uof;
        }

        #endregion Ctor

        #region Enums

        /// <summary>
        /// The possible controlled errors generated by this class
        /// </summary>
        public enum TopicHistoryError
        {
            Error1 = 1,
            Error2 = 2,
            Error3 = 3
        }

        #endregion Enums

        #region Methods

        /// <summary>
        /// Gets a topic history, based on the topic history id
        /// </summary>
        /// <param name="topicHistoryId">the id of the topic history to collect</param>
        /// <returns>returns a topic history object</returns>
        internal TopicHistory GetItem(int topicHistoryId)
        {
            return this.db.ExecuteQuery<TopicHistory>(CommandType.StoredProcedure, "Wiki_TopicHistoryGet", topicHistoryId).FirstOrDefault();
        }

        internal override void RepositoryDelete(ref TopicHistory entity)
        {
            this.db.Execute(CommandType.StoredProcedure, "Wiki_TopicHistoryDelete", entity.TopicId);
        }

        internal override void Entity_EvaluateSqlException(
            SqlException exc,
            SharedEnum.CrudOperation crudOperation)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the previous versions of the topic passed has parameter
        /// </summary>
        /// <param name="topicID">the topic to evaluate</param>
        /// <returns>returns collection of topics</returns>
        internal IEnumerable<TopicHistory> GetHistoryForTopic(int topicID)
        {
            return this.db.ExecuteQuery<TopicHistory>(CommandType.StoredProcedure, "Wiki_TopicHistoryGetHistoryForTopic", topicID);
        }

        #endregion Methods
    }
}