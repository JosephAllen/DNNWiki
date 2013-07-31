using Christoc.Modules.DNNWikiTestVersion.Components.Business;
using Christoc.Modules.DNNWikiTestVersion.Components.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Christoc.Modules.DNNWikiTestVersion.Components.Exceptions
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