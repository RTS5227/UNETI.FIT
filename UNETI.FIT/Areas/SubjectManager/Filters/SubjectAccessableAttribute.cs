using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;

namespace UNETI.FIT.Areas.SubjectManager.Filters
{
    public class SubjectAccessableAttribute : ActionFilterAttribute
    {
        private ActionExecutingContext context;
        private Subject entry = new Subject();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            context = filterContext;
            if (
                GetableSubject()
                && (
                    IsAdminOrPublic()
                    || (StudentAuthorize() && TeacherAuthorize())
                    )
                )
            {
                context.Controller.ViewData.Model = entry;
            }
            else
            {
                context.Controller.ViewData.ModelState.AddModelError("permission", "");
            }
            base.OnActionExecuting(filterContext);
        }

        private bool GetableSubject()
        {
            var routeData = context.RequestContext.RouteData;
            if (!routeData.Values.ContainsKey("id")) return false;

            int id = Int32.Parse(routeData.Values["id"].ToString());
            var repository = new SubjectRepository();
            entry = repository.GetByID(id);
            if (entry != null) return true;
            return false;
        }

        private bool IsAdminOrPublic()
        {
            return context.HttpContext.User.IsInRole("admin")
                || entry.Status == Models.Entity.SubjectStatus.Public;
        }

        private bool StudentAuthorize()
        {
            if (context.HttpContext.User.IsInRole("student"))
            {
                var confirm = entry.Confirms
                    .Where(c => c.StudentID == context.HttpContext.User.Identity.Name)
                    .FirstOrDefault();
                if (confirm != null) return confirm.Content == ConfirmEnum.Accept;
            }
            return false;
        }

        private bool TeacherAuthorize()
        {
            if (context.HttpContext.User.IsInRole("teacher"))
            {
                return entry.TeacherID == context.HttpContext.User.Identity.Name;
            }
            return false;
        }
    }
}