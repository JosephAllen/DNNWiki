﻿using DotNetNuke.Modules.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Exceptions
{
    public class CommentValidationException : _AbstractValidationException<CommentBO.CommentError>
    {
        public CommentValidationException(SharedEnum.CrudOperation crudOperation, CommentBO.CommentError crudError)
            : base(crudOperation, crudError)
        {
        }
    }
}