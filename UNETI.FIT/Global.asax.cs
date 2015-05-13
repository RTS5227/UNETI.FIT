using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using WebMatrix.WebData;
using UNETI.FIT.Infrastructure;
using System.Globalization;

namespace UNETI.FIT
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            //ControllerBuilder.Current.SetControllerFactory(new UNETI.FIT.Areas.SubjectManager.Infrastructure.NinjectControllerFactory());
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "ID", "UserName", autoCreateTables: true);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string culture = "vi-VN";//ngon ngu mac dinh
            var httpCookie = Request.Cookies["language"];
            if (httpCookie != null)
            {
                culture = httpCookie.Value;
            }
            else
            {
                HttpCookie language = new HttpCookie("language");
                language.Value = culture;
                language.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(language);
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        }
    }
}