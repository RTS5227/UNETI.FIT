using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using UNETI.FIT.Filters;
using UNETI.FIT.Models.ViewModels;
using UNETI.FIT.Models;
using UNETI.FIT.Infrastructure;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Infrastructure.Helper;
using UNETI.FIT.Models.Entity;
using System.Configuration;
using UNETI.FIT.Areas.SubjectManager.Filters;

namespace UNETI.FIT.Areas.admin.Controllers
{
    [Authorize(Roles = "admin")]
    public class ArticleController : Controller
    {
        private IArticleRepository articleRepository;
        private ITagRepository tagRepository;
        private ICategoryRepository categoryRepository;

        public ArticleController(IArticleRepository a, ICategoryRepository c, ITagRepository t)
        {
            articleRepository = a;
            categoryRepository = c;
            tagRepository = t;
        }

        //
        // GET: /Article/

        public ActionResult Index(FilterModel filter)
        {
            var result = new ListViewModelBase<ArticleViewModel>();
            result.Filter = filter;

            var entities = articleRepository
                .GetMany(s => s.Title.Contains(filter.SearchString))
                .Filter(filter)
                .ToList();

            if (entities.Count > 0)
            {
                result.List = entities.Select(s => s.ToViewModel()).ToList();
            }

            return View(result);
        }

        //
        // GET: /Article/Create
        public ActionResult Create()
        {
            var model = new ArticleViewModel
            {
                Tags = PopulateTagsData()
            };
            return View(model);
        }

        //
        // POST: /Article/Create

        [HttpPost]
        public ActionResult Create(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entry = new Article
                {
                    Title = model.Title,
                    Content = model.Content,
                    Pin = model.Pin,
                    Status = model.Status,
                    CategoryID = model.CategoryID,
                    Thumbnail = model.Thumbnail,

                    CreateAt = DateTime.Now,
                    View = 0
                };

                //Tag
                AddOrUpdateTags(entry, model.Tags);

                articleRepository.Add(entry);
                return RedirectToIndex();
            }

            return View(model);
        }

        //
        // GET: /Article/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var dbEntry = articleRepository.GetByID(id);
            if (dbEntry == null) return HttpNotFound();
            var viewModel = dbEntry.ToViewModel(tagRepository.GetAll());
            return View(viewModel);
        }

        //
        // POST: /Article/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleViewModel model)
        {
            var dbEntry = articleRepository.GetByID(model.ID);
            if (ModelState.IsValid && dbEntry != null)
            {
                var entry = new Article
                {
                    ID = dbEntry.ID,
                    View = dbEntry.View,
                    CreateAt = dbEntry.CreateAt,

                    Thumbnail = model.Thumbnail,
                    Content = model.Content,
                    Pin = model.Pin,
                    Status = model.Status,
                    CategoryID = model.CategoryID,
                    Title = model.Title,
                    UpdateAt = DateTime.Now
                };

                //tags
                AddOrUpdateTags(entry, model.Tags);

                //save
                articleRepository.Update(entry);
                return RedirectToIndex();
            }
            return View(model);
        }

        //
        // GET: /Article/Delete/5

        [HttpGet]
        public ActionResult Delete(int id = 0)
        {
            var dbEntry = articleRepository.GetByID(id);
            if (dbEntry == null) return HttpNotFound();
            return View(dbEntry.ToViewModel());
        }

        //
        // POST: /Article/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var dbEntry = articleRepository.GetByID(id);
            if (dbEntry == null) return HttpNotFound();
            articleRepository.Delete(dbEntry);
            return RedirectToIndex();
        }

        [HttpGet]
        public FileResult Export()
        {
            var list = articleRepository.GetAll().ToList();
            return new MF.Dev.ExportHelper.ExportHelper.
                ExcelExportHelper<Article>("Report", Resources.Resource.Subject, list)
                .CreateSheet();
        }

        #region[HELPER]

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }


        private ICollection<AssignedTagData> PopulateTagsData()
        {
            var tags = tagRepository.GetAll();
            var assignedTags = new List<AssignedTagData>();
            foreach (var tag in tags)
            {
                assignedTags.Add(new AssignedTagData
                {
                    TagID = tag.TagID,
                    TagDescription = tag.TagDescription,
                    Assigned = false
                });
            }
            return assignedTags;
        }

        private void AddOrUpdateTags(Article article, IEnumerable<AssignedTagData> assignedTags)
        {
            if (assignedTags == null) return;
            var context = new ApplicationDBContext();
            if (article.ID == 0)
            {
                //new article
                foreach (var atag in assignedTags.Where(x => x.Assigned == true))
                {
                    //using attach for save db query
                    var tag = new Tag { TagID = atag.TagID };
                    context.Tags.Attach(tag);
                    article.Tags.Add(tag);
                    context.Articles.Add(article);
                }
            }
            else
            {
                //exist article
                foreach (var tag in article.Tags.ToList())
                {
                    article.Tags.Remove(tag);
                }
                foreach (var tag in assignedTags.Where(a => a.Assigned == true))
                {
                    article.Tags.Add(tagRepository.GetByID(tag.TagID));
                }
                context.Entry(article).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        private void AddOrUpdateKeepExistingTags(Article article, IEnumerable<AssignedTagData> assignedTags)
        {
            var webTagAssignedIDs = assignedTags.Where(a => a.Assigned).Select(webTag => webTag.TagID);
            var dbTagIDs = article.Tags.Select(d => d.TagID);
            var tagIDs = dbTagIDs as int[] ?? dbTagIDs.ToArray();
            var tagToDeleteIDs = tagIDs.Where(id => !webTagAssignedIDs.Contains(id)).ToList();

            //delete removed tags
            foreach (var id in tagToDeleteIDs)
            {
                article.Tags.Remove(tagRepository.GetByID(id));
            }

            //add tags that user does't already have
            foreach (var id in webTagAssignedIDs)
            {
                if (!tagIDs.Contains(id))
                {
                    article.Tags.Add(tagRepository.GetByID(id));
                }
            }
        }
        #endregion
    }
}