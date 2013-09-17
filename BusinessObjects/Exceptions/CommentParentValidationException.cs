using DotNetNuke.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Wiki.BusinessObjects.Exceptions
{
    public class CommentParentValidationException : _AbstractValidationException<CommentParentBO.CommentParentError>
    {
        public CommentParentValidationException(SharedEnum.CrudOperation crudOperation, CommentParentBO.CommentParentError crudError)
            : base(crudOperation, crudError)
        {
        }
    }
}