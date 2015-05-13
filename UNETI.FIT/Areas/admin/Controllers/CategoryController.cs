using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Infrastructure.Helper;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Models.ViewModels;

namespace UNETI.FIT.Areas.admin.Controllers
{
    [Authorize(Roles="admin")]
    public class CategoryController : Controller
    {
        private ICategoryRepository cateRepository;
        public CategoryController(ICategoryRepository cate)
        {
            cateRepository = cate;
        }

        //
        // GET: /admin/Category/

        public ActionResult Index(FilterModel filter)
        {
            var result = new ListViewModelBase<Category>();

            var entities = cateRepository
                .GetMany(s => s.Name.Contains(filter.SearchString))
                .Filter(filter);
            result.List = entities.ToList();
            result.Filter = filter;
            return View(result);
        }

        #region CURD

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                cateRepository.Add(model);
                TempData["message"] = string.Format("\"{0}\" đã được lưu thành công", model.Name);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = cateRepository.GetByID(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                cateRepository.Update(model);
                TempData["message"] = string.Format("\"{0}\" đã được lưu thành công", model.Name);
                return RedirectToIndex();
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = cateRepository.GetByID(id);
            if (model != null)
            {
                cateRepository.Delete(model);
                TempData["message"] = string.Format("{0} đã được xóa thành công.", model.Name);
            }
            return RedirectToIndex();
        }

        public FileResult Export()
        {
            return new MF.Dev.ExportHelper.ExportHelper
                .ExcelExportHelper<Category>("Report", Resources.Resource.Category, cateRepository.GetAll().ToList())
                .CreateSheet();
        }

        #endregion

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
