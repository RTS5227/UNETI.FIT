using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;

using UNETI.FIT.Models;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Models.ViewModels;
using System.IO;

namespace UNETI.FIT.Infrastructure.Helper
{
    public static class ViewModelHelper
    {
        public static int pageSize = Int32.Parse(ConfigurationManager.AppSettings["MaximumItemsPerPage"]);
        
        public static IEnumerable<Category> Filter(this IQueryable<Category> entities, FilterModel filter)
        {
            //Sort
            switch (filter.SortOrder)
            {
                case "Name":
                    entities = entities.OrderBy(s => s.Name);
                    break;
                case "Name_desc":
                    entities = entities.OrderByDescending(s => s.Name);
                    break;
                default:
                    entities = entities.OrderBy(s => s.Name);
                    break;
            }

            //Pagination
            filter.TotalItems = entities.Count();
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalItems / filter.PageSize);

            entities = entities
                .Skip(filter.PageSize * (filter.PageIndex - 1))
                .Take(filter.PageSize);

            return entities.ToList();
        }

        public static IEnumerable<Article> Filter(this IQueryable<Article> entities, FilterModel filter)
        {
            //Sort
            switch (filter.SortOrder)
            {
                case "Name":
                    entities = entities.OrderBy(s => s.Title);
                    break;
                case "Name_desc":
                    entities = entities.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    entities = entities.OrderBy(s => s.CreateAt);
                    break;
                case "Date_desc":
                    entities = entities.OrderByDescending(s => s.CreateAt);
                    break;
                default:
                    entities = entities.OrderBy(s => s.CreateAt);
                    break;
            }

            //Pagination
            filter.TotalItems = entities.Count();
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalItems / filter.PageSize);

            entities = entities
                .Skip(filter.PageSize * (filter.PageIndex - 1))
                .Take(filter.PageSize);

            return entities.ToList();
        }

        public static Attachment ToDomain(this HttpPostedFileBase file)
        {
            return new Attachment
            {
                Name = Path.GetFileName(file.FileName),
                Length = file.ContentLength,
                Type = file.ContentType
            };
        }

        public static ArticleViewModel ToViewModel(this Article article)
        {
            var articleViewModel = new ArticleViewModel
            {
                ID = article.ID,
                CreateAt = article.CreateAt,
                UpdateAt = article.UpdateAt,
                Pin = article.Pin,
                Content = article.Content,
                Title = article.Title,
                View = article.View,

                CategoryID = article.CategoryID,
                Category = article.Category,

                Thumbnail = article.Thumbnail
            };

            //convert all tags in article to assigned tag
            foreach (var tag in article.Tags)
            {
                var assignedTag = new AssignedTagData
                {
                    TagID = tag.TagID,
                    TagDescription = tag.TagDescription,
                    Assigned = true
                };
                articleViewModel.Tags.Add(assignedTag);
            }
            return articleViewModel;
        }

        public static ArticleViewModel ToViewModel(this Article article, IEnumerable<Tag> allDbTags)
        {
            var articleViewModel = new ArticleViewModel
            {
                ID = article.ID,
                CreateAt = article.CreateAt,
                UpdateAt = article.UpdateAt,
                Pin = article.Pin,
                Content = article.Content,
                Title = article.Title,
                View = article.View,

                CategoryID = article.CategoryID,
                Category = article.Category,

                Thumbnail = article.Thumbnail
            };

            //convert all tags in db to assigned tag
            foreach (var tag in allDbTags)
            {
                articleViewModel.Tags.Add(new AssignedTagData
                {
                    TagID = tag.TagID,
                    TagDescription = tag.TagDescription,
                    //true if article already has tag
                    Assigned = article.Tags.FirstOrDefault(x => x.TagID == tag.TagID) != null
                });
            }
            return articleViewModel;
        }
    }
}