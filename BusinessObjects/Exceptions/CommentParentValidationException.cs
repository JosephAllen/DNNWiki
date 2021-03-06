﻿#region Copyright

//--------------------------------------------------------------------------------------------------------
// <copyright file="CommentParentValidationException.cs" company="DNN Corp®">
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

using DotNetNuke.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Wiki.BusinessObjects.Exceptions
{
    /// <summary>
    /// The Comment Parent Validation Exception Class which is based on the AbstractValidation
    /// Exception Class
    /// </summary>
    public class CommentParentValidationException : _AbstractValidationException<CommentParentBO.CommentParentError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentParentValidationException"/> class.
        /// </summary>
        /// <param name="crudOperation">The crud operation.</param>
        /// <param name="crudError">The crud error.</param>
        public CommentParentValidationException(SharedEnum.CrudOperation crudOperation, CommentParentBO.CommentParentError crudError)
            : base(crudOperation, crudError)
        {
        }
    }
}