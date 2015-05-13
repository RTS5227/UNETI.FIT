using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using UNETI.FIT;
using UNETI.FIT.Models;
using UNETI.FIT.Models.ViewModels;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Infrastructure.Helper;
using UNETI.FIT.Models.Entity;
using System.Configuration;
using System.IO;

namespace UNETI.FIT.Controllers
{
    public class HomeController : Controller
    {
        private IArticleRepository articleRepository;
        private ICategoryRepository categoryRepository;
        private ITagRepository tagRepository;
        //private IAttachmentRepository fileRepository;

        public HomeController(IArticleRepository a,
            ICategoryRepository c,
            ITagRepository t
            )
        {
            articleRepository = a;
            categoryRepository = c;
            tagRepository = t;
        }

        public ActionResult Index(int? id)
        {
            var result = new ArticleViewModel();
            int k = (id.HasValue && id.Value > 0) ? id.Value : 0;

            var entities = articleRepository
                .GetMany(s => s.Pin && s.Status == ArticleStatus.Public)
                .OrderByDescending(s => s.CreateAt);
            int sum = entities.Count();

            if (sum > 0)
            {
                if (sum <= k + 1)
                {
                    result = entities.Skip(sum - 1).First().ToViewModel();
                    k = sum - 1;
                }
                else result = entities
                        .Skip(k)
                        .First()
                        .ToViewModel();
            }
            ViewData["current"] = k;
            return View(result);
        }

        [HttpGet]
        public ActionResult Search(FilterModel filter)
        {
            var result = new ListViewModelBase<ArticleViewModel>();
            result.Filter = filter;

            var entities = articleRepository
                .GetMany(s => s.Title.Contains(filter.SearchString))
                .Filter(filter)
                .ToList();

            if (entities.Count() > 0)
            {
                result.List = entities.Select(s => s.ToViewModel()).ToList();
            }

            return View(result);
        }

        public ActionResult Announcement(CategoryTypeEnum type)
        {
            return Category(type);
        }

        public ActionResult Category(CategoryTypeEnum type)
        {
            var result = new List<CategoryViewModel>();
            int MAX_ARTICLE = Int16.Parse(ConfigurationManager.AppSettings["MaximumItemsPerPage"]);

            var entities = categoryRepository
                .GetMany(c => c.Type == type)
                .ToList();
            foreach (Category c in entities)
            {
                var list = articleRepository
                    .GetMany(a => a.CategoryID == c.ID && a.Status == ArticleStatus.Public)
                    .OrderByDescending(a => a.CreateAt)
                    .Take(MAX_ARTICLE)
                    .ToList();

                var item = list.Count() > 0
                        ? list.Select(l => l.ToViewModel()).ToList()
                        : new List<ArticleViewModel>();

                result.Add(new CategoryViewModel
                {
                    ID = c.ID,
                    Name = c.Name,
                    Articles = item
                });
            }
            return PartialView(result);
        }

        [HttpPost]
        public string Upload(string name)
        {
            if (Request.Files.Count == 0) return "";
            HttpPostedFileBase model = Request.Files[0];
            if (model == null || model.ContentLength < 10) return "";

            name = Path.GetFileName(model.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/Upload/"), name);
            model.SaveAs(path);
            return name;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            int articleID = id;
            var result = new Article();
            result = articleRepository.GetByID(articleID);
            if (result == null) return HttpNotFound();
            result.View++;
            articleRepository.Update(result);
            return View(result);
        }

        public ActionResult Menu()
        {
            var result = new List<String>();
            result = categoryRepository
                .GetMany(c => c.Type == CategoryTypeEnum.Article)
                .Select(c => c.Name)
                .ToList();
            return PartialView(result);
        }

        //
        // GET: /ChangeLanguage/

        public ActionResult ChangeLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var httpCookie = Request.Cookies["language"];
                if (httpCookie != null)
                {
                    var cookie = Response.Cookies["language"];
                    if (cookie != null) cookie.Value = culture;
                }
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}