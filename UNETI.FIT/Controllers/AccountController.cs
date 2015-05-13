using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using UNETI.FIT.Filters;
using UNETI.FIT.Models;
using UNETI.FIT.Models.ViewModels;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Infrastructure.Helper;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;

namespace UNETI.FIT.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IStudentRepository studentRepository;
        private ITeacherRepository teacherRepository;

        public AccountController(IStudentRepository r1, ITeacherRepository r2)
        {
            studentRepository = r1;
            teacherRepository = r2;
        }
        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal();
            }
            return View();
        }

        //
        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                if (string.IsNullOrWhiteSpace(User.Identity.Name)) return Redirect(Request.UrlReferrer.ToString());
                return RedirectToLocal();
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Subject", new { area= "subjectmanager"});
        }


        #region Helpers
        private ActionResult RedirectToLocal()
        {
            if (User.IsInRole("admin"))
                return RedirectToActionPermanent("index", "dashboard", new { area = "admin" });
            return RedirectToActionPermanent("index", "subject", new { area = "SubjectManager" });
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
