using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete
{
    public class FormAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            //bool result = FormsAuthentication.Authenticate(username, password);
            //if (result) FormsAuthentication.SetAuthCookie(username, false);
            //return result;
            return true;
        }
    }

    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}