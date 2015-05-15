using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Areas.SubjectManager.Models;
using UNETI.FIT.Areas.SubjectManager.Filters;
using WebMatrix.WebData;
using System.Web.Security;

using UNETI.FIT.Areas.SubjectManager.Infrastructure;
using UNETI.FIT.Models.ViewModels;
using UNETI.FIT.Infrastructure;
using UNETI.FIT.Controllers;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Infrastructure.Helper;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Helpers;
using System.IO;
using System.Configuration;

namespace UNETI.FIT.Areas.admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class StudentController : Controller
    {
        private IStudentRepository studentRepository;

        public StudentController(IStudentRepository repo)
        {
            studentRepository = repo;
        }

        //
        // GET: /Student/

        public ViewResult Index(FilterModel filter)
        {
            var result = new ListViewModelBase<Student>();

            var entities = studentRepository
                .GetMany(s => s.FullName.Contains(filter.SearchString))
                .Filter(filter)
                .ToList();
            result.List = entities;
            result.Filter = filter;
            using (var db = new ApplicationDBContext())
            {
                result.List.ForEach(a => a.LevelOfTraining = db.LevelsOfTraining.Find(a.LevelOfTrainingID));
                result.List.ForEach(a => a.ClassOfStudy = db.ClassesOfStudy.Find(a.ClassOfStudyID));
                result.List.ForEach(a => a.Faculty = db.Faculties.Find(a.FacultyID));
            }
            return View(result);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = studentRepository.GetByID(id);
            if (model == null) return HttpNotFound();

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Student model)
        {
            if (ModelState.IsValid)
            {
                studentRepository.Update(model);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Student());
        }

        [HttpPost]
        public ActionResult Create(Student model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(model.ID, model.ID);
                    Roles.AddUserToRole(model.ID, "student");

                    studentRepository.Add(model);
                    TempData["message"] = string.Format("Thêm sinh viên {0} thành công", model.FullName);
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
            var model = studentRepository.GetByID(id);
            if (model != null)
            {
                studentRepository.Delete(model);
                TempData["message"] = string.Format("{0} đã được xóa thành công.", model.FullName);
            }
            return RedirectToIndex();
        }

        [HttpGet]
        public FileResult Export(Type type)
        {
            var list = studentRepository.GetAll().ToList();
            var file = new MF.Dev.ExportHelper.ExportHelper.ExcelExportHelper<Student>("Report", Resources.Resource.Student, list).CreateSheet();
            return file;
        }

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
