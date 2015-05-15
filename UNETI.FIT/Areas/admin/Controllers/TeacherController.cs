using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Areas.SubjectManager.Models;
using UNETI.FIT.Areas.SubjectManager.Filters;
using System.Web.Security;
using WebMatrix.WebData;
using UNETI.FIT.Models.ViewModels;
using UNETI.FIT.Controllers;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Infrastructure.Helper;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Helpers;
using System.IO;
using System.Configuration;
using UNETI.FIT.Infrastructure;

namespace UNETI.FIT.Areas.admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class TeacherController : Controller
    {
        private ITeacherRepository teacherRepository;
        private ISubjectRepository subjectRepository;

        public TeacherController(ITeacherRepository repo, ISubjectRepository repo2)
        {
            teacherRepository = repo;
            subjectRepository = repo2;
        }

        //
        // GET: /Teacher/

        public ViewResult Index(FilterModel filter)
        {
            var result = new ListViewModelBase<Teacher>();

            var entities = teacherRepository
                .GetMany(s => s.FullName.Contains(filter.SearchString))
                .Filter(filter)
                .ToList();
            result.List = entities;
            result.Filter = filter;
            return View(result);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = teacherRepository.GetByID(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Teacher model)
        {
            if (ModelState.IsValid)
            {
                teacherRepository.Update(model);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Teacher());
        }

        [HttpPost]
        public ActionResult Create(Teacher model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.ID, model.ID);
                    Roles.AddUserToRole(model.ID, "teacher");

                    teacherRepository.Add(model);
                    TempData["message"] = string.Format("Thêm giáo viên {0} thành công", model.FullName);
                    return RedirectToIndex();
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", AccountController.ErrorCodeToString(e.StatusCode));
                }
            }
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var model = teacherRepository.GetByID(id);
            if (model != null)
            {
                subjectRepository.DeleteMany(a => a.TeacherID == model.ID);
                teacherRepository.Delete(model);
                TempData["message"] = string.Format("{0} đã được xóa thành công.", model.FullName);
            }
            return RedirectToIndex();
        }

        [HttpGet]
        public FileResult Export(Type type)
        {
            var list = teacherRepository.GetAll().ToList();
            var file = new MF.Dev.ExportHelper.ExportHelper.ExcelExportHelper<Teacher>("Report", Resources.Resource.Teacher, list).CreateSheet();
            return file;
        }

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
