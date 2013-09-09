using DotNetNuke.Modules.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Exceptions
{
    public class TopicValidationException : _AbstractValidationException<TopicBO.TopicError>
    {
        public TopicValidationException(SharedEnum.CrudOperation crudOperation, TopicBO.TopicError crudError)
            : base(crudOperation, crudError)
        {
        }
    }
}