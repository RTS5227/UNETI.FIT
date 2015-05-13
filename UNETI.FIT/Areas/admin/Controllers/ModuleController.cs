using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Helpers;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Models.ViewModels;

namespace UNETI.FIT.Areas.admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ModuleController : Controller
    {
        private IModuleRepository moduleRepository;
        public ModuleController(IModuleRepository repo1)
        {
            moduleRepository = repo1;
        }
        //
        // GET: /admin/Module/

        public ActionResult Index(FilterModel filter)
        {
            var result = new ListViewModelBase<Module>();

            var entities = moduleRepository
                .GetMany(s => s.Name.Contains(filter.SearchString))
                .Filter(filter);
            result.List = entities.ToList();
            result.Filter = filter;
            return View(result);
        }

        [HttpGet]
        public FileResult Export(Type type)
        {
            var list = moduleRepository.GetAll().ToList();
            var file = new MF.Dev.ExportHelper.ExportHelper.ExcelExportHelper<Module>("Report", Resources.Resource.Module, list).CreateSheet();
            return file;
        }

        #region CURD

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Module());
        }

        [HttpPost]
        public ActionResult Create(Module model)
        {
            if (ModelState.IsValid)
            {
                moduleRepository.Add(model);
                TempData["message"] = string.Format("\"{0}\" đã được lưu thành công", model.Name);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = moduleRepository.GetByID(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Module model)
        {
            if (ModelState.IsValid)
            {
                moduleRepository.Update(model);
                TempData["message"] = string.Format("\"{0}\" đã được lưu thành công", model.Name);
                return RedirectToIndex();
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = moduleRepository.GetByID(id);
            if (model != null)
            {
                moduleRepository.Delete(model);
                TempData["message"] = string.Format("{0} đã được xóa thành công.", model.Name);
            }
            return RedirectToIndex();
        }

        #endregion

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
