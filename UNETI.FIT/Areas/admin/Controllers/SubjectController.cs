using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Areas.SubjectManager.Models;
using UNETI.FIT.Areas.SubjectManager.Filters;
using UNETI.FIT.Areas.SubjectManager.Infrastructure;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Helpers;

using UNETI.FIT.Models.ViewModels;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Infrastructure.Helper;

using UNETI.FIT.Infrastructure.Concrete;
using System.IO;
using System.Configuration;
using UNETI.FIT.Infrastructure;

namespace UNETI.FIT.Areas.admin.Controllers
{
    [Authorize(Roles="admin")]
    public class SubjectController : Controller
    {
        private ISubjectRepository subjectRepository;

        public SubjectController(ISubjectRepository isr)
        {
            subjectRepository = isr;
        }

        //
        // GET: /Subject/

        public ActionResult Index(FilterModel filter)
        {
            var result = new ListViewModelBase<Subject>();

            var entities = subjectRepository
                .GetMany(s => s.Title.Contains(filter.SearchString)); ;

            if (filter.Category >= 1)
            {
                entities = entities
                    .Where(s => s.ModuleID == filter.Category);
            }

            result.List = entities.Filter(filter).ToList();
            result.Filter = filter;
            using (var db = new ApplicationDBContext())
            {
                result.List.ForEach(a => a.Module = db.Modules.Find(a.ModuleID));
                result.List.ForEach(a => a.Teacher = db.Teachers.Find(a.TeacherID));
            }
            return View(result);
        }

        #region CURD

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Subject());
        }

        [HttpPost]
        public ActionResult Create(Subject model)
        {
            if (ModelState.IsValid)
            {
                model.CreateAt = DateTime.Now;
                subjectRepository.Add(model);
                TempData["message"] = string.Format("\"{0}\" đã được lưu thành công", model.Title);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Subject model = subjectRepository.GetByID(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Subject model)
        {
            if (ModelState.IsValid)
            {
                subjectRepository.Update(model);
                TempData["message"] = string.Format("\"{0}\" đã được lưu thành công", model.Title);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Subject model = subjectRepository.GetByID(id);
            if (model != null)
            {
                subjectRepository.Delete(model);
                TempData["message"] = string.Format("{0} đã được xóa thành công.", model.Title);
            }
            return RedirectToIndex();
        }

        #endregion

        [HttpGet]
        public FileResult Export(Type type)
        {
            var list = subjectRepository.GetAll().ToList();
            var file = new MF.Dev.ExportHelper.ExportHelper.ExcelExportHelper<Subject>("Report", Resources.Resource.Subject, list).CreateSheet();
            return file;
        }

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
