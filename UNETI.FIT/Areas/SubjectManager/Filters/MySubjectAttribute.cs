using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;

namespace UNETI.FIT.Areas.SubjectManager.Filters
{
    public class MySubjectAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.IsInRole("admin")) return true;
            if (!httpContext.User.IsInRole("teacher")) return false;
            var subjectRepository = new SubjectRepository();

            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized) return false;

            var routeData = httpContext.Request.RequestContext.RouteData;
            if (!routeData.Values.ContainsKey("id")) return false;

            int id = Int32.Parse(routeData.Values["id"].ToString());
            var username = httpContext.User.Identity.Name;
            var model = subjectRepository.GetByID(id);
            if (model == null) return false;
            return model.TeacherID == username;
        }
    }
}