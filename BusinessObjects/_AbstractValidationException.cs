#region Copyright

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

using DotNetNuke.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Wiki.BusinessObjects
{
    /// <summary>
    /// Generic abstract class for handling Business objects error
    /// </summary>
    /// <typeparam name="E">will be replaced by a enum refering the type of errors a business object
    /// can throw on a crud operation</typeparam>
    public abstract class _AbstractValidationException<E> : Exception
    {
        private SharedEnum.CrudOperation _crudOperation;
        private E _crudError;

        public _AbstractValidationException(SharedEnum.CrudOperation crudOperation, E crudError)
            : base(string.Empty)
        {
            this._crudOperation = crudOperation;
            this._crudError = crudError;
        }

        /// <summary>
        /// The crud operation that generated the error
        /// </summary>
        public SharedEnum.CrudOperation CrudOperation
        {
            get
            {
                return _crudOperation;
            }
        }

        /// <summary>
        /// The error that occurred
        /// </summary>
        public E CrudError
        {
            get
            {
                return this._crudError;
            }
        }
    }
}