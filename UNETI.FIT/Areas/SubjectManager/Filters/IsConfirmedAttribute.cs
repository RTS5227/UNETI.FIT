using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;

namespace UNETI.FIT.Areas.SubjectManager.Filters
{
    public class IsConfirmedAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.IsInRole("admin")) return true;
            if (!httpContext.User.IsInRole("student")) return false;
            var repository = new ConfirmRepository();
            var entry = repository.Get(c => c.StudentID == httpContext.User.Identity.Name);
            if (entry == null) return false;
            return entry.Content == ConfirmEnum.Accept;
        }
    }
}