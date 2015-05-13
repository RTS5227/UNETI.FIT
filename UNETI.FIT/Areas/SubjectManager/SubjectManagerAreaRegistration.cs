using System.Web.Mvc;

namespace UNETI.FIT.Areas.SubjectManager
{
    public class SubjectManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SubjectManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SubjectManager_default_1",
                "do-an/{id}",
                new { controller = "Subject", action = "ChiTiet", id = UrlParameter.Optional },
                new { id = @"\d+" }
            );

            context.MapRoute(
                "SubjectManager_default",
                "do-an/{action}/{id}",
                new { controller = "Subject", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "SubjectManager_default_3",
                "giaovien/{id}",
                new { controller = "Teacher", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "SubjectManager_default_4",
                "sinhvien/{id}",
                new { controller = "Student", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "SubjectManager_default5",
                "SubjectManager/{controller}/{action}/{id}",
                new { controller = "Subject", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
