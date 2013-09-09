using DotNetNuke.Modules.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Exceptions
{
    public class TopicHistoryValidationException : _AbstractValidationException<TopicHistoryBO.TopicHistoryError>
    {
        public TopicHistoryValidationException(SharedEnum.CrudOperation crudOperation, TopicHistoryBO.TopicHistoryError crudError)
            : base(crudOperation, crudError)
        {
        }
    }
}