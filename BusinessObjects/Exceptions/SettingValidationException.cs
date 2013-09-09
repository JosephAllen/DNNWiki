using DotNetNuke.Modules.Wiki.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetNuke.Modules.Wiki.BusinessObjects.Exceptions
{
    public class SettingValidationException : _AbstractValidationException<SettingBO.SettingError>
    {
        public SettingValidationException(SharedEnum.CrudOperation crudOperation, SettingBO.SettingError crudError)
            : base(crudOperation, crudError)
        {
        }
    }
}