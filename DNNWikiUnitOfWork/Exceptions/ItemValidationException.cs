using DotNetNuke.Modules.DNNWikiUnitOfWork.Components;
using DotNetNuke.Modules.DNNWikiUnitOfWork.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.DNNWikiUnitOfWork.Exceptions
{
    public class ItemValidationException : Exception
    {
        private SharedEnum.CrudOperation _crudOperation;
        private ItemBO.ItemError _error;

        public ItemValidationException(SharedEnum.CrudOperation crudOperation, ItemBO.ItemError error)
            : base(string.Empty)
        {
            this._crudOperation = crudOperation;
            this._error = error;
        }

        #region IValidationException Members

        public SharedEnum.CrudOperation CrudOperation()
        {
            return _crudOperation;
        }

        public ItemBO.ItemError Error()
        {
            return _error;
        }

        #endregion IValidationException Members
    }
}