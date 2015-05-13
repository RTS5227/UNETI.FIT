using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Models.Entity;

namespace UNETI.FIT.Areas.admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        private IContactProcessor contactProcessor;
        public DashboardController(IContactProcessor contact)
        {
            contactProcessor = contact;
        }
        //
        // GET: /admin/Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(Email model)
        {
            try
            {
                contactProcessor.ProcessContact(model);
                TempData["message"] = "Successfully Send...";
            }
            catch (Exception ex)
            {
                TempData["message"] = "Sending Failed...";
            }
            return RedirecToIndex();
        }

        private ActionResult RedirecToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
