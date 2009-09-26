using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtnEmall.Server.Base
{
    public interface IValidator
    {
        bool ValidatePermission(string sessionId, string action, string businessEntity);
    }
}
