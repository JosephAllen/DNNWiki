using DotNetNuke.Modules.DNNUnitOfWork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.DNNUnitOfWork.BusinessObjects.Exceptions
{
    public class ItemValidationException : _AbstractValidationException<ItemBO.ItemError>
    {
        public ItemValidationException(SharedEnum.CrudOperation crudOperation, ItemBO.ItemError crudError)
            : base(crudOperation, crudError)
        {
        }
    }
}