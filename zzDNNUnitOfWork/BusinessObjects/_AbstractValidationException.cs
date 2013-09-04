using DotNetNuke.Modules.DNNUnitOfWork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.DNNUnitOfWork.BusinessObjects
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