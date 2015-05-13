using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebMatrix.WebData;
using UNETI.FIT.Models;
using UNETI.FIT.Models.ViewModels;

namespace UNETI.FIT.Controllers
{
    [Authorize]
    public class NewsManagerController : Controller
    {
        private NewsContext db = new NewsContext();

        //
        // GET: /NewsManager/

        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.Category);
            return View(articles.ToList());
        }

        //
        // GET: /NewsManager/Details/5

        public ActionResult Details(int id = 0)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        //
        // GET: /NewsManager/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        //
        // POST: /NewsManager/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleViewModel articleVM)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Upload/"), fileName);
                        file.SaveAs(path);
                        articleVM.ThumbUrl = fileName;
                    }
                }

                var article = articleVM.ToDomainMode();
                UpdateUserProfile(article);
                UpdateCategory(article, articleVM.Category);
               // UpdateTags(article, articleVM.Tags);

                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", articleVM.Category.CategoryId);
            return View(articleVM);
        }

        private void UpdateUserProfile(Article article)
        {
            article.Author = db.UserDetails.Single(u => u.UserName == User.Identity.Name);
        }

        private void UpdateCategory(Article article, Category category)
        {
            article.Category = category;
        }

        private void UpdateTags(Article artile, IEnumerable<Tag> tags)
        {
            if (tags == null) return;
            if (artile.ArticleId == 0)
            {
                //new article
                foreach (Tag tag in tags)
                {
                    artile.Tags.Add(db.Tags.Find(tag.TagId));
                }
            }
            else
            {
                //exist article
                foreach (var tag in artile.Tags.ToList())
                {
                    artile.Tags.Remove(tag);
                }
                foreach (var tag in tags)
                {
                    artile.Tags.Add(db.Tags.Find(tag.TagId));
                }
            }
        }

        //
        // GET: /NewsManager/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", article.Category.CategoryId);
            return View(article);
        }

        //
        // POST: /NewsManager/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                //article.Author = User.Identity.Name.ToString();
                article.UpdateAt = DateTime.Now;

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Upload/"), fileName);
                        file.SaveAs(path);
                        article.ThumbUrl = fileName;
                    }
                }

                if (article.Pin == true && db.Articles.Count() > 1)
                {
                    var article_old = db.Articles.Single(a => a.Pin == true);
                    if (article.ArticleId != article_old.ArticleId && article_old != null)
                    {
                        article_old.Pin = false;
                        db.Entry(article_old).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", article.Category.CategoryId);
            return View(article);
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file !=null)
            {
                var path = "~/Content/Upload" + file.FileName;
                file.SaveAs(path);
            }
            return View();
        }

        //
        // GET: /NewsManager/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        //
        // POST: /NewsManager/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}